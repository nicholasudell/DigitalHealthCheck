using System;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Helpers;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{

    public class FindYourBloodPressureModel : HealthCheckPageModel
    {
        public class UnsanitisedModel
        {
            public string Diastolic { get; set; }

            public string KnowYourBloodPressure { get; set; }

            public string Systolic { get; set; }
        }

        private class SanitisedModel
        {
            public int? Diastolic { get; set; }

            public HaveYouBeenMeasured KnowYourBloodPressure { get; set; }

            public int? Systolic { get; set; }
        }

        const float MaxBloodPressure = 210;

        const float MinDiastolicBloodPressure = 40;

        const float MinSystolicBloodPressure = 70;

        public UnsanitisedModel Model { get; set; }

        public string DiastolicBloodPressureError { get; set; }

        public string KnowYourBloodPressureError { get; set; }

        public string SystolicBloodPressureError { get; set; }

        public FindYourBloodPressureModel(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) : base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();

            Model = new UnsanitisedModel
            {
                Systolic = healthCheck?.SystolicBloodPressure?.ToString(),
                Diastolic = healthCheck?.DiastolicBloodPressure?.ToString(),
                KnowYourBloodPressure = healthCheck?.KnowYourBloodPressure?.ToString().ToLower()
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
                    healthCheck.KnowYourBloodPressure != HaveYouBeenMeasured.Yes &&
                    sanitisedModel.KnowYourBloodPressure == HaveYouBeenMeasured.Yes)
                {
                    healthCheck.BloodPressureUpdated = true;
                    healthCheck.BloodPressureUpdatedDate = DateTime.Now;
                }

                healthCheck.SystolicBloodPressure = sanitisedModel.Systolic;
                healthCheck.DiastolicBloodPressure = sanitisedModel.Diastolic;
                healthCheck.KnowYourBloodPressure = sanitisedModel.KnowYourBloodPressure;
                healthCheck.WentToFindBloodPressure = true;
            }

            await Database.SaveChangesAsync();

            return RedirectWithId("./Cholesterol");
        }

        async Task<SanitisedModel> ValidateAndSanitise(HealthCheck check, UnsanitisedModel model)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel();

            if (string.IsNullOrEmpty(model.KnowYourBloodPressure) || !Enum.TryParse<HaveYouBeenMeasured>(model.KnowYourBloodPressure, true, out var sanitisedKnowYourBloodPressure))
            {
                KnowYourBloodPressureError = $"Select yes if you have had your blood pressure measured within the last six months and you remember the number.";
                AddError(KnowYourBloodPressureError, "#know-your-blood-pressure");
                isValid = false;
            }
            else
            {
                sanitisedModel.KnowYourBloodPressure = sanitisedKnowYourBloodPressure;
            }

            if (sanitisedModel.KnowYourBloodPressure == HaveYouBeenMeasured.Yes)
            {
                if (string.IsNullOrEmpty(model.Systolic))
                {
                    SystolicBloodPressureError = "Enter your systolic blood pressure";
                    AddError(SystolicBloodPressureError, "#systolic-blood-pressure");
                    isValid = false;
                }
                else if (!int.TryParse(model.Systolic, out var BloodPressureSanitised))
                {
                    SystolicBloodPressureError = "Your systolic blood pressure must be a number, like 80";
                    await AddError(check, SystolicBloodPressureError, "#systolic-blood-pressure");
                    isValid = false;
                }
                else if (BloodPressureSanitised < MinSystolicBloodPressure || BloodPressureSanitised > MaxBloodPressure)
                {
                    SystolicBloodPressureError = $"Your systolic blood pressure must be a number between {MinSystolicBloodPressure} and {MaxBloodPressure}";
                    await AddError(check, SystolicBloodPressureError, "#systolic-blood-pressure");
                    isValid = false;
                }
                else
                {
                    sanitisedModel.Systolic = BloodPressureSanitised;
                }

                if (string.IsNullOrEmpty(model.Diastolic))
                {
                    DiastolicBloodPressureError = "Enter your diastolic blood pressure";
                    AddError(DiastolicBloodPressureError, "#diastolic-blood-pressure");
                    isValid = false;
                }
                else if (!int.TryParse(model.Diastolic, out var BloodPressureSanitised))
                {
                    DiastolicBloodPressureError = "Your diastolic blood pressure must be a number, like 80";
                    await AddError(check, DiastolicBloodPressureError, "#diastolic-blood-pressure");
                    isValid = false;
                }
                else if (BloodPressureSanitised < MinDiastolicBloodPressure || BloodPressureSanitised > MaxBloodPressure)
                {
                    DiastolicBloodPressureError = $"Your diastolic blood pressure must be a number between {MinDiastolicBloodPressure} and {MaxBloodPressure}";
                    await AddError(check, DiastolicBloodPressureError, "#diastolic-blood-pressure");
                    isValid = false;
                }
                else
                {
                    sanitisedModel.Diastolic = BloodPressureSanitised;
                }
            }

            return isValid ? sanitisedModel : null;
        }
    }
}