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
    public class FollowUpMentalWellbeingModel : FollowUpPageModel
    {
        private readonly IHealthCheckResultFactory healthCheckResultFactory;

        public HealthCheck Check { get; set; }

        public Result Result { get; set; }

        protected override string BeRemindedErrorMessage => "Select yes if you would like to be reminded to improve your mental wellbeing";

        protected override string ConfidentToChangeErrorMessage => "Select how confident you are in improving your mental wellbeing.";

        protected override string CurrentRoute => "mental";

        protected override string DoYouHavePlansErrorMessage => "Select yes if you have plans to improve your mental wellbeing";

        protected override string ImportantToChangeErrorMessage => "Select how important it is for you to improve your mental wellbeing.";

        protected override string SetADateErrorMessage => "Select yes if you will set a date to improve your mental wellbeing";

        public FollowUpMentalWellbeingModel(
            Database database,
            ICredentialsDecrypter credentialsDecrypter,
            IHealthPriorityRouter healthPriorityRouter,
            IPageFlow pageFlow,
            IHealthCheckResultFactory healthCheckResultFactory)
            : base(database, credentialsDecrypter, healthPriorityRouter, pageFlow) => this.healthCheckResultFactory = healthCheckResultFactory;


        public async Task<IActionResult> OnPostAsync(UnsanitisedModel model) => await base.PostAsync(model);
        

        protected override async Task LoadBarriers()
        {
            var barriers = Database.Barriers
                .Where(x => x.Category == CurrentRoute);

            var check = await GetHealthCheckAsync();

            var result = healthCheckResultFactory.GetResult(check, false);

            // There's one barrier that should only be shown for amber or red mental wellbeing score.

            if (result.MentalWellbeing == DefaultStatus.Healthy)
            {
                barriers = barriers.Where(x => x.Id != Database.NeedMentalHealthReferralBarrierId);
            }

            Barriers = await barriers
                .Select(x => new Components.Pages.Barrier()
                {
                    Text = x.Text,
                    Value = x.Id.ToString()
                }).ToListAsync();
        }

        protected override async Task LoadPageData()
        {
            await base.LoadPageData();

            Check = await GetHealthCheckAsync();

            Result = healthCheckResultFactory.GetResult(Check, false);
        }

        protected override async Task<FollowUp> GetFollowUpAsync(HealthCheck check)
        {
            await Database.Entry(check).Reference(c => c.MentalWellbeingFollowUp).LoadAsync(); 
            return check.MentalWellbeingFollowUp;
        }
        protected override void SetFollowUp(HealthCheck check, FollowUp followUp) => check.MentalWellbeingFollowUp = followUp;
    }
}