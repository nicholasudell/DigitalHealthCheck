using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class PolycysticOvariesAndGestationalDiabetesModel : HealthCheckPageModel
    {
        public class UnsanitisedModel
        {
            public string GestationalDiabetes { get; set; }

            public string PolycysticOvaries { get; set; }
        }

        private class SanitisedModel
        {
            public bool GestationalDiabetes { get; set; }

            public bool PolycysticOvaries { get; set; }
        }

        public string GestationalDiabetesError { get; set; }

        public UnsanitisedModel Model { get; set; }

        public string PolycysticOvariesError { get; set; }

        public PolycysticOvariesAndGestationalDiabetesModel(
            Database database,
            ICredentialsDecrypter credentialsDecrypter,
            IPageFlow pageFlow) :
            base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();

            var polycysticOvaries = Variant ? healthCheck?.Variant?.PolycysticOvaries : healthCheck?.PolycysticOvaries;
            var gestationalDiabetes = Variant? healthCheck?.Variant?.GestationalDiabetes : healthCheck?.GestationalDiabetes;

            Model = new UnsanitisedModel
            {
                PolycysticOvaries = polycysticOvaries.HasValue ? (polycysticOvaries.Value ? "yes" : "no") : null,
                GestationalDiabetes = gestationalDiabetes.HasValue ? (gestationalDiabetes.Value ? "yes" : "no") : null
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
                if(Variant)
                {
                    healthCheck.Variant.PolycysticOvaries = sanitisedModel.PolycysticOvaries;
                    healthCheck.Variant.GestationalDiabetes = sanitisedModel.GestationalDiabetes;
                }
                else
                {
                    healthCheck.PolycysticOvaries = sanitisedModel.PolycysticOvaries;
                    healthCheck.GestationalDiabetes = sanitisedModel.GestationalDiabetes;
                }
                
            }

            await Database.SaveChangesAsync();

            return RedirectWithId("./BloodSugar");
        }

        SanitisedModel ValidateAndSanitise(UnsanitisedModel model)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel();

            if (string.IsNullOrEmpty(model.PolycysticOvaries) ||
                (model.PolycysticOvaries != "yes" && model.PolycysticOvaries != "no"))
            {
                PolycysticOvariesError = $"Select yes if you have polycystic ovaries";
                AddError(PolycysticOvariesError, "#polycystic-ovaries");
                isValid = false;
            }
            else
            {
                sanitisedModel.PolycysticOvaries = model.PolycysticOvaries == "yes";
            }

            if (string.IsNullOrEmpty(model.GestationalDiabetes) ||
                (model.GestationalDiabetes != "yes" && model.GestationalDiabetes != "no"))
            {
                GestationalDiabetesError = $"Select yes if you have gestational diabetes";
                AddError(GestationalDiabetesError, "#gestational-diabetes");
                isValid = false;
            }
            else
            {
                sanitisedModel.GestationalDiabetes = model.GestationalDiabetes == "yes";
            }

            return isValid ? sanitisedModel : null;
        }
    }
}