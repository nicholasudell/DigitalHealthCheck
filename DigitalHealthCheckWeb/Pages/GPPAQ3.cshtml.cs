using System;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{

    public class GPPAQ3Model : HealthCheckPageModel
    {
        public class UnsanitisedModel
        {
            public string Pace { get; set; }

            public string Walking { get; set; }
        }

        private class SanitisedModel
        {
            public GPPAQWalkingPace Pace { get; set; }

            public GPPAQActivityLevel Walking { get; set; }
        }

        public UnsanitisedModel Model { get; set; }

        public string PaceError { get; set; }

        public string WalkingError { get; set; }

        public GPPAQ3Model(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) :
            base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();

            Model = new UnsanitisedModel
            {
                Walking = healthCheck?.Walking?.ToString()?.ToLowerInvariant(),
                Pace = healthCheck?.WalkingPace?.ToString()?.ToLowerInvariant()
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
                healthCheck.Walking = sanitisedModel.Walking;
                healthCheck.WalkingPace = sanitisedModel.Pace;
            }

            await Database.SaveChangesAsync();

            return RedirectWithId("./Diabetes");
        }

        SanitisedModel ValidateAndSanitise(UnsanitisedModel model)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel();

            if (string.IsNullOrEmpty(model.Walking) ||
                !Enum.TryParse<GPPAQActivityLevel>(model.Walking, true, out var sanitisedWalking))
            {
                WalkingError = $"Select how many hours you spent walking to work, shopping, or for pleasure";
                AddError(WalkingError, "#walking");
                isValid = false;
            }
            else
            {
                sanitisedModel.Walking = sanitisedWalking;
            }

            if (string.IsNullOrEmpty(model.Pace) ||
                !Enum.TryParse<GPPAQWalkingPace>(model.Pace, true, out var sanitisedPace))
            {
                PaceError = $"Select how you would describe your walking pace";
                AddError(PaceError, "#pace");
                isValid = false;
            }
            else
            {
                sanitisedModel.Pace = sanitisedPace;
            }

            return isValid ? sanitisedModel : null;
        }
    }
}