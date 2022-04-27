using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class FollowUpBloodPressureModel : FollowUpNumbersPageModel
    {
        private readonly IHealthCheckResultFactory healthCheckResultFactory;

        public BloodPressureStatus? Status { get; set; }

        protected override string BeRemindedErrorMessage => "Select yes if you would like to be reminded to get your blood pressure checked";

        protected override string ConfidentToChangeErrorMessage => "Select how confident you are in making a plan to get your blood pressure checked.";

        protected override string CurrentRoute => "bloodpressure";

        protected override string DoYouHavePlansErrorMessage => "Select yes if you have plans to get your blood pressure checked";

        protected override string ImportantToChangeErrorMessage => "Select how important it is for you to get your blood pressure checked.";

        protected override string SelectedOptionErrorMessage => "Select your preferred option to get your blood pressure checked.";

        protected override string SetADateErrorMessage => "Select yes if you will set a date to get your blood pressure checked";

        public FollowUpBloodPressureModel(
            Database database,
            ICredentialsDecrypter credentialsDecrypter,
            IHealthPriorityRouter healthPriorityRouter,
            IHealthCheckResultFactory healthCheckResultFactory,
            IPageFlow pageFlow)
            : base(database, credentialsDecrypter, healthPriorityRouter, pageFlow) => this.healthCheckResultFactory = healthCheckResultFactory;

        public async Task<IActionResult> OnPostAsync(UnsanitisedModel model) => await base.PostAsync(model);

        protected override async Task LoadPageData()
        {
            await base.LoadPageData();

            var check = await GetHealthCheckAsync();

            Status = healthCheckResultFactory.GetResult(check, false).BloodPressure;
        }

        protected override async Task<FollowUp> GetFollowUpAsync(HealthCheck check)
        {
            await Database.Entry(check).Reference(c => c.BloodPressureFollowUp).LoadAsync();
            return check.BloodPressureFollowUp;
        }
        protected override void SetFollowUp(HealthCheck check, FollowUp followUp) => check.BloodPressureFollowUp = followUp;
    }
}