using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Helpers;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class FollowUpHeightAndWeightModel : FollowUpNumbersPageModel
    {
        protected override string BeRemindedErrorMessage => "Select yes if you would like to be reminded by email to get your height and weight measurements checked";

        protected override string ConfidentToChangeErrorMessage => "Select how confident you are in in making a plan to get your height and weight measured";

        protected override string CurrentRoute => "heightandweight";

        protected override string DoYouHavePlansErrorMessage => "Select yes if you have any plans to get your height and weight measurements in the following months";

        protected override string ImportantToChangeErrorMessage => "Select how important is it for you get your height and weight measured";

        protected override string SelectedOptionErrorMessage => "Select how you would like to get your height and weight measured";

        protected override string SetADateErrorMessage => "Select yes if you will set a date to get your height and weight measurements";

        public FollowUpHeightAndWeightModel(
            Database database,
            ICredentialsDecrypter credentialsDecrypter,
            IHealthPriorityRouter healthPriorityRouter,
            IPageFlow pageFlow)
            : base(database, credentialsDecrypter, healthPriorityRouter, pageFlow) 
        { }

        protected override async Task LoadPageData()
        {
            await LoadBarriers();

            var check = await GetHealthCheckAsync();

            AskForEmailAddress = string.IsNullOrEmpty(check.EmailAddress);

            if (Model is null) //FollowUp might already have been set, e.g. onPost
            {
                var followUp = await GetFollowUpAsync(check);

                Model = new UnsanitisedModel
                {
                    BeReminded = followUp?.BeReminded.AsYesNoOrNull(),
                    SetADate = followUp?.SetADate.AsYesNoOrNull(),
                    DoYouHavePlans = followUp?.DoYouHavePlans.AsYesNoOrNull(),
                    ImportantToChange = followUp?.ImportantToChange,
                    ConfidentToChange = followUp?.ConfidentToChange,
                    Barriers = check?.ChosenBarriers?.Where(x => x.Category == CurrentRoute).Select(x => x.Id.ToString()).ToList() ?? new List<string>(),
                    OtherBarrier = check?.CustomBarriers?.FirstOrDefault(x => x.Category == CurrentRoute)?.Text,
                    SelectedOption = followUp?.SelectedOption
                };
            }
        }

        protected override async Task<SanitisedModel> ValidateAndSanitise(UnsanitisedModel model)
        {
            //This is mostly a copy of the base implementation, but we don't care about BeReminded for only this follow up.

            var sanitisedModel = new SanitisedModel();

            var isValid = true;

            if (string.IsNullOrEmpty(model.ImportantToChange))
            {
                ImportantToChangeError = ImportantToChangeErrorMessage;
                AddError(ImportantToChangeError, "#important-to-change");
                isValid = false;
            }
            else
            {
                sanitisedModel.ImportantToChange = model.ImportantToChange;
            }

            if (string.IsNullOrEmpty(model.ConfidentToChange))
            {
                ConfidentToChangeError = ConfidentToChangeErrorMessage;
                AddError(ConfidentToChangeError, "#confident-to-change");
                isValid = false;
            }
            else
            {
                sanitisedModel.ConfidentToChange = model.ConfidentToChange;
            }

            if (string.IsNullOrEmpty(model.DoYouHavePlans) || (model.DoYouHavePlans != "yes" && model.DoYouHavePlans != "no"))
            {
                DoYouHavePlansError = DoYouHavePlansErrorMessage;
                AddError(DoYouHavePlansError, "#do-you-have-plans");
                isValid = false;
            }
            else
            {
                sanitisedModel.DoYouHavePlans = model.DoYouHavePlans == "yes";
            }

            if (string.IsNullOrEmpty(model.SetADate) || (model.SetADate != "yes" && model.SetADate != "no"))
            {
                SetADateError = SetADateErrorMessage;
                AddError(SetADateError, "#set-a-date");
                isValid = false;
            }
            else
            {
                sanitisedModel.SetADate = model.SetADate == "yes";
            }

            if (string.IsNullOrEmpty(model.SelectedOption))
            {
                SelectedOptionError = SelectedOptionErrorMessage;

                InsertErrorBefore(SelectedOptionError, "#options", "#important-to-change", "#confident-to-change");

                isValid = false;
            }
            else
            {
                sanitisedModel.SelectedOption = model.SelectedOption;
            }

            var barriers = new List<Barrier>();

            // There is an exclusive "none" option on the barriers checkbox that's there for if you
            // don't want to pick any items. This is clearer for some users than simply leaving the
            // control blank.

            if (!model.Barriers.Any(x => x == "none"))
            {
                foreach (var barrierId in model.Barriers.Except(new[] { "other" }))
                {
                    var barrier = await Database.Barriers.FindAsync(int.Parse(barrierId));

                    barriers.Add(barrier);
                }

                if (model.Barriers.Contains("other"))
                {
                    if (string.IsNullOrEmpty(model.OtherBarrier))
                    {
                        BarriersError = OtherBarrierNotEnteredErrorMessage;
                        InsertErrorBefore(BarriersError, "#other-barrier", "#important-to-change", "#confident-to-change");
                        isValid = false;
                    }
                    else if (model.OtherBarrier.Length > 300)
                    {
                        BarriersError = OtherBarrierTooLongErrorMessage;
                        InsertErrorBefore(BarriersError, "#other-barrier", "#important-to-change", "#confident-to-change");
                        isValid = false;
                    }
                    else
                    {
                        sanitisedModel.OtherBarrier = model.OtherBarrier;
                    }
                }
            }

            sanitisedModel.SelectedBarriers = barriers;

            return isValid ? sanitisedModel : null;
        }

        public async Task<IActionResult> OnPostAsync(UnsanitisedModel model)
        {
            Model = model;

            //Need to have pagedata loaded to know if we require an email address
            await LoadPageData();

            var sanitisedModel = await ValidateAndSanitise(model);

            if (sanitisedModel == null)
            {
                return await Reload();
            }

            await SaveData(new FollowUp()
            {
                Type = CurrentRoute,
                BeReminded = sanitisedModel.BeReminded,
                ConfidentToChange = sanitisedModel.ConfidentToChange,
                DoYouHavePlans = sanitisedModel.DoYouHavePlans,
                ImportantToChange = sanitisedModel.ImportantToChange,
                SetADate = sanitisedModel.SetADate,
                SelectedOption = sanitisedModel.SelectedOption
            }, sanitisedModel.EmailAddress, sanitisedModel.SelectedBarriers, sanitisedModel.OtherBarrier);

            return RedirectWithId("./HealthCheckInComplete");
        }

        protected override async Task<FollowUp> GetFollowUpAsync(HealthCheck check)
        {
            await Database.Entry(check).Reference(c => c.HeightAndWeightFollowUp).LoadAsync();

            return check.HeightAndWeightFollowUp;
        }
        protected override void SetFollowUp(HealthCheck check, FollowUp followUp) => check.HeightAndWeightFollowUp = followUp;
    }
}