using System;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using DigitalHealthCheckWeb.Model.Risks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class CheckYourAnswersModel : HealthCheckPageModel
    {
        public HealthCheck Check { get; set; }

        public CheckYourAnswersModel(
                    Database database,
            ICredentialsDecrypter credentialsDecrypter,
            IPageFlow pageFlow)
            : base(database, credentialsDecrypter, pageFlow)
        { }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!IsValidated())
            {
                return RedirectToValidation();
            }

            Check = await base.GetHealthCheckAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Reset the start time so that we definitely recalculate when going to the next page.
            HttpContext.Session.SetString("CalculatingStartTime", string.Empty); 

            var healthCheck = await GetHealthCheckAsync();

            healthCheck.SubmittedCheckYourAnswers = true;

            await Database.SaveChangesAsync();

            if (healthCheck.Height is null || healthCheck.Weight is null)
            {
                return RedirectWithId("./FollowUpHeightAndWeight");
            }

            return RedirectWithId("./Calculating");
        }
    }
}