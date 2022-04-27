using System;
using System.Threading.Tasks;
using DigitalHealthCheckEF;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace DigitalHealthCheckWeb.Model
{


    public class InterventionEligibilityReport : Report<InterventionEligibilityReport.InterventionEligibilityRecord>
    {
        public class InterventionEligibilityRecord
        {
            public string Cohort { get; set; }
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
            public int HasAnyHighRisk {get;set;}
        }

        private readonly Database database;
        private readonly DateTime? from;
        private readonly DateTime? to;

        public InterventionEligibilityReport(Database database, DateTime? from = null, DateTime? to = null) : base
        (
            new CsvConfiguration(CultureInfo.CurrentCulture),
            csv =>
            {
                csv.Context.RegisterClassMap<InterventionEligibilityMap>();
            }
        )
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
            this.from = from;
            this.to = to;
        }

        protected override async Task<IList<InterventionEligibilityRecord>> GetRecordsAsync()
        {
            database.Database.SetCommandTimeout(TimeSpan.FromMinutes(10));

            var rawUptake = await database.InterventionEligibilityReport(from, to).ToListAsync();

            var blackAsianOrMixedEthnicities = new[]
            {
                Ethnicity.WhiteBlackCaribbean,
                Ethnicity.WhiteBlackAfrican,
                Ethnicity.WhiteAsian,
                Ethnicity.MixedOther,
                Ethnicity.Indian,
                Ethnicity.Pakistani,
                Ethnicity.Bangladeshi,
                Ethnicity.Chinese,
                Ethnicity.AsianOther,
                Ethnicity.African,
                Ethnicity.Caribbean,
                Ethnicity.BlackOther
            };

            var finishedUptake = rawUptake.Where(x=> x.HealthCheckCompleted);
            var unfinishedUptake = rawUptake.Where(x=> !x.HealthCheckCompleted);

            return new List<InterventionEligibilityRecord>()
            {
                CreateRecord(unfinishedUptake, "Full Group Unfinished"),
                CreateRecord(unfinishedUptake.Where(x=> x.SexForResults == Sex.Male), "Men Only Unfinished"),
                CreateRecord(unfinishedUptake.Where(x=> x.Age >= 60), "Aged 60+ Unfinished"),
                CreateRecord(unfinishedUptake.Where(x=> x.Ethnicity.HasValue &&  blackAsianOrMixedEthnicities.Contains(x.Ethnicity.Value) ), "Black Asian or Mixed Ethnicity Unfinished"),
                CreateRecord(unfinishedUptake.Where(x=> !string.IsNullOrEmpty(x.Postcode) && x.IMDQuintile != null && x.IMDQuintile <=2 ), "Deprivation Lowest 2 Quintiles Unfinished"),
                CreateRecord(unfinishedUptake.Where(x=> x.SmokingStatus == SmokingStatus.Light || x.SmokingStatus == SmokingStatus.Moderate || x.SmokingStatus == SmokingStatus.Heavy), "Smokers Unfinished"),
                
                CreateRecord(finishedUptake, "Full Group Finished"),
                CreateRecord(finishedUptake.Where(x=> x.SexForResults == Sex.Male), "Men Only Finished"),
                CreateRecord(finishedUptake.Where(x=> x.Age >= 60), "Aged 60+ Finished"),
                CreateRecord(finishedUptake.Where(x=> x.Ethnicity.HasValue &&  blackAsianOrMixedEthnicities.Contains(x.Ethnicity.Value) ), "Black Asian or Mixed Ethnicity Finished"),
                CreateRecord(finishedUptake.Where(x=> !string.IsNullOrEmpty(x.Postcode) && x.IMDQuintile != null && x.IMDQuintile <=2 ), "Deprivation Lowest 2 Quintiles Finished"),
                CreateRecord(finishedUptake.Where(x=> x.SmokingStatus == SmokingStatus.Light || x.SmokingStatus == SmokingStatus.Moderate || x.SmokingStatus == SmokingStatus.Heavy), "Smokers Finished"),
            };
        }

        InterventionEligibilityRecord CreateRecord(IEnumerable<DigitalHealthCheckEF.InterventionEligibilityReport> rawUptake, string cohort)
        {
            return new InterventionEligibilityRecord
            {
                Cohort = cohort,
                HighCVD = rawUptake.Sum(x=> x.HighCVD),
                HighDiabetes = rawUptake.Sum(x=> x.HighDiabetes),
                PhysicalActivity0 = rawUptake.Sum(x=> x.PhysicalActivity0),
                PhysicalActivity1 = rawUptake.Sum(x=> x.PhysicalActivity1),
                PhysicalActivity2 = rawUptake.Sum(x=> x.PhysicalActivity2),
                HighMentalHealthRisk = rawUptake.Sum(x=> x.HighMentalHealthRisk),
                MediumMentalHealthRisk = rawUptake.Sum(x=> x.MediumMentalHealthRisk),
                HighAlcoholRisk = rawUptake.Sum(x=> x.HighAlcoholRisk),
                Smoker = rawUptake.Sum(x=> x.Smoker),
                BMI25To30 = rawUptake.Sum(x=> x.BMI25To30),
                BMI30Plus = rawUptake.Sum(x=> x.BMI30Plus),
                HighCholesterol = rawUptake.Sum(x=> x.HighCholesterol),
                HighBloodPressure = rawUptake.Sum(x=> x.HighBloodPressure),
                MediumBloodSugar = rawUptake.Sum(x=> x.MediumBloodSugar),
                HighBloodSugar = rawUptake.Sum(x=> x.HighBloodSugar),
                EligibleForGPFollowUp = rawUptake.Sum(x=> x.EligibleForGPFollowUp),
                HasAnyHighRisk = rawUptake.Sum(x=> x.HighRiskBundle)
            };
        }

        private sealed class InterventionEligibilityMap : ClassMap<InterventionEligibilityRecord>
        {
            public InterventionEligibilityMap()
            {
                Map(x => x.Cohort);
                Map(x => x.HighCVD);
                Map(x => x.HighDiabetes);
                Map(x => x.PhysicalActivity0);
                Map(x => x.PhysicalActivity1);
                Map(x => x.PhysicalActivity2);
                Map(x => x.HighMentalHealthRisk);
                Map(x => x.MediumMentalHealthRisk);
                Map(x => x.HighAlcoholRisk);
                Map(x => x.Smoker);
                Map(x => x.BMI25To30);
                Map(x => x.BMI30Plus);
                Map(x => x.HighCholesterol);
                Map(x => x.HighBloodPressure);
                Map(x => x.MediumBloodSugar);
                Map(x => x.HighBloodSugar);
                Map(x => x.EligibleForGPFollowUp);
                Map(x=> x.HasAnyHighRisk);
            }

        }
    }
}
