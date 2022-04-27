using DigitalHealthCheckEF;

namespace QRiskEstimator
{
    public class OutputFileRecord
    {
        public string NHSNumber { get; set; }
        public string UniqueLink { get; set; }
        public int Age { get; set; }
        public YesNoSkip AtrialFibrillation { get; set; }
        public YesNoSkip AtypicalAntipsychoticMedication { get; set; }
        public YesNoSkip Steroids { get; set; }
        public YesNoSkip Migraines { get; set; }
        public YesNoSkip RheumatoidArthritis { get; set; }
        public YesNoSkip ChronicKidneyDisease { get; set; }
        public YesNoSkip SevereMentalIllness { get; set; }
        public YesNoSkip ErectileDysfunction { get; set; }
        public YesNoSkip SystemicLupusErythematosus { get; set; }
        public YesNoSkip BloodPressureTreatment { get; set; }
        public YesNoSkip Type1Diabetes { get; set; }
        public YesNoSkip Type2Diabetes { get; set; }
        public float BMI { get; set; }
        public Sex SexAtBirth { get; set; }
        public Ethnicity Ethnicity { get; set; }
        public YesNoSkip FamilyHistoryCVD { get; set; }
        public float? CholesterolRatio { get; set; }
        public double? SystolicBloodPressure { get; set; }
        public SmokingStatus SmokingStatus { get; set; }
        public string Postcode { get; set; }
        public double QRISK { get; set; }
        
    }
}
