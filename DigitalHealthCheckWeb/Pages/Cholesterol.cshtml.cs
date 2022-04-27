using System;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Helpers;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class CholesterolModel : HealthCheckPageModel
    {
        public class UnsanitisedModel
        {
            public string HdlCholesterolMmoll { get; set; }

            public string KnowYourCholesterol { get; set; }

            public string TotalCholesterolMmoll { get; set; }
        }

        private class SanitisedModel
        {
            public float? HdlCholesterolMmoll { get; set; }

            public HaveYouBeenMeasured KnowYourCholesterol { get; set; }

            public float? TotalCholesterolMmoll { get; set; }
        }

        const int MaxHdlCholesterolMmoll = 12;

        const int MaxTotalCholesterolMmoll = 20;

        const int MinHdlCholesterolMmoll = 0;

        const int MinTotalCholesterolMmoll = 0;

        public UnsanitisedModel Model { get; set; }

        public string HdlCholesterolMmollError { get; set; }

        public string KnowYourCholesterolError { get; set; }

        public string TotalCholesterolMmollError { get; set; }

        public CholesterolModel(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) :
            base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();

            Model = new UnsanitisedModel
            {
                HdlCholesterolMmoll = healthCheck?.HdlCholesterol?.ToString(),
                TotalCholesterolMmoll = healthCheck?.TotalCholesterol?.ToString(),
                KnowYourCholesterol = healthCheck?.KnowYourCholesterol?.ToString().ToLower()
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
                    healthCheck.KnowYourCholesterol != HaveYouBeenMeasured.Yes &&
                    sanitisedModel.KnowYourCholesterol == HaveYouBeenMeasured.Yes)
                {
                    healthCheck.CholesterolUpdated = true;
                    healthCheck.CholesterolUpdatedDate = DateTime.Now;
                }

                healthCheck.KnowYourCholesterol = sanitisedModel.KnowYourCholesterol;

                if (sanitisedModel.KnowYourCholesterol == HaveYouBeenMeasured.Yes)
                {
                    healthCheck.HdlCholesterol = sanitisedModel.HdlCholesterolMmoll;
                    healthCheck.TotalCholesterol = sanitisedModel.TotalCholesterolMmoll;
                }
                else
                {
                    healthCheck.HdlCholesterol = null;
                    healthCheck.TotalCholesterol = null;
                }
            }

            await Database.SaveChangesAsync();

            if (sanitisedModel.KnowYourCholesterol == HaveYouBeenMeasured.Forgot)
            {
                return RedirectWithId("./FindYourCholesterol");
            }

            return RedirectWithId("./MentalWellbeing");
        }

        async Task<SanitisedModel> ValidateAndSanitise(HealthCheck check, UnsanitisedModel model)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel();

            if (string.IsNullOrEmpty(model.KnowYourCholesterol) || !Enum.TryParse<HaveYouBeenMeasured>(model.KnowYourCholesterol, true, out var sanitisedKnowYourCholesterol))
            {
                KnowYourCholesterolError = $"Select yes if you have had your cholesterol measured within the last three months and you remember the number";
                AddError(KnowYourCholesterolError, "#know-your-cholesterol");
                isValid = false;
            }
            else
            {
                sanitisedModel.KnowYourCholesterol = sanitisedKnowYourCholesterol;
            }

            // Need to check the previous Q was valid first for this,
            // because this question is only shown if the first question is answered "yes"
            // and the default value of type HaveYouBeenMeasured is yes.

            if (isValid && sanitisedModel.KnowYourCholesterol == HaveYouBeenMeasured.Yes)
            {
                if (string.IsNullOrEmpty(model.TotalCholesterolMmoll))
                {
                    TotalCholesterolMmollError = "Enter your total cholesterol. If you don't know it, select no or I cannot remember the number";
                    AddError(TotalCholesterolMmollError, "#total-cholesterol");
                    isValid = false;
                }
                else if (!float.TryParse(model.TotalCholesterolMmoll, out var CholesterolSanitised))
                {
                    TotalCholesterolMmollError = "Your total cholesterol must be a number, like 8.62";
                    await AddError(check, TotalCholesterolMmollError, "#total-cholesterol");
                    isValid = false;
                }
                else if (CholesterolSanitised < MinTotalCholesterolMmoll || CholesterolSanitised > MaxTotalCholesterolMmoll)
                {
                    TotalCholesterolMmollError = $"Your total cholesterol must be a number between {MinTotalCholesterolMmoll} and {MaxTotalCholesterolMmoll}";
                    await AddError(check, TotalCholesterolMmollError, "#total-cholesterol");
                    isValid = false;
                }
                else
                {
                    sanitisedModel.TotalCholesterolMmoll = CholesterolSanitised;
                }

                if (string.IsNullOrEmpty(model.HdlCholesterolMmoll))
                {
                    HdlCholesterolMmollError = "Enter your HDL cholesterol. If you don't know it, select no or I cannot remember the number";
                    AddError(HdlCholesterolMmollError, "#hdl-cholesterol");
                    isValid = false;
                }
                else if (!float.TryParse(model.HdlCholesterolMmoll, out var CholesterolSanitised))
                {
                    HdlCholesterolMmollError = "Your HDL cholesterol must be a number, like 8.62";
                    await AddError(check, HdlCholesterolMmollError, "#hdl-cholesterol");
                    isValid = false;
                }
                else if (CholesterolSanitised < MinHdlCholesterolMmoll || CholesterolSanitised > MaxHdlCholesterolMmoll)
                {
                    HdlCholesterolMmollError = $"Your HDL cholesterol must be a number between {MinHdlCholesterolMmoll} and {MaxHdlCholesterolMmoll}";
                    await AddError(check, HdlCholesterolMmollError, "#hdl-cholesterol");
                    isValid = false;
                }
                else
                {
                    sanitisedModel.HdlCholesterolMmoll = CholesterolSanitised;
                }
            }

            return isValid ? sanitisedModel : null;
        }
    }
}