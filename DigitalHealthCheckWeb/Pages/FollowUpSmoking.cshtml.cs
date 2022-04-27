using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class FollowUpSmokingModel : FollowUpPageModel
    {
        public DefaultStatus SmokingStatus { get; set; }

        protected override string BeRemindedErrorMessage => "Select yes if you would like to be reminded to quit smoking";

        protected override string ConfidentToChangeErrorMessage => "Select how confident you are in quitting smoking.";

        protected override string CurrentRoute => "smoking";

        protected override string DoYouHavePlansErrorMessage => "Select yes if you have plans to quit smoking";

        protected override string ImportantToChangeErrorMessage => "Select how important it is for you to quit smoking.";

        protected override string SetADateErrorMessage => "Select yes if you will set a date to quit smoking";

        public FollowUpSmokingModel(
            Database database,
            ICredentialsDecrypter credentialsDecrypter,
            IHealthPriorityRouter healthPriorityRouter,
            IPageFlow pageFlow)
            : base(database, credentialsDecrypter, healthPriorityRouter, pageFlow)
        {
        }

        public async Task<IActionResult> OnPostAsync(UnsanitisedModel model) => await base.PostAsync(model);
        

        protected override async Task LoadPageData()
        {
            await base.LoadPageData();

            var check = await GetHealthCheckAsync();

            SmokingStatus = (check.SmokingStatus is DigitalHealthCheckEF.SmokingStatus.ExSmoker or DigitalHealthCheckEF.SmokingStatus.NonSmoker) ?
                DefaultStatus.Healthy :
                DefaultStatus.Danger;
        }

        protected override async Task<FollowUp> GetFollowUpAsync(HealthCheck check)
{
await Database.Entry(check).Reference(c => c.QuitSmokingFollowUp).LoadAsync(); 
return check.QuitSmokingFollowUp;
}
        protected override void SetFollowUp(HealthCheck check, FollowUp followUp) => check.QuitSmokingFollowUp = followUp;
    }
}