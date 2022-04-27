using System;
using System.Collections.Generic;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using QMSUK.QRisk;

namespace DigitalHealthCheckWeb.Model
{
    /// <summary>
    /// Calculates user risk scores based on their measurements using QMS' implementations.
    /// </summary>
    /// <seealso cref="DigitalHealthCheckWeb.Model.IRiskScoreCalculator" />
    public class QMSRiskCalculator : IRiskScoreCalculator
    {
        public const double AverageFemaleCholesterolRatio = 3.7d;

        public const double AverageFemaleSystolicBloodPressure = 123.2d;

        public const double AverageHbA1c = 37.2d;

        public const double AverageMaleCholesterolRatio = 4.4d;

        public const double AverageMaleSystolicBloodPressure = 129.2d;

        public const double MaxHbA1c = 47.99d;

        public const int MaximumBMI = 40;

        public const double MinHbA1c = 15d;

        public const int MinimumBMI = 20;

        private readonly IBodyMassIndexCalculator bodyMassIndexCalculator;

        readonly IDictionary<DigitalHealthCheckEF.Ethnicity, QMSUK.QRisk.Ethnicity> ethnicityMappings = new Dictionary<DigitalHealthCheckEF.Ethnicity, QMSUK.QRisk.Ethnicity>()
        {
            {DigitalHealthCheckEF.Ethnicity.African, QMSUK.QRisk.Ethnicity.BlackAfrican },
            {DigitalHealthCheckEF.Ethnicity.Arab, QMSUK.QRisk.Ethnicity.OtherAsian },
            {DigitalHealthCheckEF.Ethnicity.AsianOther, QMSUK.QRisk.Ethnicity.OtherAsian },
            {DigitalHealthCheckEF.Ethnicity.Bangladeshi, QMSUK.QRisk.Ethnicity.Bangladeshi },
            {DigitalHealthCheckEF.Ethnicity.BlackOther, QMSUK.QRisk.Ethnicity.OtherBlack },
            {DigitalHealthCheckEF.Ethnicity.Caribbean, QMSUK.QRisk.Ethnicity.Caribbean },
            {DigitalHealthCheckEF.Ethnicity.Chinese, QMSUK.QRisk.Ethnicity.Chinese },
            {DigitalHealthCheckEF.Ethnicity.GypsyOrTraveller, QMSUK.QRisk.Ethnicity.OtherWhiteBackground},
            {DigitalHealthCheckEF.Ethnicity.Indian, QMSUK.QRisk.Ethnicity.Indian },
            {DigitalHealthCheckEF.Ethnicity.Irish, QMSUK.QRisk.Ethnicity.Irish },
            {DigitalHealthCheckEF.Ethnicity.MixedOther, QMSUK.QRisk.Ethnicity.OtherMixed},
            {DigitalHealthCheckEF.Ethnicity.OtherOther, QMSUK.QRisk.Ethnicity.OtherEthnicGroup },
            {DigitalHealthCheckEF.Ethnicity.Pakistani, QMSUK.QRisk.Ethnicity.Pakistani },
            {DigitalHealthCheckEF.Ethnicity.WhiteAsian, QMSUK.QRisk.Ethnicity.WhiteAndAsianMixed },
            {DigitalHealthCheckEF.Ethnicity.WhiteBlackAfrican, QMSUK.QRisk.Ethnicity.WhiteAndBlackAfricanMixed },
            {DigitalHealthCheckEF.Ethnicity.WhiteBlackCaribbean, QMSUK.QRisk.Ethnicity.WhiteAndBlackCaribbeanMixed},
            {DigitalHealthCheckEF.Ethnicity.WhiteBritish, QMSUK.QRisk.Ethnicity.British},
            {DigitalHealthCheckEF.Ethnicity.WhiteOther, QMSUK.QRisk.Ethnicity.OtherWhiteBackground }
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="QMSRiskCalculator"/> class.
        /// </summary>
        /// <param name="bodyMassIndexCalculator">The body mass index calculator.</param>
        public QMSRiskCalculator(IBodyMassIndexCalculator bodyMassIndexCalculator) =>
            this.bodyMassIndexCalculator = bodyMassIndexCalculator;

        /// <summary>
        /// Calculates the 10-year risk of developing cardiovascular disease (CVD).
        /// </summary>
        /// <param name="check">The health check.</param>
        /// <param name="townsendScore">The townsend deprivation score.</param>
        /// <returns>
        /// A value between 0 and 100 representing your percentage chance.
        /// </returns>
        public double Calculate10YearCVDRiskScore(HealthCheck check, double townsendScore, bool useVariant)
        {
            if (check is null)
            {
                throw new ArgumentNullException(nameof(check));
            }

            var calculator = new QMSUK.QRisk.QRisk3();

            var sex = useVariant ? check.Variant.Sex : check.SexForResults;

            if (sex == Sex.Female)
            {
                return calculator.CalculateFemaleRisk(
                        check.Age.Value,
                        ConvertToInt(check.AtrialFibrillation),
                        ConvertToInt(check.AtypicalAntipsychoticMedication),
                        ConvertToInt(check.Steroids),
                        ConvertToInt(check.Migraines),
                        ConvertToInt(check.RheumatoidArthritis),
                        ConvertToInt(check.ChronicKidneyDisease),
                        ConvertToInt(check.SevereMentalIllness),
                        ConvertToInt(check.SystemicLupusErythematosus),
                        ConvertToInt(check.BloodPressureTreatment),
                        0, // do you have type 1 diabetes,
                        0, // do you have type 2 diabetes,
                        CalculateBmi(check.Height.Value, check.Weight.Value),
                        Convert(check.Ethnicity.Value),
                        ConvertToInt(check.FamilyHistoryCVD),
                        (check.HdlCholesterol.HasValue && check.TotalCholesterol.HasValue) ?
                            CalculateCholesterolRatio(check.HdlCholesterol.Value, check.TotalCholesterol.Value) :
                            AverageFemaleCholesterolRatio,
                        check.SystolicBloodPressure ?? AverageFemaleSystolicBloodPressure,
                        0, // standard deviation of last two blood pressures
                        Convert(check.SmokingStatus.Value),
                        townsendScore
                    );
            }
            else
            {
                return calculator.CalculateMaleRisk(
                        check.Age.Value,
                        ConvertToInt(check.AtrialFibrillation),
                        ConvertToInt(check.AtypicalAntipsychoticMedication),
                        ConvertToInt(check.Steroids),
                        0, // impotence
                        ConvertToInt(check.Migraines),
                        ConvertToInt(check.RheumatoidArthritis),
                        ConvertToInt(check.ChronicKidneyDisease),
                        ConvertToInt(check.SevereMentalIllness),
                        ConvertToInt(check.SystemicLupusErythematosus),
                        ConvertToInt(check.BloodPressureTreatment),
                        0, // do you have type 1 diabetes,
                        0, // do you have type 2 diabetes,
                        CalculateBmi(check.Height.Value, check.Weight.Value),
                        Convert(check.Ethnicity.Value),
                        ConvertToInt(check.FamilyHistoryCVD),
                        (check.HdlCholesterol.HasValue && check.TotalCholesterol.HasValue) ?
                            CalculateCholesterolRatio(check.HdlCholesterol.Value, check.TotalCholesterol.Value) :
                            AverageMaleCholesterolRatio,
                        check.SystolicBloodPressure ?? AverageMaleSystolicBloodPressure,
                        0, // standard deviation of last two blood pressures
                        Convert(check.SmokingStatus.Value),
                        townsendScore
                    );
            }
        }

        public double Calculate10YearDiabetesRiskScore(HealthCheck check, double townsendScore, bool useVariant)
        {
            var bloodSugar = check.BloodSugar ?? AverageHbA1c;

            var clampedBloodSugar = Math.Max(Math.Min(bloodSugar, MaxHbA1c), MinHbA1c);

            var bmi = CalculateBmi(check.Height.Value, check.Weight.Value);

            var sex = useVariant ? check.Variant.Sex : check.SexForResults;

            var shouldUseFemaleSexAtBirthFactors = check.SexAtBirth == Sex.Female;

            return sex switch
            {
                Sex.Female => QDiabetes.CalculateFemaleRisk(
                    check.Age.Value,
                    check.AtypicalAntipsychoticMedication.HasValue && check.AtypicalAntipsychoticMedication.Value == YesNoSkip.Yes, //atypical antipsychotics
                    check.Steroids.Value,
                    false, //CVD
                    shouldUseFemaleSexAtBirthFactors && (useVariant ? check.Variant.GestationalDiabetes.Value : check.GestationalDiabetes.Value), //gestational diabetes
                    false, //learning difficulties
                    false, //manic schizophrenia
                    shouldUseFemaleSexAtBirthFactors && (useVariant ? check.Variant.PolycysticOvaries.Value : check.PolycysticOvaries.Value), //polycystic ovaries
                    false, //statins
                    check.BloodPressureTreatment.Value, //treated for Hypertension
                    check.FamilyHistoryDiabetes.Value,
                    clampedBloodSugar,
                    Convert(check.Ethnicity.Value),
                    Convert(check.SmokingStatus.Value),
                    townsendScore,
                    bmi),
                _ => QDiabetes.CalculateMaleRisk(
                    check.Age.Value,
                    check.AtypicalAntipsychoticMedication.HasValue && check.AtypicalAntipsychoticMedication.Value == YesNoSkip.Yes, //atypical antipsychotics
                    check.Steroids.Value,
                    false, //CVD
                    false, //learning difficulties
                    false, //manic schizophrenia
                    false, //statins
                    check.BloodPressureTreatment.Value, //treated for Hypertension
                    check.FamilyHistoryDiabetes.Value,
                    clampedBloodSugar,
                    Convert(check.Ethnicity.Value),
                    Convert(check.SmokingStatus.Value),
                    townsendScore,
                    bmi),
            };
        }

        /// <summary>
        /// Calculates the Alcohol Use Disorders Identification Test (AUDIT) score.
        /// </summary>
        /// <param name="check">The health check.</param>
        /// <returns>
        /// A value between 0 and 40, where 0 indicates less evidence of alcohol use disorders.
        /// </returns>
        public int CalculateAuditScore(HealthCheck check, bool useVariant)
        {
            if (!check.DrinksAlcohol.Value || check.MSASQ == AUDITFrequency.Never || check.MSASQ == AUDITFrequency.LessThanMonthly)
            {
                return -1;
            }

            // Each AUDIT enum directly maps to the AUDIT score system except DrinkingFrequency

            var drinkingFrequency = check.DrinkingFrequency switch
            {
                AUDITDrinkingFrequency.Never => 0,
                AUDITDrinkingFrequency.LessThanMonthly => 1,
                AUDITDrinkingFrequency.Monthly => 1, //Monthly and less than monthly have the same value
                AUDITDrinkingFrequency.TwoToFourPerMonth => 2,
                AUDITDrinkingFrequency.TwoToThreePerWeek => 3,
                AUDITDrinkingFrequency.FourTimesWeeklyPlus => 4
            };

            return (int)check.FailedResponsibilityDueToAlcohol
                + drinkingFrequency
                + (int)(useVariant ? check.Variant.MSASQ : check.MSASQ)
                + (int)check.GuiltAfterDrinking
                + (int)check.MemoryLossAfterDrinking
                + (int)check.NeededToDrinkAlcoholMorningAfter
                + (int)check.UnableToStopDrinking
                + (int)check.ContactsConcernedByDrinking
                + (int)check.InjuryCausedByDrinking
                + ConvertUnitsToAuditScore((int)check.TypicalDayAlcoholUnits);
        }

        /// <summary>
        /// Calculates the General Anxiety Disorder-2 score.
        /// </summary>
        /// <param name="check">The health check.</param>
        /// <returns>
        /// A value between 0 and 7, where 0 indicates unlikely to have general anxiety disorder.
        /// </returns>
        public int CalculateGAD2Score(HealthCheck check, bool useVariant) =>
            (int)check.Anxious + (int)check.Control;

        /// <summary>
        /// Calculates the General Practice Physical Activity Questionnaire (GPPAQ) score.
        /// </summary>
        /// <param name="check">The health check.</param>
        /// <returns>
        /// A value between 0 and 3, where 0 indicates no activity.
        /// </returns>
        public int CalculateGPPAQScore(HealthCheck check, bool useVariant)
        {
            var exerciseScore = Math.Min((int)check.PhysicalActivity + (int)check.Cycling, 3);

            // There are two work scores that map to a working score of 0, Not In Employment and
            // Sedentary, so we subtract one and Max(0,x) it so they both equal 0.
            var workScore = Math.Max(0, (int)check.WorkActivity - 1);

            return Math.Min(exerciseScore + workScore, 3);
        }

        /// <summary>
        /// Calculates the relative age a person with no modifiable risk factors
        /// would have to be to get the same CVD risk score provided.
        /// </summary>
        /// <param name="check">The health check.</param>
        /// <param name="tenYearCVDRiskScore">The CVD risk score to find a comparable heart age for.</param>
        /// <returns>
        /// An age between 0 and 84. If the age is above 84 it returns 84 still.
        /// </returns>
        public int CalculateHeartAge(HealthCheck check, double tenYearCVDRiskScore, bool useVariant)
        {
            var sex = useVariant ? check.Variant.Sex : check.SexForResults;

            //If your heart age is > 84, this returns null, so set to 84.
            return QMSUK.QRisk.QRisk3.HeartAge(Convert(sex.Value), Convert(check.Ethnicity.Value), tenYearCVDRiskScore) ?? 84;
        }

        private static double CalculateCholesterolRatio(float hdlCholesterol, float totalCholesterol) => totalCholesterol / hdlCholesterol;

        static Smoking Convert(SmokingStatus smoking) => (Smoking)(int)smoking;

        static Gender Convert(Sex sex) => sex == Sex.Female ? Gender.Female : Gender.Male;

        static int ConvertToInt(YesNoSkip? val) => (val.HasValue && val == YesNoSkip.Yes) ? 1 : 0;

        static int ConvertToInt(bool? val) => (val.HasValue && val.Value) ? 1 : 0;

        private static int ConvertUnitsToAuditScore(int units) =>
            units <= 2 ? 0 :
            units <= 4 ? 1 :
            units <= 6 ? 2 :
            units <= 9 ? 3 :
                         4;

        private static double NormaliseBodyMassIndex(double bodyMassIndex) =>
            Math.Max(Math.Min(bodyMassIndex, MaximumBMI), MinimumBMI);

        private double CalculateBmi(float height, float weight) =>
            NormaliseBodyMassIndex(bodyMassIndexCalculator.CalculateBodyMassIndex(height, weight));

        QMSUK.QRisk.Ethnicity Convert(DigitalHealthCheckEF.Ethnicity ethnicity) =>
            ethnicityMappings[ethnicity];
    }
}