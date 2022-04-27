using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Helpers;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class RiskFactors3Model : HealthCheckPageModel
    {
        public class UnsanitisedModel
        {
            public string AtypicalAntipsychoticMedication { get; set; }

            public string SevereMentalIllness { get; set; }

            public string SystemicLupusErythematosus { get; set; }
        }

        private class SanitisedModel
        {
            public YesNoSkip? AtypicalAntipsychoticMedication { get; set; }

            public YesNoSkip? SevereMentalIllness { get; set; }

            public YesNoSkip? SystemicLupusErythematosus { get; set; }
        }

        public string AtypicalAntipsychoticMedicationError { get; set; }

        public UnsanitisedModel Model { get; set; }

        public string SevereMentalIllnessError { get; set; }

        public string SystemicLupusErythematosusError { get; set; }

        public RiskFactors3Model(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) :
            base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();

            Model = new UnsanitisedModel
            {
                SystemicLupusErythematosus = healthCheck?.SystemicLupusErythematosus?.ToString().ToLowerInvariant(),
                SevereMentalIllness = healthCheck?.SevereMentalIllness?.ToString().ToLowerInvariant(),
                AtypicalAntipsychoticMedication = healthCheck?.AtypicalAntipsychoticMedication?.ToString().ToLowerInvariant()
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
                healthCheck.SystemicLupusErythematosus = sanitisedModel.SystemicLupusErythematosus;
                healthCheck.SevereMentalIllness = sanitisedModel.SevereMentalIllness;
                healthCheck.AtypicalAntipsychoticMedication = sanitisedModel.AtypicalAntipsychoticMedication;
            }

            await Database.SaveChangesAsync();

            return RedirectWithId("./BloodPressure");
        }

        SanitisedModel ValidateAndSanitise(UnsanitisedModel model)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel();

            if (model.SystemicLupusErythematosus.SanitiseEnum<YesNoSkip>(out var sanitisedSystemicLupusErythematosus))
            {
                sanitisedModel.SystemicLupusErythematosus = sanitisedSystemicLupusErythematosus;
            }
            else
            {
                SystemicLupusErythematosusError = $"Select yes if you have been diagnosed with systemic lupus erythematosus";
                AddError(SystemicLupusErythematosusError, "#lupus");
                isValid = false;
            }

            if (model.SevereMentalIllness.SanitiseEnum<YesNoSkip>(out var sanitisedSevereMentalIllness))
            {
                sanitisedModel.SevereMentalIllness = sanitisedSevereMentalIllness;
            }
            else
            {
                SevereMentalIllnessError = $"Select yes if you have been diagnosed with a severe mental illness";
                AddError(SevereMentalIllnessError, "#mental-illness");
                isValid = false;
            }

            if (sanitisedModel.SevereMentalIllness.HasValue && sanitisedModel.SevereMentalIllness.Value == YesNoSkip.Yes)
            {
                if (model.AtypicalAntipsychoticMedication.SanitiseEnum<YesNoSkip>(out var sanitisedAtypicalAntipsychoticMedication))
                {
                    sanitisedModel.AtypicalAntipsychoticMedication = sanitisedAtypicalAntipsychoticMedication;
                }
                else
                {
                    AtypicalAntipsychoticMedicationError = $"Select yes if you are on atypical antipsychotic medication";
                    AddError(AtypicalAntipsychoticMedicationError, "#medication");
                    isValid = false;
                }
            }

            return isValid ? sanitisedModel : null;
        }
    }
}