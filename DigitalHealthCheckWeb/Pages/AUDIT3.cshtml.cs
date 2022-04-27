using System;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{


    public class AUDIT3Model : AuditHealthCheckPageModel
    {
        public class UnsanitisedModel
        {
            public string Guilt { get; set; }

            public string MorningAfter { get; set; }
        }

        private class SanitisedModel
        {
            public AUDITFrequency Guilt { get; set; }

            public AUDITFrequency MorningAfter { get; set; }
        }

        public UnsanitisedModel Model { get; set; }

        public string FeltGuiltError { get; set; }

        public string MorningAfterError { get; set; }

        public AUDIT3Model(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow)
            : base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();

            Model = new UnsanitisedModel
            {
                MorningAfter = healthCheck?.NeededToDrinkAlcoholMorningAfter?.ToString()?.ToLowerInvariant(),
                Guilt = healthCheck?.GuiltAfterDrinking?.ToString()?.ToLowerInvariant(),
            };
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
                healthCheck.NeededToDrinkAlcoholMorningAfter = sanitisedModel.MorningAfter;
                healthCheck.GuiltAfterDrinking = sanitisedModel.Guilt;
            }

            await Database.SaveChangesAsync();

            //If we need to show more alcohol questions, we have to override the next page
            //but preserve the next page value until we don't need to show any more questions
            //that way, it'll then finally follow the redirect.

            if (!AllAuditQuestionsAnswered(healthCheck))
            {
                return RedirectToPage("./AUDIT4", new
                {
                    id = UserId,
                    next = NextPage,
                    then = ThenPage
                });
            }
            else
            {
                return RedirectWithId("./AUDIT4");
            }
        }

        SanitisedModel ValidateAndSanitise(UnsanitisedModel model)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel();

            if (string.IsNullOrEmpty(model.MorningAfter) || !Enum.TryParse<AUDITFrequency>(model.MorningAfter, true, out var sanitisedMorningAfter))
            {
                MorningAfterError = $"Select how often you have needed an alcoholic drink in the morning to get yourself going after a heavy drinking session in the last year";
                AddError(MorningAfterError, "#morning-after");
                isValid = false;
            }
            else
            {
                sanitisedModel.MorningAfter = sanitisedMorningAfter;
            }

            if (string.IsNullOrEmpty(model.Guilt) || !Enum.TryParse<AUDITFrequency>(model.Guilt, true, out var sanitisedFeltGuilt))
            {
                FeltGuiltError = $"Select how often you have had a feeling of guilt or remorse after drinking";
                AddError(FeltGuiltError, "#guilt");
                isValid = false;
            }
            else
            {
                sanitisedModel.Guilt = sanitisedFeltGuilt;
            }

            return isValid ? sanitisedModel : null;
        }
    }
}