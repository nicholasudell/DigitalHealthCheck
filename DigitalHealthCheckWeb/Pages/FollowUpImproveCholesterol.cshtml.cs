using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class FollowUpImproveCholesterolModel : FollowUpPageModel
    {
        private readonly IHealthCheckResultFactory healthCheckResultFactory;

        public DefaultStatus HeartDiseaseStatus { get; set; }

        public DefaultStatus Status { get; set; }

        protected override string BeRemindedErrorMessage => "Select yes if you would like to be reminded to improve your cholesterol";

        protected override string ConfidentToChangeErrorMessage => "Select how confident you are in improving your cholesterol.";

        protected override string CurrentRoute => "improvecholesterol";

        protected override string DoYouHavePlansErrorMessage => "Select yes if you have plans to improve your cholesterol";

        protected override string ImportantToChangeErrorMessage => "Select how important it is for you to improve your cholesterol.";

        protected override string SetADateErrorMessage => "Select yes if you will set a date to improve your cholesterol";

        public FollowUpImproveCholesterolModel(
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

            var result = healthCheckResultFactory.GetResult(check, false);

            Status = result.Cholesterol.Value;
            HeartDiseaseStatus = result.HeartDisease;
        }

        protected override async Task<FollowUp> GetFollowUpAsync(HealthCheck check)
{
await Database.Entry(check).Reference(c => c.ImproveCholesterolFollowUp).LoadAsync(); 
return check.ImproveCholesterolFollowUp;
}
        protected override void SetFollowUp(HealthCheck check, FollowUp followUp) => check.ImproveCholesterolFollowUp = followUp;
    }
}