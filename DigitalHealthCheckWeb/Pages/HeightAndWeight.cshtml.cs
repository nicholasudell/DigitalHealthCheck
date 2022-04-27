using System;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Helpers;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{


    public class HeightAndWeightModel : HealthCheckPageModel
    {
        public class UnsanitisedModel
        {
            public string ImperialFeet { get; set; }

            public string ImperialInches { get; set; }

            public string ImperialPounds { get; set; }

            public string ImperialStone { get; set; }

            public string MetricHeight { get; set; }

            public string MetricWeight { get; set; }

            public string SubmitAction { get; set; }
        }

        private class SanitisedModel
        {
            public int? ImperialFeet { get; set; }

            public int? ImperialInches { get; set; }

            public int? ImperialPounds { get; set; }

            public int? ImperialStone { get; set; }

            public float? MetricHeight { get; set; }

            public float? MetricWeight { get; set; }
        }

        private const float KilogramsPerPound = 0.453592f;

        private const float KilogramsPerStone = 6.35029f;

        private const float MetersPerFoot = 0.3048f;

        private const float MetersPerInch = 0.0254f;

        public string ErrorImperialFeet { get; set; }

        public string ErrorImperialInches { get; set; }

        public string ErrorImperialPounds { get; set; }

        public string ErrorImperialStone { get; set; }

        public string ErrorMetricHeight { get; set; }

        public string ErrorMetricWeight { get; set; }

        public UnsanitisedModel Model { get; set; }

        [FromQuery(Name = "useImperial")]
        public bool UseImperial { get; set; } = true;

        public HeightAndWeightModel(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow)
            : base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();

            if (bool.TryParse(Request.Query["useImperial"].ToString(), out var queryUseImperial))
            {
                UseImperial = queryUseImperial;
            }
            else
            {
                UseImperial = healthCheck?.PrefersImperialHeightAndWeight ?? true;
            }

            Model = new UnsanitisedModel
            {
                MetricHeight = ConvertToCm(healthCheck?.Height)?.ToString(),
                MetricWeight = healthCheck?.Weight?.ToString("n2"),
                ImperialFeet = (healthCheck?.Height).HasValue ? ConvertToFeet(healthCheck.Height.Value).ToString() : null,
                ImperialInches = (healthCheck?.Height).HasValue ? ConvertToInches(healthCheck.Height.Value).ToString() : null,
                ImperialStone = (healthCheck?.Weight).HasValue ? ConvertToStone(healthCheck.Weight.Value).ToString() : null,
                ImperialPounds = (healthCheck?.Weight).HasValue ? ConvertToPound(healthCheck.Weight.Value).ToString() : null,
            };
        }

        public async Task<IActionResult> OnPostAsync(UnsanitisedModel model)
        {
            Model = model;

            var healthCheck = await GetHealthCheckAsync();

            if (model.SubmitAction == "DontKnow")
            {
                if (healthCheck is not null)
                {
                    healthCheck.Height = null;

                    healthCheck.Weight = null;

                    healthCheck.HeightAndWeightSkipped = true;
                }

                await Database.SaveChangesAsync();

                return RedirectWithId("./Sex");
            }

            var sanitisedModel = await ValidateAndSanitize(healthCheck, model);

            if (sanitisedModel is null)
            {
                await Database.SaveChangesAsync();

                return await Reload();
            }

            if (healthCheck is not null)
            {
                await Database.Entry(healthCheck).Reference(nameof(HealthCheck.HeightAndWeightFollowUp)).LoadAsync();

                if (healthCheck.Height == null && healthCheck.HeightAndWeightFollowUp is not null)
                {
                    healthCheck.HeightAndWeightUpdated = true;
                    healthCheck.HeightAndWeightUpdatedDate = DateTime.Now;
                }

                healthCheck.Height = UseImperial ?
                     ConvertFromFeetAndInches(sanitisedModel.ImperialFeet.Value, sanitisedModel.ImperialInches.Value) :
                     sanitisedModel.MetricHeight;

                healthCheck.Weight = UseImperial ?
                    ConvertFromPoundAndStone(sanitisedModel.ImperialStone.Value, sanitisedModel.ImperialPounds.Value) :
                    sanitisedModel.MetricWeight;

                healthCheck.PrefersImperialHeightAndWeight = UseImperial;
            }

            await Database.SaveChangesAsync();

            return RedirectWithId("./Sex");
        }

        private static float ConvertFromFeetAndInches(int feet, int inches) =>
            (feet * MetersPerFoot) + (inches * MetersPerInch);

        private static float ConvertFromPoundAndStone(int stone, int pound) =>
            (stone * KilogramsPerStone) + (pound * KilogramsPerPound);

        private static int? ConvertToCm(float? meters) => meters is null ? null : (int)(meters.Value * 100);

        private static int ConvertToFeet(float meters) => (int)(meters / MetersPerFoot);

        private static int ConvertToInches(float meters)
        {
            var feet = ConvertToFeet(meters);
            var feetInMeters = feet * MetersPerFoot;
            var remainder = meters - feetInMeters;

            return (int)(remainder / MetersPerInch);
        }

        private static int ConvertToPound(float kilograms)
        {
            var stone = ConvertToStone(kilograms);
            var stoneInKg = stone * KilogramsPerStone;
            var remainder = kilograms - stoneInKg;

            return (int)(remainder / KilogramsPerPound);
        }

        private static int ConvertToStone(float kilograms) => (int)(kilograms / KilogramsPerStone);

        async Task<SanitisedModel> ValidateAndSanitize(HealthCheck check, UnsanitisedModel model)
        {
            const int MinFeet = 1;
            const int MaxFeet = 9;
            const int MinInches = 0;
            const int MaxInches = 11;
            const int MinCm = 0;
            const int MaxCm = 275;
            const int MinStone = 1;
            const int MaxStone = 80;
            const int MinPounds = 0;
            const int MaxPounds = 13;
            const float MinKg = 1f;
            const float MaxKg = 575f;

            var sanitisedModel = new SanitisedModel();
            var isValid = true;

            if (UseImperial)
            {
                if (string.IsNullOrEmpty(model.ImperialFeet))
                {
                    ErrorImperialFeet = $"Please enter a height between {MinFeet} and {MaxFeet}ft. Please use whole numbers and no decimals.";
                    AddError(ErrorImperialFeet, "#height-feet");
                    isValid = false;
                }
                else if (!int.TryParse(model.ImperialFeet.RemoveSuffix("feet", "ft.", "ft", "'"), out var imperialFeetSanitised))
                {
                    ErrorImperialFeet = $"Please enter a height between {MinFeet} and {MaxFeet}ft. Please use whole numbers and no decimals.";
                    await AddError(check, ErrorImperialFeet, "#height-feet");
                    isValid = false;
                }
                else if (imperialFeetSanitised < MinFeet || imperialFeetSanitised > MaxFeet)
                {
                    ErrorImperialFeet = $"Please enter a height between {MinFeet} and {MaxFeet}ft. Please use whole numbers and no decimals.";
                    await AddError(check, ErrorImperialFeet, "#height-feet");
                    isValid = false;
                }
                else
                {
                    sanitisedModel.ImperialFeet = imperialFeetSanitised;

                    //Allow people to enter "6 ft." and leave the inches blank
                    if (string.IsNullOrEmpty(model.ImperialInches))
                    {
                        model.ImperialInches = "0";
                    }
                }

                if (string.IsNullOrEmpty(model.ImperialInches))
                {
                    ErrorImperialInches = "Your height in inches must be between 0 and 11in because there are 12 inches in one foot. Please use whole numbers and no decimals. For example, instead of writing 66 inches, write 5 feet and 6 inches.";
                    AddError(ErrorImperialInches, "#height-inches");
                    isValid = false;
                }
                else if (!int.TryParse(model.ImperialInches.RemoveSuffix("inches", "in.", "in", "\""), out var imperialInchesSanitised))
                {
                    ErrorImperialInches = "Your height in inches must be between 0 and 11in because there are 12 inches in one foot. Please use whole numbers and no decimals. For example, instead of writing 66 inches, write 5 feet and 6 inches.";
                    await AddError(check, ErrorImperialInches, "#height-inches");
                    isValid = false;
                }
                else if (imperialInchesSanitised < MinInches || imperialInchesSanitised > MaxInches)
                {
                    ErrorImperialInches = $"Your height in inches must be between 0 and 11in because there are 12 inches in one foot. Please use whole numbers and no decimals. For example, instead of writing 66 inches, write 5 feet and 6 inches.";
                    await AddError(check, ErrorImperialInches, "#height-inches");
                    isValid = false;
                }
                else
                {
                    sanitisedModel.ImperialInches = imperialInchesSanitised;
                }

                if (string.IsNullOrEmpty(model.ImperialStone))
                {
                    ErrorImperialStone = $"Please enter a weight between {MinStone} and {MaxStone} stone. Please use whole numbers and avoid decimals.";
                    AddError(ErrorImperialStone, "#weight-stone");
                    isValid = false;
                }
                else if (!int.TryParse(model.ImperialStone.RemoveSuffix("stone", "stones", "st"), out var imperialStoneSanitised))
                {
                    ErrorImperialStone = $"Please enter a weight between {MinStone} and {MaxStone} stone. Please use whole numbers and avoid decimals.";
                    await AddError(check, ErrorImperialStone, "#weight-stone");
                    isValid = false;
                }
                else if (imperialStoneSanitised < MinStone || imperialStoneSanitised > MaxStone)
                {
                    ErrorImperialStone = $"Please enter a weight between {MinStone} and {MaxStone} stone. Please use whole numbers and avoid decimals.";
                    await AddError(check, ErrorImperialStone, "#weight-stone");
                    isValid = false;
                }
                else
                {
                    sanitisedModel.ImperialStone = imperialStoneSanitised;

                    //Allow people to enter "12 st." and leave the pounds blank

                    if (string.IsNullOrEmpty(model.ImperialPounds))
                    {
                        model.ImperialPounds = "0";
                    }
                }

                if (string.IsNullOrEmpty(model.ImperialPounds))
                {
                    ErrorImperialPounds = "Your weight in pounds must be between 0 and 13 because there are 14 pounds in one stone. Please use whole numbers and avoid decimals. For example, instead of writing 160 pounds, write 11 stone and 6 pounds.";
                    AddError(ErrorImperialPounds, "#weight-pounds");
                    isValid = false;
                }
                else if (!int.TryParse(model.ImperialPounds.RemoveSuffix("pound", "pounds", "lb"), out var imperialPoundsSanitised))
                {
                    ErrorImperialPounds = "Your weight in pounds must be between 0 and 13 because there are 14 pounds in one stone. Please use whole numbers and avoid decimals. For example, instead of writing 160 pounds, write 11 stone and 6 pounds.";
                    await AddError(check, ErrorImperialPounds, "#weight-pounds");
                    isValid = false;
                }
                else if (imperialPoundsSanitised < MinPounds || imperialPoundsSanitised > MaxPounds)
                {
                    ErrorImperialPounds = "Your weight in pounds must be between 0 and 13 because there are 14 pounds in one stone. Please use whole numbers and avoid decimals. For example, instead of writing 160 pounds, write 11 stone and 6 pounds.";
                    await AddError(check, ErrorImperialPounds, "#weight-pounds");
                    isValid = false;
                }
                else
                {
                    sanitisedModel.ImperialPounds = imperialPoundsSanitised;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(model.MetricHeight))
                {
                    ErrorMetricHeight = $"Please enter a height between {MinCm} and {MaxCm}cm. Please use whole numbers and no decimals.";
                    AddError(ErrorMetricHeight, "#height");
                    isValid = false;
                }
                else if (!int.TryParse(model.MetricHeight.RemoveSuffix("cm", "centimetres"), out var metricHeightSanitised))
                {
                    ErrorMetricHeight = $"Please enter a height between {MinCm} and {MaxCm}cm. Please use whole numbers and no decimals.";
                    await AddError(check, ErrorMetricHeight, "#height");
                    isValid = false;
                }
                else if (metricHeightSanitised < MinCm || metricHeightSanitised > MaxCm)
                {
                    ErrorMetricHeight = $"Please enter a height between {MinCm} and {MaxCm}cm. Please use whole numbers and no decimals.";
                    await AddError(check, ErrorMetricHeight, "#height");
                    isValid = false;
                }
                else
                {
                    sanitisedModel.MetricHeight = metricHeightSanitised / 100f;
                }

                if (string.IsNullOrEmpty(model.MetricWeight))
                {
                    ErrorMetricWeight = $"Please enter a weight between {MinKg:n0} and {MaxKg:n0}kg. Please use whole numbers and avoid decimals.";
                    AddError(ErrorMetricWeight, "#weight");
                    isValid = false;
                }
                else if (!float.TryParse(model.MetricWeight.RemoveSuffix("kg", "kilograms"), out var metricWeightSanitised))
                {
                    ErrorMetricWeight = $"Please enter a weight between {MinKg:n0} and {MaxKg:n0}kg. Please use whole numbers and avoid decimals.";
                    await AddError(check, ErrorMetricWeight, "#weight");
                    isValid = false;
                }
                else if (metricWeightSanitised < MinKg || metricWeightSanitised > MaxKg)
                {
                    ErrorMetricWeight = $"Please enter a weight between {MinKg:n0} and {MaxKg:n0}kg. Please use whole numbers and avoid decimals.";
                    await AddError(check, ErrorMetricWeight, "#weight");
                    isValid = false;
                }
                else
                {
                    sanitisedModel.MetricWeight = metricWeightSanitised;
                }
            }

            return isValid ? sanitisedModel : null;
        }
    }
}