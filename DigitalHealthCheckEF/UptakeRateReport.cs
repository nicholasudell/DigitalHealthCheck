using Microsoft.EntityFrameworkCore;

namespace DigitalHealthCheckEF
{

    [Keyless]
    public class UptakeRateReport
    {
        public Sex? SexForResults { get; set; }
        public int? Age { get; set; }
        public Ethnicity? Ethnicity { get; set; }
        public string Postcode { get; set; }
        public SmokingStatus? SmokingStatus { get; set; }
        public int Invited { get; set; }
        public int? IMDQuintile {get;set;}
        public int OpenedLink { get; set; }
        public int AnsweredAQuestion {get;set;}
        public int Validation {get;set;}
        public int Results {get;set;}
        public int AtLeast1FollowUp {get;set;}
        public int AtLeast1Intervention {get;set;}
        public int HealthCheckCompleted {get;set;}
        public int UpdatedMeasurements {get;set;}

    }
}