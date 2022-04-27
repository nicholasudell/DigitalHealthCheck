using Microsoft.EntityFrameworkCore;

namespace DigitalHealthCheckEF
{
    [Keyless]
    public class InterventionEligibilityReport
    {
        public Sex? SexForResults { get; set; }
        public int? Age { get; set; }
        public Ethnicity? Ethnicity { get; set; }
        public string Postcode { get; set; }
        public SmokingStatus? SmokingStatus { get; set; }

        public int? IMDQuintile {get;set;}
        public bool HealthCheckCompleted { get; set; }
        public int HighCVD { get; set; }
        public int HighDiabetes {get;set;}
        public int PhysicalActivity0 {get;set;}
        public int PhysicalActivity1 {get;set;}
        public int PhysicalActivity2 {get;set;}
        public int HighMentalHealthRisk {get;set;}
        public int MediumMentalHealthRisk {get;set;}
        public int HighAlcoholRisk {get;set;}

        public int Smoker {get;set;}
        public int BMI25To30 {get;set;}
        public int BMI30Plus {get;set;}
        public int HighCholesterol {get;set;}
        public int HighBloodPressure {get;set;}
        public int MediumBloodSugar {get;set;}
        public int HighBloodSugar {get;set;}
        public int EligibleForGPFollowUp {get;set;}

        public int HighRiskBundle {get;set;}
    }
}