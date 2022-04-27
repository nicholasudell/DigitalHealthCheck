using System;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class GPPAQ2Model : HealthCheckPageModel
    {
        public class UnsanitisedModel
        {
            public string Gardening { get; set; }

            public string Housework { get; set; }
        }

        private class SanitisedModel
        {
            public GPPAQActivityLevel Gardening { get; set; }

            public GPPAQActivityLevel Housework { get; set; }
        }

        public string GardeningError { get; set; }

        public UnsanitisedModel Model { get; set; }

        public string HouseworkError { get; set; }

        public GPPAQ2Model(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) :
            base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();

            Model = new UnsanitisedModel
            {
                Housework = healthCheck?.Housework?.ToString()?.ToLowerInvariant(),
                Gardening = healthCheck?.Gardening?.ToString()?.ToLowerInvariant()
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
                healthCheck.Housework = sanitisedModel.Housework;
                healthCheck.Gardening = sanitisedModel.Gardening;
            }

            await Database.SaveChangesAsync();

            return RedirectWithId("./GPPAQ3");
        }

        SanitisedModel ValidateAndSanitise(UnsanitisedModel model)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel();

            if (string.IsNullOrEmpty(model.Gardening) ||
                !Enum.TryParse<GPPAQActivityLevel>(model.Gardening, true, out var sanitisedGardening))
            {
                GardeningError = $"Select how many hours you spent doing gardening or DIY during the last week";
                AddError(GardeningError, "#gardening");
                isValid = false;
            }
            else
            {
                sanitisedModel.Gardening = sanitisedGardening;
            }

            if (string.IsNullOrEmpty(model.Housework) ||
                !Enum.TryParse<GPPAQActivityLevel>(model.Housework, true, out var sanitisedHousework))
            {
                HouseworkError = $"Select how many hours you spent doing housework or childcare during the last week";
                AddError(HouseworkError, "#housework");
                isValid = false;
            }
            else
            {
                sanitisedModel.Housework = sanitisedHousework;
            }

            return isValid ? sanitisedModel : null;
        }
    }
}