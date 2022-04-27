using System;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{

    public class DrinkingFrequencyModel : AuditHealthCheckPageModel
    {
        public class UnsanitisedModel
        {
            public string Frequency { get; set; }
        }

        private class SanitisedModel
        {
            public AUDITDrinkingFrequency Frequency { get; set; }
        }

        public UnsanitisedModel Model { get; set; }

        public string FrequencyError { get; set; }

        public DrinkingFrequencyModel(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) :
            base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();

            Model = new UnsanitisedModel
            {
                Frequency = healthCheck?.DrinkingFrequency?.ToString()?.ToLowerInvariant()
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(UnsanitisedModel model)
        {
            Model = model;

            var sanitisedModel = ValidateAndSanitise(model);

            if (sanitisedModel is null)
            {
                return await Reload();
            }

            var healthCheck = await GetHealthCheckAsync();

            if (healthCheck is not null)
            {
                healthCheck.DrinkingFrequency = sanitisedModel.Frequency;
            }

            await Database.SaveChangesAsync();

            //If we need to show more alcohol questions, we have to override the next page
            //but preserve the next page value until we don't need to show any more questions
            //that way, it'll then finally follow the redirect.
            if (!AllAuditQuestionsAnswered(healthCheck))
            {
                return RedirectToPage("./AUDIT1", new
                {
                    id = UserId,
                    next = NextPage,
                    then = ThenPage
                });
            }
            else
            {
                return RedirectWithId("./AUDIT1");
            }
        }

        SanitisedModel ValidateAndSanitise(UnsanitisedModel model)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel();

            if (string.IsNullOrEmpty(model.Frequency) || !Enum.TryParse<AUDITDrinkingFrequency>(model.Frequency, true, out var sanitisedFrequency))
            {
                FrequencyError = $"Select how often you have a drink containing alcohol";
                AddError(FrequencyError, "#frequency");
                isValid = false;
            }
            else
            {
                sanitisedModel.Frequency = sanitisedFrequency;
            }

            return isValid ? sanitisedModel : null;
        }
    }
}