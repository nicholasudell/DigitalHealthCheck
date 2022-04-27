using System.Linq;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalHealthCheckWeb.Pages
{
    public class FollowUpAlcoholModel : FollowUpPageModel
    {
        private readonly IHealthCheckResultFactory healthCheckResultFactory;

        public DefaultStatus DrinkingStatus { get; set; }

        protected override string BeRemindedErrorMessage => "Select yes if you would like to be reminded to drink less";

        protected override string ConfidentToChangeErrorMessage => "Select how confident you are in drinking less.";

        protected override string CurrentRoute => "alcohol";

        protected override string DoYouHavePlansErrorMessage => "Select yes if you have plans to drink less";

        protected override string ImportantToChangeErrorMessage => "Select how important it is for you to drink less.";

        protected override string SetADateErrorMessage => "Select yes if you will set a date to drink less";

        public FollowUpAlcoholModel(
            Database database,
            ICredentialsDecrypter credentialsDecrypter,
            IHealthPriorityRouter healthPriorityRouter,
            IHealthCheckResultFactory healthCheckResultFactory,
            IPageFlow pageFlow)
            : base(database, credentialsDecrypter, healthPriorityRouter, pageFlow) => this.healthCheckResultFactory = healthCheckResultFactory;

        public async Task<IActionResult> OnPostAsync(UnsanitisedModel model) => await base.PostAsync(model);

        protected override async Task LoadBarriers()
        {
            var barriers = Database.Barriers
                .Where(x => x.Category == CurrentRoute);

            var check = await GetHealthCheckAsync();

            // There's one barrier that should only be shown for AUDIT score 8+,
            // so we filter that out here for AUDIT <8

            if (check.AUDIT < 8)
            {
                barriers = barriers.Where(x => x.Id != Database.AuditOver8BarrierId);
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

            var check = await GetHealthCheckAsync();

            var result = healthCheckResultFactory.GetResult(check, false);

            DrinkingStatus = result.Alcohol;
        }

        protected override async Task<FollowUp> GetFollowUpAsync(HealthCheck check)
{
await Database.Entry(check).Reference(c => c.DrinkLessFollowUp).LoadAsync(); 
return check.DrinkLessFollowUp;
}
        protected override void SetFollowUp(HealthCheck check, FollowUp followUp) => check.DrinkLessFollowUp = followUp;
    }
}