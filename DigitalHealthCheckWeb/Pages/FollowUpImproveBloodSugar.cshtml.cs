using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class FollowUpImproveBloodSugarModel : FollowUpPageModel
    {
        public BloodSugarStatus Status { get; set; }

        protected override string BeRemindedErrorMessage => "Select yes if you would like to be reminded to improve your blood sugar";

        protected override string ConfidentToChangeErrorMessage => "Select how confident you are in improving your blood sugar.";

        protected override string CurrentRoute => "improvebloodsugar";

        protected override string DoYouHavePlansErrorMessage => "Select yes if you have plans to improve your blood sugar";

        protected override string ImportantToChangeErrorMessage => "Select how important it is for you to improve your blood sugar.";

        protected override string SetADateErrorMessage => "Select yes if you will set a date to improve your blood sugar";

        public FollowUpImproveBloodSugarModel(
            Database database,
            ICredentialsDecrypter credentialsDecrypter,
            IHealthPriorityRouter healthPriorityRouter,
            IHealthCheckResultFactory healthCheckResultFactory,
            IPageFlow pageFlow)
            : base(database, credentialsDecrypter, healthPriorityRouter, pageFlow) { }

        public async Task<IActionResult> OnPostAsync(UnsanitisedModel model) => await base.PostAsync(model);

        protected override async Task<FollowUp> GetFollowUpAsync(HealthCheck check)
        {
            await Database.Entry(check).Reference(c => c.ImproveBloodSugarFollowUp).LoadAsync();
            return check.ImproveBloodSugarFollowUp;
        }
        protected override void SetFollowUp(HealthCheck check, FollowUp followUp) => check.ImproveBloodSugarFollowUp = followUp;
    }
}