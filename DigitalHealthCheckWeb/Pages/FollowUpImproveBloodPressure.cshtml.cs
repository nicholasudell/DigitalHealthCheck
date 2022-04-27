using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class FollowUpImproveBloodPressureModel : FollowUpPageModel
    {
        private readonly IHealthCheckResultFactory healthCheckResultFactory;

        public BloodPressureStatus Status { get; set; }

        protected override string BeRemindedErrorMessage => "Select yes if you would like to be reminded to improve your blood pressure";

        protected override string ConfidentToChangeErrorMessage => "Select how confident you are in drinking less.";

        protected override string CurrentRoute => "improvebloodpressure";

        protected override string DoYouHavePlansErrorMessage => "Select yes if you have plans to improve your blood pressure";

        protected override string ImportantToChangeErrorMessage => "Select how important it is for you to improve your blood pressure.";

        protected override string SetADateErrorMessage => "Select yes if you will set a date to improve your blood pressure";

        public FollowUpImproveBloodPressureModel(
            Database database,
            ICredentialsDecrypter credentialsDecrypter,
            IHealthPriorityRouter healthPriorityRouter,
            IHealthCheckResultFactory healthCheckResultFactory,
            IPageFlow pageFlow)
            : base(database, credentialsDecrypter, healthPriorityRouter, pageFlow) =>
            this.healthCheckResultFactory = healthCheckResultFactory;

        public async Task<IActionResult> OnPostAsync(UnsanitisedModel model) => await base.PostAsync(model);

        protected override async Task LoadPageData()
        {
            await base.LoadPageData();

            var check = await GetHealthCheckAsync();

            var result = healthCheckResultFactory.GetResult(check, false);

            Status = result.BloodPressure.Value;
        }

        protected override async Task<FollowUp> GetFollowUpAsync(HealthCheck check)
        {
            await Database.Entry(check).Reference(c => c.ImproveBloodPressureFollowUp).LoadAsync();
            return check.ImproveBloodPressureFollowUp;
        }
        protected override void SetFollowUp(HealthCheck check, FollowUp followUp) => check.ImproveBloodPressureFollowUp = followUp;
    }
}