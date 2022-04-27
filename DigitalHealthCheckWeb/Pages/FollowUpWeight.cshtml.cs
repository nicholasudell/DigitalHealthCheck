using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class FollowUpWeightModel : FollowUpPageModel
    {
        private readonly IHealthCheckResultFactory healthCheckResultFactory;

        public BodyMassIndexStatus WeightStatus { get; set; }

        protected override string BeRemindedErrorMessage => "Select yes if you would like to be reminded to try to achieve a healthy weight";

        protected override string ConfidentToChangeErrorMessage => "Select how confident you are in trying to achieve a healthy weight.";

        protected override string CurrentRoute => "weight";

        protected override string DoYouHavePlansErrorMessage => "Select yes if you have plans to try to achieve a healthy weight";

        protected override string ImportantToChangeErrorMessage => "Select how important it is for you to try to achieve a healthy weight.";

        protected override string SetADateErrorMessage => "Select yes if you will set a date to try to achieve a healthy weight";

        public FollowUpWeightModel(
            Database database,
            ICredentialsDecrypter credentialsDecrypter,
            IHealthPriorityRouter healthPriorityRouter,
            IHealthCheckResultFactory healthCheckResultFactory,
            IPageFlow pageFlow)
            : base(database, credentialsDecrypter, healthPriorityRouter, pageFlow) => this.healthCheckResultFactory = healthCheckResultFactory;

        protected override async Task LoadPageData()
        {
            await base.LoadPageData();

            var check = await GetHealthCheckAsync();

            var result = healthCheckResultFactory.GetResult(check, false);

            WeightStatus = result.BodyMassIndex;
        }

        public async Task<IActionResult> OnPostAsync(UnsanitisedModel model) => await base.PostAsync(model);
        protected override async Task<FollowUp> GetFollowUpAsync(HealthCheck check)
{
await Database.Entry(check).Reference(c => c.HealthyWeightFollowUp).LoadAsync(); 
return check.HealthyWeightFollowUp;
}
        protected override void SetFollowUp(HealthCheck check, FollowUp followUp) => check.HealthyWeightFollowUp = followUp;
    }
}