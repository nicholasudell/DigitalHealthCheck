using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class FollowUpGPModel : FollowUpPageModel
    {
        private readonly IHealthCheckResultFactory healthCheckResultFactory;

        public DefaultStatus DrinkingStatus { get; set; }

        public Result Result { get; set; }

        protected override string BeRemindedErrorMessage => "Select yes if you would like to be reminded to make an appointment at your GP clinic";

        protected override string ConfidentToChangeErrorMessage => "Select how confident you are in making an appointment at your GP clinic.";

        protected override string CurrentRoute => "gp";

        protected override string DoYouHavePlansErrorMessage => "Select yes if you have plans to make an appointment at your GP clinic";

        protected override string ImportantToChangeErrorMessage => "Select how important it is for you to make an appointment at your GP clinic.";

        protected override string SetADateErrorMessage => "Select yes if you will set a date to make an appointment at your GP clinic";

        public FollowUpGPModel(
            Database database,
            ICredentialsDecrypter credentialsDecrypter,
            IHealthPriorityRouter healthPriorityRouter,
            IPageFlow pageFlow,
            IHealthCheckResultFactory healthCheckResultFactory)
            : base(database, credentialsDecrypter, healthPriorityRouter, pageFlow)
        {
            this.healthCheckResultFactory = healthCheckResultFactory;
        }

        public async Task<IActionResult> OnPostAsync(UnsanitisedModel model) => await base.PostAsync(model);

        protected override async Task<FollowUp> GetFollowUpAsync(HealthCheck check)
        {
            await Database.Entry(check).Reference(c => c.BookGPAppointmentFollowUp).LoadAsync();
            return check.BookGPAppointmentFollowUp;
        }

        protected override async Task LoadPageData()
        {
            await base.LoadPageData();

            Result = healthCheckResultFactory.GetResult(await GetHealthCheckAsync(), false);
        }

        protected override void SetFollowUp(HealthCheck check, FollowUp followUp) => check.BookGPAppointmentFollowUp = followUp;
    }
}