using System;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class PhysicalActivityModel : HealthCheckPageModel
    {
        public string Error { get; set; }

        public string WorkActivity { get; set; }

        public PhysicalActivityModel(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) :
            base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();
            WorkActivity = healthCheck?.WorkActivity?.ToString()?.ToLowerInvariant();
        }

        public async Task<IActionResult> OnPostAsync(string workActivity)
        {
            WorkActivity = workActivity;

            var sanitisedWorkActivity = ValidateAndSanitise(workActivity);

            if (sanitisedWorkActivity is null)
            {
                return await Reload();
            }

            var healthCheck = await GetHealthCheckAsync();

            if (healthCheck is not null)
            {
                healthCheck.WorkActivity = sanitisedWorkActivity;
            }

            await Database.SaveChangesAsync();

            return RedirectWithId("./GPPAQ1");
        }

        GPPAQEmploymentActivity? ValidateAndSanitise(string value)
        {
            if (string.IsNullOrEmpty(value) || !Enum.TryParse<GPPAQEmploymentActivity>(value, true, out var sanitisedEmploymentActivity))
            {
                Error = "Select what type and amount of physical activity is involved in your work";
                AddError(Error, "#work-activity");
                return null;
            }
            else
            {
                return sanitisedEmploymentActivity;
            }
        }
    }
}