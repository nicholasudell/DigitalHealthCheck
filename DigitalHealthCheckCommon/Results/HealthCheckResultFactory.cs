using System;
using DigitalHealthCheckEF;

namespace DigitalHealthCheckCommon
{
    /// <summary>
    /// Creates <see cref="Result"/> instances from <see cref="HealthCheck"/> instances.
    /// </summary>
    /// <seealso cref="IHealthCheckResultFactory" />
    public class HealthCheckResultFactory : IHealthCheckResultFactory
    {
        private readonly IBodyMassIndexCalculator bodyMassIndexCalculator;

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthCheckResultFactory"/> class.
        /// </summary>
        /// <param name="bodyMassIndexCalculator">The body mass index calculator.</param>
        public HealthCheckResultFactory(IBodyMassIndexCalculator bodyMassIndexCalculator) =>
            this.bodyMassIndexCalculator = bodyMassIndexCalculator ?? throw new ArgumentNullException(nameof(bodyMassIndexCalculator));

        /// <summary>
        /// Analyses a health check to determine the health state of the user.
        /// Each status is along the lines of "Healthy", "Warning", "Danger"
        /// although some vary (e.g. Blood Pressure where it's possible to be
        /// too low).
        /// </summary>
        /// <param name="check">The check to analyse.</param>
        /// <returns>
        /// A <see cref="Result" /> object.
        /// </returns>
        public Result GetResult(HealthCheck check, bool useVariant)
        {
            if (check is null)
            {
                throw new ArgumentNullException(nameof(check));
            }

            if (!check.Weight.HasValue || !check.Height.HasValue)
            {
                throw new InvalidOperationException("Cannot calculate BMI without height and weight.");
            }

            var bmi = bodyMassIndexCalculator.CalculateBodyMassIndex(check.Height.Value, check.Weight.Value);

            // Certain ethnic groups have a lower threshold for being considered overweight in BMI calcs
            // because they are at a higher risk

            var higherBmiRiskEthnicity = check.Ethnicity.Value.IsHighBMIRisk();

            float? cholesterolRatio = (check.TotalCholesterol.HasValue && check.HdlCholesterol.HasValue) ? ((float)check.TotalCholesterol.Value) / check.HdlCholesterol.Value : null;

            BloodPressureStatus? systolicRisk = check.SystolicBloodPressure switch
            {
                < 90 => BloodPressureStatus.Low,
                < 120 => BloodPressureStatus.Healthy,
                < 140 => BloodPressureStatus.SlightlyHigh,
                < 180 => BloodPressureStatus.High,
                >= 180 => BloodPressureStatus.Severe,
                _ => null
            };

            BloodPressureStatus? diastolicRisk = check.DiastolicBloodPressure switch
            {
                < 60 => BloodPressureStatus.Low,
                < 80 => BloodPressureStatus.Healthy,
                < 90 => BloodPressureStatus.SlightlyHigh,
                < 110 => BloodPressureStatus.High,
                >= 110 => BloodPressureStatus.Severe,
                _ => null
            };

            BloodPressureStatus? bpRisk = null;

            if (systolicRisk.HasValue && diastolicRisk.HasValue)
            {
                bpRisk = (systolicRisk == BloodPressureStatus.Low) || (diastolicRisk == BloodPressureStatus.Low) ?
                        BloodPressureStatus.Low :
                        (BloodPressureStatus?)Math.Max((int)systolicRisk, (int)diastolicRisk);
            }

            // Calculation for working out when Heart Age is considered dangerous.
            var heartAgeThreshold = (int)Math.Round(((check.Age.Value - 40) / 5f) + 5);

            var sex = useVariant ? check.Variant.Sex.Value : check.SexForResults.Value;

            var result = new Result
            {
                Smoker = check.SmokingStatus switch
                {
                    SmokingStatus.ExSmoker or SmokingStatus.NonSmoker => DefaultStatus.Healthy,
                    _ => DefaultStatus.Danger
                },

                Alcohol = (useVariant? check.Variant.AUDIT : check.AUDIT) switch
                {
                    < 8 => DefaultStatus.Healthy,
                    < 16 => DefaultStatus.Warning,
                    >= 16 => DefaultStatus.Danger,
                    _ => throw new InvalidOperationException("Cannot calculate alcohol without an AUDIT score.")
                },

                PhysicalActivity = (useVariant ? check.Variant.GPPAQ : check.GPPAQ) switch
                {
                    3 => PhysicalActivityStatus.Active,
                    2 => PhysicalActivityStatus.MostlyActive,
                    1 => PhysicalActivityStatus.MostlyInactive,
                    0 => PhysicalActivityStatus.Sedentary,
                    _ => throw new InvalidOperationException("Cannot calculate physical activity without a GPPAQ score.")
                },

                BodyMassIndex = bmi switch
                {
                    < 16f => BodyMassIndexStatus.SeverelyUnderweight,
                    < 18.5f => BodyMassIndexStatus.Underweight,
                    var n when (n > 27.5f && higherBmiRiskEthnicity) => BodyMassIndexStatus.Obese,
                    var n when (n > 30f && !higherBmiRiskEthnicity) => BodyMassIndexStatus.Obese,
                    var n when (n > 23f && higherBmiRiskEthnicity) => BodyMassIndexStatus.Overweight,
                    var n when (n > 25f && !higherBmiRiskEthnicity) => BodyMassIndexStatus.Overweight,
                    _ => BodyMassIndexStatus.Healthy //Check for BMI existance is done at the top.
                },

                BloodPressure = bpRisk,

                Cholesterol = check.TotalCholesterol switch
                {
                    <= 5 => DefaultStatus.Healthy,
                    < 7.5f => DefaultStatus.Warning,
                    >= 7.5f => DefaultStatus.Danger,
                    _ => null
                },

                CholesterolRatio = cholesterolRatio switch
                {
                    > 6f => DefaultStatus.Danger,
                    <= 6f => DefaultStatus.Healthy,
                    _ => null
                },

                HdlCholesterol = check.HdlCholesterol switch
                {
                    var n when (n >= 18f && sex == Sex.Male) => DefaultStatus.Danger,
                    var n when (n >= 21.6f && sex == Sex.Female) => DefaultStatus.Danger,
                    not null => DefaultStatus.Healthy,
                    _ => null
                },

                HeartAge = ((useVariant ? check.Variant.HeartAge : check.HeartAge) - check.Age) switch
                {
                    var n when (n >= heartAgeThreshold) => DefaultStatus.Danger,
                    _ => DefaultStatus.Healthy
                },

                HeartDisease = (useVariant ? check.Variant.QRisk : check.QRisk) switch
                {
                    >= 20 => DefaultStatus.Danger,
                    >= 10 => DefaultStatus.Warning,
                    < 10 => DefaultStatus.Healthy,
                    _ => throw new InvalidOperationException("Cannot calculate heart disease risk without a QRISK score.")
                },

                Diabetes = (useVariant ? check.Variant.QDiabetes : check.QDiabetes) switch
                {
                    >= 5.6 => DefaultStatus.Danger,
                    < 5.6 => DefaultStatus.Healthy,
                    _ => throw new InvalidOperationException("Cannot calculate diabetes risk without a QDIABETES score.")
                },

                BloodSugar = check.BloodSugar switch
                {
                    < 42 => BloodSugarStatus.Healthy,
                    <= 47 => BloodSugarStatus.Warning,
                    <= 100 => BloodSugarStatus.Danger,
                    > 100 => BloodSugarStatus.Severe,
                    _ => null
                },

                Walking = check.Walking.Value > GPPAQActivityLevel.LessThanOneHour,

                MentalWellbeing = check.SkipMentalHealthQuestions == true ? null : (useVariant ? check.Variant.GAD2 : check.GAD2) switch
                {
                    var x when (x >= 5 && check.FeelingDown == true && check.Disinterested == true) => DefaultStatus.Danger,
                    var x when (x < 2 && check.FeelingDown == false && check.Disinterested == false) => DefaultStatus.Healthy,
                    _ => DefaultStatus.Warning
                }
            };

            return result;
        }
    }
}