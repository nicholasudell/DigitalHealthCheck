using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Helpers;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class FollowUpMoveMoreModel : FollowUpPageModel
    {
        public PhysicalActivityStatus PhysicalActivity { get; set; }

        public bool Walking { get; set; }

        protected override string BeRemindedErrorMessage => "Select yes if you would like to be reminded to move more";

        protected override string ConfidentToChangeErrorMessage => "Select how confident you are in making a plan to be more active.";

        protected override string CurrentRoute => "move";

        protected override string DoYouHavePlansErrorMessage => "Select yes if you have plans to move more";

        protected override string ImportantToChangeErrorMessage => "Select how important it is for you to move more.";

        protected override string SetADateErrorMessage => "Select yes if you will set a date to start moving more";

        public FollowUpMoveMoreModel(
            Database database,
            ICredentialsDecrypter credentialsDecrypter,
            IHealthPriorityRouter healthPriorityRouter,
            IPageFlow pageFlow) :
            base(database, credentialsDecrypter, healthPriorityRouter, pageFlow)
        {
        }


        public async Task<IActionResult> OnPostAsync(UnsanitisedModel model) => await base.PostAsync(model);
       

        protected override async Task LoadPageData()
        {
            await base.LoadPageData();

            var check = await GetHealthCheckAsync();

            PhysicalActivity = (PhysicalActivityStatus)check.GPPAQ;

            Walking = check.Walking > GPPAQActivityLevel.LessThanOneHour;
        }

        protected override async Task<FollowUp> GetFollowUpAsync(HealthCheck check)
{
await Database.Entry(check).Reference(c => c.MoveMoreFollowUp).LoadAsync(); 
return check.MoveMoreFollowUp;
}
        protected override void SetFollowUp(HealthCheck check, FollowUp followUp) => check.MoveMoreFollowUp = followUp;
    }
}