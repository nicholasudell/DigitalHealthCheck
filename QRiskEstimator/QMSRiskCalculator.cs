using System;
using System.Collections.Generic;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using QMSUK.QRisk;

namespace QRiskEstimator
{
    /// <summary>
    /// Calculates user risk scores based on their measurements using QMS' implementations.
    /// </summary>
    public class QMSRiskCalculator
    {
        public const double AverageFemaleCholesterolRatio = 3.7d;

        public const double AverageFemaleSystolicBloodPressure = 123.2d;

        public const double AverageMaleCholesterolRatio = 4.4d;

        public const double AverageMaleSystolicBloodPressure = 129.2d;

        public const int MaximumBMI = 40;

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
        public double Calculate10YearCVDRiskScore(OutputFileRecord check, double townsendScore)
        {
            if (check is null)
            {
                throw new ArgumentNullException(nameof(check));
            }

            var calculator = new QMSUK.QRisk.QRisk3();

            var sex = check.SexAtBirth;

            var b_AF = ConvertToInt(check.AtrialFibrillation);
            var b_atypicalantipsy = ConvertToInt(check.AtypicalAntipsychoticMedication);
            var b_corticosteroids = ConvertToInt(check.Steroids);
            var b_migraine = ConvertToInt(check.Migraines);
            var b_ra = ConvertToInt(check.RheumatoidArthritis);
            var b_renal = ConvertToInt(check.ChronicKidneyDisease);
            var b_semi = ConvertToInt(check.SevereMentalIllness);
            var b_sle = ConvertToInt(check.SystemicLupusErythematosus);
            var b_treatedhyp = ConvertToInt(check.BloodPressureTreatment);
            var b_type1 = ConvertToInt(check.Type1Diabetes);
            var b_type2 = ConvertToInt(check.Type2Diabetes);
            var bmi = NormaliseBodyMassIndex(check.BMI);
            var ethnicity = Convert(check.Ethnicity);
            var fh_cvd = ConvertToInt(check.FamilyHistoryCVD);
            var rati = check.CholesterolRatio ?? AverageMaleCholesterolRatio;
            var sbp = check.SystolicBloodPressure ?? AverageMaleSystolicBloodPressure;
            var smoking = Convert(check.SmokingStatus);

            if (sex == Sex.Female)
            {
                
                return calculator.CalculateFemaleRisk(
                        age: check.Age,
                        b_AF: b_AF,
                        b_atypicalantipsy: b_atypicalantipsy,
                        b_corticosteroids: b_corticosteroids,
                        b_migraine: b_migraine,
                        b_ra: b_ra,
                        b_renal: b_renal,
                        b_semi: b_semi,
                        b_sle: b_sle,
                        b_treatedhyp: b_treatedhyp,
                        b_type1: b_type1,
                        b_type2: b_type2,
                        bmi: bmi,
                        ethnicity: ethnicity,
                        fh_cvd: fh_cvd,
                        rati: rati,
                        sbp: sbp,
                        sbps5: 0, // standard deviation of last two blood pressures
                        smoking: smoking,
                        town: townsendScore
                    );
            }
            else
            {
                var b_impotence2 = ConvertToInt(check.ErectileDysfunction);

                
                return calculator.CalculateMaleRisk(
                        age: check.Age,
                        b_AF: b_AF,
                        b_atypicalantipsy: b_atypicalantipsy,
                        b_corticosteroids: b_corticosteroids,
                        b_impotence2: b_impotence2,
                        b_migraine: b_migraine,
                        b_ra: b_ra,
                        b_renal: b_renal,
                        b_semi: b_semi,
                        b_sle: b_sle,
                        b_treatedhyp: b_treatedhyp,
                        b_type1: b_type1,
                        b_type2: b_type2,
                        bmi: bmi,
                        ethnicity: ethnicity,
                        fh_cvd: fh_cvd,
                        rati: rati,
                        sbp: sbp,
                        sbps5: 0, // standard deviation of last two blood pressures
                        smoking: smoking,
                        town: townsendScore
                    );
            }
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
        public int CalculateHeartAge(OutputFileRecord check, double tenYearCVDRiskScore, bool useVariant)
        {
            var sex = check.SexAtBirth;

            //If your heart age is > 84, this returns null, so set to 84.
            return QRisk3.HeartAge(Convert(sex), Convert(check.Ethnicity), tenYearCVDRiskScore) ?? 84;
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