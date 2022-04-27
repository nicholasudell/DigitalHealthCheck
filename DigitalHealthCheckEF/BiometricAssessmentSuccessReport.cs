using Microsoft.EntityFrameworkCore;

namespace DigitalHealthCheckEF
{
    [Keyless]
    public class BiometricAssessmentSuccessReport
    {
        public Sex? SexForResults { get; set; }
        public int? Age { get; set; }
        public Ethnicity? Ethnicity { get; set; }
        public string Postcode { get; set; }
        public SmokingStatus? SmokingStatus { get; set; }

        public int? IMDQuintile {get;set;}
        
        public int UnknownBP {get;set;}
        public int UnknownCholesterol {get;set;}
        public int UnknownHeightAndWeight {get;set;}
        public int UnknownBloodSugar {get;set;}
        public int HasAllBiometrics {get;set;}
        public int NeedsAnyBiometric {get;set;}
        public int BloodPressureUpdated {get;set;}
        public int CholesterolUpdated {get;set;}
        public int BloodSugarUpdated {get;set;}
        public int HeightAndWeightUpdated {get;set;}
        public int GotAtLeastOneBiometric {get;set;}
    }
}
