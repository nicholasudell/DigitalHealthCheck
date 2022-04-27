using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{


    public class DiabetesModel : HealthCheckPageModel
    {
        public class UnsanitisedModel
        {
            public string FamilyHistory { get; set; }

            public string Steroids { get; set; }
        }

        private class SanitisedModel
        {
            public bool FamilyHistory { get; set; }

            public bool Steroids { get; set; }
        }

        public UnsanitisedModel Model { get; set; }

        public string FamilyHistoryError { get; set; }

        public string SteroidsError { get; set; }

        public DiabetesModel(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) :
            base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();

            var familyHistory = healthCheck?.FamilyHistoryDiabetes;
            var steroids = healthCheck?.Steroids;

            Model = new UnsanitisedModel
            {
                FamilyHistory = familyHistory.HasValue ? (familyHistory.Value ? "yes" : "no") : null,
                Steroids = steroids.HasValue ? (steroids.Value ? "yes" : "no") : null
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
                healthCheck.FamilyHistoryDiabetes = sanitisedModel.FamilyHistory;
                healthCheck.Steroids = sanitisedModel.Steroids;
            }

            await Database.SaveChangesAsync();

            if (healthCheck?.SexAtBirth == Sex.Female && healthCheck?.SexForResults == Sex.Female)
            {
                return RedirectWithId("./PolycysticOvariesAndGestationalDiabetes");
            }
            else
            {
                return RedirectWithId("./BloodSugar");
            }
        }

        SanitisedModel ValidateAndSanitise(UnsanitisedModel model)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel();

            if (string.IsNullOrEmpty(model.FamilyHistory) || (model.FamilyHistory != "yes" && model.FamilyHistory != "no"))
            {
                FamilyHistoryError = $"Select yes if a close family member has diabetes";
                AddError(FamilyHistoryError, "#family-history");
                isValid = false;
            }
            else
            {
                sanitisedModel.FamilyHistory = model.FamilyHistory == "yes";
            }

            if (string.IsNullOrEmpty(model.Steroids) || (model.Steroids != "yes" && model.Steroids != "no"))
            {
                SteroidsError = $"Select yes if you have been precribed and regularly take steroids";
                AddError(SteroidsError, "#steroids");
                isValid = false;
            }
            else
            {
                sanitisedModel.Steroids = model.Steroids == "yes";
            }

            return isValid ? sanitisedModel : null;
        }
    }
}