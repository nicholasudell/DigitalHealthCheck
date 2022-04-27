using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Helpers;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class RiskFactors1Model : HealthCheckPageModel
    {
        public class UnsanitisedModel
        {
            public string AtrialFibrillation { get; set; }

            public string ChronicKidneyDisease { get; set; }

            public string FamilyHistoryCVD { get; set; }
        }

        private class SanitisedModel
        {
            public bool AtrialFibrillation { get; set; }

            public bool ChronicKidneyDisease { get; set; }

            public bool FamilyHistoryCVD { get; set; }
        }

        public string AtrialFibrillationError { get; set; }

        public string ChronicKidneyDiseaseError { get; set; }

        public string FamilyHistoryCVDError { get; set; }

        public UnsanitisedModel Model { get; set; }

        public RiskFactors1Model(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) :
            base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();

            Model = new UnsanitisedModel
            {
                FamilyHistoryCVD = healthCheck?.FamilyHistoryCVD.AsYesNoOrNull(),
                ChronicKidneyDisease = healthCheck?.ChronicKidneyDisease.AsYesNoOrNull(),
                AtrialFibrillation = healthCheck?.AtrialFibrillation.AsYesNoOrNull()
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
                healthCheck.FamilyHistoryCVD = sanitisedModel.FamilyHistoryCVD;
                healthCheck.ChronicKidneyDisease = sanitisedModel.ChronicKidneyDisease;
                healthCheck.AtrialFibrillation = sanitisedModel.AtrialFibrillation;
            }

            await Database.SaveChangesAsync();

            return RedirectWithId("./RiskFactors2");
        }

        SanitisedModel ValidateAndSanitise(UnsanitisedModel model)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel();

            if (model.FamilyHistoryCVD.SanitiseYesNo(out var sanitisedFamilyHistoryCVD))
            {
                sanitisedModel.FamilyHistoryCVD = sanitisedFamilyHistoryCVD;
            }
            else
            {
                FamilyHistoryCVDError = $"Select yes if an immediate family member has been diagnosed with angina or had a heart attack under the age of 60";
                AddError(FamilyHistoryCVDError, "#family-history");
                isValid = false;
            }

            if (model.ChronicKidneyDisease.SanitiseYesNo(out var sanitisedChronicKidneyDisease))
            {
                sanitisedModel.ChronicKidneyDisease = sanitisedChronicKidneyDisease;
            }
            else
            {
                ChronicKidneyDiseaseError = $"Select yes if you have been diagnosed with chronic kidney disease";
                AddError(ChronicKidneyDiseaseError, "#chronic-kidney-disease");
                isValid = false;
            }

            if (model.AtrialFibrillation.SanitiseYesNo(out var sanitisedAtrialFibrillation))
            {
                sanitisedModel.AtrialFibrillation = sanitisedAtrialFibrillation;
            }
            else
            {
                AtrialFibrillationError = $"Select yes if you have been diagnosed with atrial fibrillation";
                AddError(AtrialFibrillationError, "#atrial-fibrillation");
                isValid = false;
            }

            return isValid ? sanitisedModel : null;
        }
    }
}