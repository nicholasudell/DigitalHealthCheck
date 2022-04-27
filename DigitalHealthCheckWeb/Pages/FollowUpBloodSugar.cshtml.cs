using System;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Components.GDS;
using DigitalHealthCheckWeb.Helpers;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalHealthCheckWeb.Pages
{
    public class FollowUpBloodSugarModel : FollowUpNumbersPageModel
    {
        public new class UnsanitisedModel : FollowUpNumbersPageModel.UnsanitisedModel
        {
            public string FingerPrickEmailAddress { get; set; }
        }

        public new class SanitisedModel : FollowUpNumbersPageModel.SanitisedModel
        {
            public string FingerPrickEmailAddress { get; set; }
        }

        private UnsanitisedModel model;

        public new UnsanitisedModel Model
        {
            get => model;
            set
            {
                model = value;
                base.Model = value;
            }
        }

        public string FingerPrickEmailAddressError { get; set; }

        private readonly IHealthCheckResultFactory healthCheckResultFactory;

        public bool OtherHighRiskFactors { get; set; }

        public BloodSugarStatus? Status { get; set; }

        protected override string BeRemindedErrorMessage => "Select yes if you would like to be reminded to get my blood sugar checked";

        protected override string ConfidentToChangeErrorMessage => "Select how confident you are in making a plan to get your blood sugar checked.";

        protected override string CurrentRoute => "bloodsugar";

        protected override string DoYouHavePlansErrorMessage => "Select yes if you have plans to get my blood sugar checked";

        protected override string ImportantToChangeErrorMessage => "Select how important it is for you to get my blood sugar checked.";

        protected override string SelectedOptionErrorMessage => "Select your preferred option to get your blood sugar checked.";

        protected override string SetADateErrorMessage => "Select yes if you will set a date to get my blood sugar checked";

        public FollowUpBloodSugarModel(
            Database database,
            ICredentialsDecrypter credentialsDecrypter,
            IHealthPriorityRouter healthPriorityRouter,
            IHealthCheckResultFactory healthCheckResultFactory,
            IPageFlow pageFlow)
            : base(database, credentialsDecrypter, healthPriorityRouter, pageFlow) => this.healthCheckResultFactory = healthCheckResultFactory;

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

            var check = await GetHealthCheckAsync();

            if (sanitisedModel.SelectedOption == "home")
            {

                var bloodKitRequest = await Database.BloodKitRequests.FirstOrDefaultAsync(x => x.Check == check);

                if (bloodKitRequest is null)
                {
                    bloodKitRequest = new BloodKitRequest()
                    {
                        Check = check,
                        DateRequested = DateTime.Today,
                        Email = sanitisedModel.FingerPrickEmailAddress
                    };

                    await Database.BloodKitRequests.AddAsync(bloodKitRequest);
                }
                else
                {
                    // Only need to do this if we're updating email

                    if (bloodKitRequest.Email != sanitisedModel.FingerPrickEmailAddress)
                    {
                        bloodKitRequest.DateRequested = DateTime.Today;
                        bloodKitRequest.Email = sanitisedModel.FingerPrickEmailAddress;

                        await Database.SaveChangesAsync();
                    }
                }
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

            return ToNextPage(model.SubmitAction, await GetHealthCheckAsync());
        }

        protected override async Task LoadPageData()
        {
            await base.LoadPageData();

            var check = await GetHealthCheckAsync();

            if (Model is null) //FollowUp might already have been set, e.g. onPost
            {
                var bloodKitRequest = await Database.BloodKitRequests.FirstOrDefaultAsync(x => x.Check == check);

                Model = new UnsanitisedModel
                {
                    BeReminded = base.Model.BeReminded,
                    SetADate = base.Model.SetADate,
                    DoYouHavePlans = base.Model.DoYouHavePlans,
                    ImportantToChange = base.Model.ImportantToChange,
                    ConfidentToChange = base.Model.ConfidentToChange,
                    SelectedOption = base.Model.SelectedOption,
                    FingerPrickEmailAddress = bloodKitRequest?.Email,
                    Barriers = base.Model.Barriers,
                    OtherBarrier = base.Model.OtherBarrier,
                    EmailAddress = base.Model.EmailAddress
                };
            }

            var result = healthCheckResultFactory.GetResult(check, false);

            Status = result.BloodSugar;

            OtherHighRiskFactors = result.BodyMassIndex == BodyMassIndexStatus.Obese ||
                result.BloodPressure == BloodPressureStatus.High ||
                result.BloodPressure == BloodPressureStatus.Severe ||
                result.Diabetes == DefaultStatus.Danger;
        }

        protected async Task<SanitisedModel> ValidateAndSanitise(UnsanitisedModel model)
        {
            var sanitisedBase = await base.ValidateAndSanitise(model);

            if (sanitisedBase is null)
            {
                return null;
            }

            var isValid = true;

            var sanitisedModel = new SanitisedModel
            {
                SetADate = sanitisedBase.SetADate,
                BeReminded = sanitisedBase.BeReminded,
                ConfidentToChange = sanitisedBase.ConfidentToChange,
                DoYouHavePlans = sanitisedBase.DoYouHavePlans,
                ImportantToChange = sanitisedBase.ImportantToChange,
                SelectedOption = sanitisedBase.SelectedOption,
                OtherBarrier = sanitisedBase.OtherBarrier,
                SelectedBarriers = sanitisedBase.SelectedBarriers,
                EmailAddress = sanitisedBase.EmailAddress
            };

            if (sanitisedModel.SelectedOption == "home")
            {
                if (string.IsNullOrEmpty(model.FingerPrickEmailAddress))
                {
                    FingerPrickEmailAddressError = "Enter your email address";
                    isValid = false;
                }
                else if (!model.FingerPrickEmailAddress.IsValidEmail())
                {
                    FingerPrickEmailAddressError = "Enter an email address in the correct format, like name@example.com";
                    isValid = false;
                }
                else
                {
                    sanitisedModel.FingerPrickEmailAddress = model.FingerPrickEmailAddress;
                }

                if (!string.IsNullOrEmpty(FingerPrickEmailAddressError))
                {
                    var index = -1;

                    // Count backwards, because there are fewer options to check for that way
                    for (var i = ErrorList.Count - 1; i >= 0; i--)
                    {
                        var error = ErrorList[i];

                        if (error.Href == "#important-to-change" || error.Href == "#confident-to-change" || error.Href == "#options")
                        {
                            index = i + 1;
                            break;
                        }
                    }

                    ErrorList.Insert(index == -1 ? 0 : index, new GDSErrorSummary.Item
                    {
                        Content = FingerPrickEmailAddressError,
                        Href = "#finger-prick-email-address"
                    });
                }
            }

            return isValid ? sanitisedModel : null;
        }

        protected override async Task<FollowUp> GetFollowUpAsync(HealthCheck check)
        {
            await Database.Entry(check).Reference(c => c.BloodSugarFollowUp).LoadAsync();
            return check.BloodSugarFollowUp;
        }
        protected override void SetFollowUp(HealthCheck check, FollowUp followUp) => check.BloodSugarFollowUp = followUp;
    }
}