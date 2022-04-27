using Microsoft.EntityFrameworkCore;

namespace DigitalHealthCheckEF
{
    [Keyless]
    public class DetailedUptakeReport
    {
        public Sex? SexForResults { get; set; }
        public int? Age { get; set; }
        public Ethnicity? Ethnicity { get; set; }
        public string Postcode { get; set; }
        public SmokingStatus? SmokingStatus { get; set; }
        public int? IMDQuintile {get;set;}
        
        public int OpenedLink {get; set;}
        public int ClickedStartNow {get; set;}
        public int HeightAndWeight {get; set;}
        public int Sex {get; set;}
        public int EthnicitySubmitted {get; set;}
        public int Smoking {get; set;}
        public int DrinksAlcohol {get; set;}
        public int PhysicalActivityStarted {get; set;}
        public int PhysicalActivityFinished {get; set;}
        public int BloodSugar {get; set;}
        public int RiskFactors1 {get; set;}
        public int RiskFactors2 {get; set;}
        public int RiskFactors3 {get; set;}
        public int BloodPressure {get; set;}
        public int Cholesterol {get; set;}
        public int Validation {get; set;}
        public int SuccessfulValidation {get; set;}
        public int SkippedValidation {get; set;}
        public int CheckYourAnswers {get; set;}
        public int CheckYourAnswersWithHeightAndWeight {get; set;}
        public int CheckYourAnswersWithoutHeightAndWeight {get; set;}
        public int FollowUpHeightAndWeight {get; set;}
        public int GeneratedResults {get; set;}
        public int SubmittedHealthPrioritiesAfterResults {get; set;}
        public int BookGPAppointmentFollowUp {get; set;}
        public int TotalFollowedUpOnFirstPriority {get; set;}
        public int FollowedUpOnFirstPriorityNoGPFollowUp {get; set;}
        public int FollowedUpOnFirstPriorityGPFollowUp {get; set;}
        public int TotalFollowedUpOnSecondPriority {get; set;}
        public int FollowedUpOnSecondPriorityNoGPFollowUp {get; set;}
        public int FollowedUpOnSecondPriorityGPFollowUp {get; set;}
        public int TotalFollowedUpOnThreeOrMorePriorities {get; set;}
        public int FollowedUpOnThirdPriorityNoGPFollowUp {get; set;}
        public int FollowedUpOnThirdPriorityGPFollowUp {get; set;}
        public int AnyBarriers {get; set;}
        public int AnyInterventions {get; set;}
        public int Completed {get; set;}

    }
}