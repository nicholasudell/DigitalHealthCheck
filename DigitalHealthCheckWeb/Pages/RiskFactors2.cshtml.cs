using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Helpers;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class RiskFactors2Model : HealthCheckPageModel
    {
        public class UnsanitisedModel
        {
            public string BloodPressureTreatment { get; set; }

            public string Migraines { get; set; }

            public string RheumatoidArthritis { get; set; }
        }

        private class SanitisedModel
        {
            public bool BloodPressureTreatment { get; set; }

            public bool Migraines { get; set; }

            public bool RheumatoidArthritis { get; set; }
        }

        public string BloodPressureTreatmentError { get; set; }

        public string MigrainesError { get; set; }

        public string RheumatoidArthritisError { get; set; }

        public UnsanitisedModel Model { get; set; }

        public RiskFactors2Model(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) :
            base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();

            Model = new UnsanitisedModel
            {
                BloodPressureTreatment = healthCheck?.BloodPressureTreatment.AsYesNoOrNull(),
                Migraines = healthCheck?.Migraines.AsYesNoOrNull(),
                RheumatoidArthritis = healthCheck?.RheumatoidArthritis.AsYesNoOrNull()
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
                healthCheck.BloodPressureTreatment = sanitisedModel.BloodPressureTreatment;
                healthCheck.Migraines = sanitisedModel.Migraines;
                healthCheck.RheumatoidArthritis = sanitisedModel.RheumatoidArthritis;
            }

            await Database.SaveChangesAsync();

            return RedirectWithId("./RiskFactors3");
        }

        SanitisedModel ValidateAndSanitise(UnsanitisedModel model)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel();

            if (model.BloodPressureTreatment.SanitiseYesNo(out var sanitisedBloodPressureTreatment))
            {
                sanitisedModel.BloodPressureTreatment = sanitisedBloodPressureTreatment;
            }
            else
            {
                BloodPressureTreatmentError = $"Select yes if you are on blood pressure treatment";
                AddError(BloodPressureTreatmentError, "#blood-pressure-treatment");
                isValid = false;
            }

            if (model.Migraines.SanitiseYesNo(out var sanitisedMigraines))
            {
                sanitisedModel.Migraines = sanitisedMigraines;
            }
            else
            {
                MigrainesError = $"Select yes if you have migraines";
                AddError(MigrainesError, "#migraines");
                isValid = false;
            }

            if (model.RheumatoidArthritis.SanitiseYesNo(out var sanitisedRheumatoidArthritis))
            {
                sanitisedModel.RheumatoidArthritis = sanitisedRheumatoidArthritis;
            }
            else
            {
                RheumatoidArthritisError = $"Select yes if you have been diagnosed with rheumatoid arthritis";
                AddError(RheumatoidArthritisError, "#rheumatoid-arthritis");
                isValid = false;
            }

            return isValid ? sanitisedModel : null;
        }
    }
}