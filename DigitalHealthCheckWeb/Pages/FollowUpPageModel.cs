using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Helpers;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalHealthCheckWeb.Pages
{
    public abstract class FollowUpPageModel : HealthCheckPageModel
    {
        protected abstract Task<FollowUp> GetFollowUpAsync(HealthCheck check);
        protected abstract void SetFollowUp(HealthCheck check, FollowUp followUp);

        public IList<Components.Pages.Barrier> Barriers { get; set; }

        public string BarriersError { get; set; }

        public UnsanitisedModel Model { get; set; }

        protected virtual string OtherBarrierNotEnteredErrorMessage { get; } = "Enter the other barrier.";

        protected virtual string OtherBarrierTooLongErrorMessage { get; } = "Barrier must be 300 characters or fewer.";

        protected override async Task<HealthCheck> GetHealthCheckAsync()
        {
            var check = await base.GetHealthCheckAsync();

            await Database.Entry(check).Collection(c => c.ChosenBarriers).LoadAsync();
            await Database.Entry(check).Collection(c => c.CustomBarriers).LoadAsync();

            return check;
        }

        protected virtual async Task LoadBarriers() =>
            Barriers = await Database.Barriers
                .Where(x => x.Category == CurrentRoute)
                .Select(x => new Components.Pages.Barrier()
                {
                    Text = x.Text,
                    Value = x.Id.ToString()
                }).ToListAsync();

        protected virtual async Task LoadPageData()
        {
            await LoadBarriers();

            var check = await GetHealthCheckAsync();

            ShowSkipFollowUps = IsLastChosenPriority(check);

            IsFinalFollowUp = IsEndOfProcess(check);

            AskForEmailAddress = string.IsNullOrEmpty(check.EmailAddress);

            var allFollowUps = healthPriorityRouter.AllFollowupsFor(check).ToList();

            //Add 2 because it's a zero-based index, and we want the *next* health priority's number
            //Note: If not found, IndexOf() returns -1, which will mean we return 1, which is what we want.
            NextHealthPriorityNumber = allFollowUps.IndexOf(CurrentRoute) + 2;

            var followUpName = allFollowUps.ElementAtOrDefault(NextHealthPriorityNumber - 1);

            NextHealthPriorityName = followUpName switch
            {
                "bloodpressure" => "Check Your Blood Pressure",
                "improvebloodpressure" => "Improve Your Blood Pressure",
                "cholesterol" => "Check Your Cholesterol",
                "improvecholesterol" => "Improve Your Cholesterol",
                "smoking" => "Stop Smoking",
                "alcohol" => "Drinking Less",
                "weight" => "Managing Weight",
                "bloodsugar" => "Check Your Blood Sugar",
                "improvebloodsugar" => "Improve Your Blood Sugar",
                "move" => "Moving More",
                "mental" => "Mental Wellbeing",
                _ => null
            };

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
                    OtherBarrier = check?.CustomBarriers?.FirstOrDefault(x => x.Category == CurrentRoute)?.Text
                };
            }
        }

        protected async Task<SanitisedModel> ValidateAndSanitise(UnsanitisedModel model)
        {
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

            if (string.IsNullOrEmpty(model.BeReminded) || (model.BeReminded != "yes" && model.BeReminded != "no"))
            {
                BeRemindedError = BeRemindedErrorMessage;
                AddError(BeRemindedError, "#be-reminded");
                isValid = false;
            }
            else
            {
                sanitisedModel.BeReminded = model.BeReminded == "yes";
            }

            if (sanitisedModel.BeReminded && AskForEmailAddress)
            {
                if (string.IsNullOrEmpty(model.EmailAddress))
                {
                    EmailAddressError = "Please enter your email address";
                    AddError(EmailAddressError, "#contact-by-email");
                    isValid = false;
                }
                else if (!model.EmailAddress.IsValidEmail())
                {
                    EmailAddressError = "Enter an email address in the correct format, like name@example.com";
                    AddError(EmailAddressError, "#contact-by-email");
                    isValid = false;
                }
                else
                {
                    sanitisedModel.EmailAddress = model.EmailAddress;
                }
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

        protected virtual bool RequiresValidation => true;

        public virtual async Task<IActionResult> OnGetAsync()
        {
            if (RequiresValidation && !IsValidated())
            {
                return RedirectToValidation();
            }

            await LoadPageData();

            return Page();
        }

        protected virtual async Task<IActionResult> PostAsync(UnsanitisedModel model)
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
                SetADate = sanitisedModel.SetADate
            },sanitisedModel.EmailAddress, sanitisedModel.SelectedBarriers, sanitisedModel.OtherBarrier);

            return ToNextPage(model.SubmitAction, await GetHealthCheckAsync());
        }

        protected async Task SaveData(FollowUp followUp, string emailAddress, IList<Barrier> chosenBarriers, string otherBarrier)
        {
            var check = await GetHealthCheckAsync();

            var oldFollowUp = await GetFollowUpAsync(check);

            if (oldFollowUp == null)
            {
                SetFollowUp(check, followUp);
            }
            else
            {
                oldFollowUp.BeReminded = followUp.BeReminded;
                oldFollowUp.DoYouHavePlans = followUp.DoYouHavePlans;
                oldFollowUp.SetADate = followUp.SetADate;
                oldFollowUp.ConfidentToChange = followUp.ConfidentToChange;
                oldFollowUp.ImportantToChange = followUp.ImportantToChange;
                oldFollowUp.SelectedOption =  followUp.SelectedOption;
                oldFollowUp.Type = followUp.Type;
            }


            check.EmailAddress ??= emailAddress;

            check.ChosenBarriers = (check.ChosenBarriers ?? new Collection<Barrier>())
                .Except(Database.Barriers.Where(x => x.Category == CurrentRoute))
                .Concat(chosenBarriers)
                .ToList();

            if (otherBarrier != null)
            {
                var barrier = check.CustomBarriers.FirstOrDefault(x => x.Category == CurrentRoute);

                if (barrier is null)
                {
                    barrier = new CustomBarrier
                    {
                        Category = CurrentRoute
                    };

                    check.CustomBarriers.Add(barrier);
                }

                barrier.Text = otherBarrier;
            }
            else
            {
                var barrier = check.CustomBarriers.FirstOrDefault(x => x.Category == CurrentRoute);

                if (barrier is not null)
                {
                    check.CustomBarriers.Remove(barrier);
                }
            }

            await Database.SaveChangesAsync();
        }

        public class UnsanitisedModel
        {
            public IList<string> Barriers { get; set; } = new List<string>();

            public string OtherBarrier { get; set; }

            public string BeReminded { get; set; }

            public string ConfidentToChange { get; set; }

            public string DoYouHavePlans { get; set; }

            public string EmailAddress { get; set; }

            public string ImportantToChange { get; set; }

            public string SetADate { get; set; }

            public string SubmitAction { get; set; }
        }

        public class SanitisedModel
        {
            public string OtherBarrier { get; set; }

            public IList<Barrier> SelectedBarriers { get; set; } = new List<Barrier>();

            public bool BeReminded { get; set; }

            public string ConfidentToChange { get; set; }

            public bool DoYouHavePlans { get; set; }

            public string EmailAddress { get; set; }

            public string ImportantToChange { get; set; }

            public bool SetADate { get; set; }
        }

        private readonly IHealthPriorityRouter healthPriorityRouter;

        public bool AskForEmailAddress { get; set; }

        public string BeRemindedError { get; set; }

        public string ConfidentToChangeError { get; set; }

        public string DoYouHavePlansError { get; set; }

        public string EmailAddressError { get; set; }

        public string ImportantToChangeError { get; set; }

        public bool IsFinalFollowUp { get; set; }

        public string NextHealthPriorityName { get; set; }

        public int NextHealthPriorityNumber { get; set; }

        public string SetADateError { get; set; }

        public bool ShowSkipFollowUps { get; set; }

        protected abstract string BeRemindedErrorMessage { get; }

        protected abstract string ConfidentToChangeErrorMessage { get; }

        protected abstract string CurrentRoute { get; }

        protected abstract string DoYouHavePlansErrorMessage { get; }

        protected abstract string ImportantToChangeErrorMessage { get; }

        protected abstract string SetADateErrorMessage { get; }

        protected FollowUpPageModel(
            Database database,
            ICredentialsDecrypter credentialsDecrypter,
            IHealthPriorityRouter healthPriorityRouter,
            IPageFlow pageFlow)
            : base(database, credentialsDecrypter, pageFlow) =>
            this.healthPriorityRouter = healthPriorityRouter;

        protected bool IsEndOfProcess(HealthCheck check) =>
            healthPriorityRouter.AllFollowupsFor(check).Last() == CurrentRoute;

        protected bool IsLastChosenPriority(HealthCheck check) =>
            // If the current route isn't found, IndexOf will be -1, so we won't be on the LastChosenPriority.
            healthPriorityRouter.AllFollowupsFor(check).ToList().IndexOf(CurrentRoute) >= 1;


        protected virtual IActionResult ToNextPage(string submitAction, HealthCheck check)
            => submitAction switch
            {
                "Actions" => RedirectWithId("./FollowUpActions"),
                _ => RedirectWithId(healthPriorityRouter.NextPage(check, CurrentRoute)),
            };

        
    }
}