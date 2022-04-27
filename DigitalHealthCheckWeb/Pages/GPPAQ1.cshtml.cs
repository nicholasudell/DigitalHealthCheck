using System;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class GPPAQ1Model : HealthCheckPageModel
    {
        public class UnsanitisedModel
        {
            public string Cycling { get; set; }

            public string PhysicalActivity { get; set; }
        }

        private class SanitisedModel
        {
            public GPPAQActivityLevel Cycling { get; set; }

            public GPPAQActivityLevel PhysicalActivity { get; set; }
        }

        public string CyclingError { get; set; }

        public UnsanitisedModel Model { get; set; }

        public string PhysicalActivityError { get; set; }

        public GPPAQ1Model(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) :
            base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();

            Model = new UnsanitisedModel
            {
                Cycling = healthCheck?.Cycling?.ToString()?.ToLowerInvariant(),
                PhysicalActivity = healthCheck?.PhysicalActivity?.ToString()?.ToLowerInvariant()
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
                healthCheck.Cycling = sanitisedModel.Cycling;
                healthCheck.PhysicalActivity = sanitisedModel.PhysicalActivity;
            }

            await Database.SaveChangesAsync();

            return RedirectWithId("./GPPAQ2");
        }

        SanitisedModel ValidateAndSanitise(UnsanitisedModel model)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel();

            if (string.IsNullOrEmpty(model.PhysicalActivity) || !Enum.TryParse<GPPAQActivityLevel>(model.PhysicalActivity, true, out var sanitisedPhysicalActivity))
            {
                PhysicalActivityError = $"Select how many hours you spent doing physical activity during the last week.";
                AddError(PhysicalActivityError, "#physical-activity");
                isValid = false;
            }
            else
            {
                sanitisedModel.PhysicalActivity = sanitisedPhysicalActivity;
            }

            if (string.IsNullOrEmpty(model.Cycling) || !Enum.TryParse<GPPAQActivityLevel>(model.Cycling, true, out var sanitisedCycling))
            {
                CyclingError = $"Select how many hours you spent cycling during the last week.";
                AddError(CyclingError, "#cycling");
                isValid = false;
            }
            else
            {
                sanitisedModel.Cycling = sanitisedCycling;
            }

            return isValid ? sanitisedModel : null;
        }
    }
}