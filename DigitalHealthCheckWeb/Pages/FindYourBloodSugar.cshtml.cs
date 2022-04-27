using System;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Helpers;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class FindYourBloodSugarModel : HealthCheckPageModel
    {
        public class UnsanitisedModel
        {
            public string HbA1c { get; set; }

            public string KnowYourHbA1c { get; set; }
        }

        private class SanitisedModel
        {
            public float? HbA1c { get; set; }

            public HaveYouBeenMeasured KnowYourHbA1c { get; set; }
        }

        const float MaxHba1c = 250f;

        const float MinHbA1c = 18f;

        public UnsanitisedModel Model { get; set; }

        public string HbA1cError { get; set; }

        public string KnowYourHbA1cError { get; set; }

        public string PreviousPage { get; set; }

        public FindYourBloodSugarModel(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) : base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();

            Model = new UnsanitisedModel
            {
                HbA1c = healthCheck?.BloodSugar?.ToString(),
                KnowYourHbA1c = healthCheck?.KnowYourHbA1c?.ToString().ToLower()
            };
        }

        public async Task<IActionResult> OnPostAsync(UnsanitisedModel model)
        {
            Model = model;

            var healthCheck = await GetHealthCheckAsync();

            var sanitisedModel = await ValidateAndSanitise(healthCheck, model);

            if (sanitisedModel is null)
            {
                await Database.SaveChangesAsync();

                return await Reload();
            }

            if (healthCheck is not null)
            {
                if (healthCheck.QRisk is not null &&
                    healthCheck.KnowYourHbA1c != HaveYouBeenMeasured.Yes &&
                    sanitisedModel.KnowYourHbA1c == HaveYouBeenMeasured.Yes)
                {
                    healthCheck.BloodSugarUpdated = true;
                    healthCheck.BloodSugarUpdatedDate = DateTime.Now;
                }

                healthCheck.BloodSugar = sanitisedModel.HbA1c;
                healthCheck.KnowYourHbA1c = sanitisedModel.KnowYourHbA1c;
                healthCheck.WentToFindBloodSugar = true;
            }

            await Database.SaveChangesAsync();

            return RedirectWithId("./RiskFactors1");
        }

        async Task<SanitisedModel> ValidateAndSanitise(HealthCheck check, UnsanitisedModel model)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel();

            if (string.IsNullOrEmpty(model.KnowYourHbA1c) || !Enum.TryParse<HaveYouBeenMeasured>(model.KnowYourHbA1c, true, out var sanitisedKnowYourHbA1c))
            {
                KnowYourHbA1cError = $"Select yes if you have had your blood sugar levels measured within the last six months and you remember the number.";
                AddError(KnowYourHbA1cError, "#know-your-hba1c");
                isValid = false;
            }
            else
            {
                sanitisedModel.KnowYourHbA1c = sanitisedKnowYourHbA1c;
            }

            if (sanitisedModel.KnowYourHbA1c == HaveYouBeenMeasured.Yes)
            {
                if (string.IsNullOrEmpty(model.HbA1c))
                {
                    HbA1cError = $"Please enter a HBA1c between {MinHbA1c:n2} and {MaxHba1c:n2}.";
                    AddError(HbA1cError, "#hba1c");
                    isValid = false;
                }
                else if (!float.TryParse(model.HbA1c, out var hbA1cSanitised))
                {
                    HbA1cError = $"Please enter a HBA1c between {MinHbA1c:n2} and {MaxHba1c:n2}.";
                    await AddError(check, HbA1cError, "#hba1c");
                    isValid = false;
                }
                else if (hbA1cSanitised < MinHbA1c || hbA1cSanitised > MaxHba1c)
                {
                    HbA1cError = $"Please enter a HBA1c between {MinHbA1c:n2} and {MaxHba1c:n2}.";
                    await AddError(check, HbA1cError, "#hba1c");
                    isValid = false;
                }
                else
                {
                    sanitisedModel.HbA1c = hbA1cSanitised;
                }
            }

            return isValid ? sanitisedModel : null;
        }
    }
}