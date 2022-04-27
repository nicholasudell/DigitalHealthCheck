using System;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class SmokerModel : HealthCheckPageModel
    {
        public string Error { get; set; }

        public string HowMuch { get; set; }

        public SmokerModel(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) :
            base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();
            HowMuch = healthCheck?.SmokingStatus?.ToString().ToLowerInvariant();
        }

        public async Task<IActionResult> OnPostAsync(string howMuch)
        {
            HowMuch = howMuch;

            var sanitisedHowMuch = ValidateAndSanitise(howMuch);

            if (sanitisedHowMuch is null)
            {
                return await Reload();
            }

            var healthCheck = await GetHealthCheckAsync();

            if (healthCheck is not null)
            {
                healthCheck.SmokingStatus = sanitisedHowMuch;
            }

            await Database.SaveChangesAsync();

            return RedirectWithId("./DoYouDrinkAlcohol");
        }

        SmokingStatus? ValidateAndSanitise(string value)
        {
            if (string.IsNullOrEmpty(value) || !Enum.TryParse<SmokingStatus>(value, true, out var sanitised))
            {
                Error = "Select how much you smoke";
                AddError(Error, "#how-much");
                return null;
            }

            return sanitised;
        }
    }
}