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
    public class DetailedUptakeReport : Report<DetailedUptakeReport.UptakeRecord>
    {
        public class UptakeRecord
        {
            public string Cohort { get; set; }
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

        private readonly Database database;
        private readonly DateTime? from;
        private readonly DateTime? to;

        public DetailedUptakeReport(Database database, DateTime? from = null, DateTime? to = null) : base
        (
            new CsvConfiguration(CultureInfo.CurrentCulture),
            csv =>
            {
                csv.Context.RegisterClassMap<UptakeRecordMap>();
            }
        )
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
            this.from = from;
            this.to = to;
        }

        protected override async Task<IList<UptakeRecord>> GetRecordsAsync()
        {
            database.Database.SetCommandTimeout(TimeSpan.FromMinutes(10));

            var rawUptake = await database.DetailedUptakeReport(from, to).ToListAsync();

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

            return new List<UptakeRecord>()
            {
                CreateUptakeRecord(rawUptake, "Full Group"),
                CreateUptakeRecord(rawUptake.Where(x=> x.SexForResults == Sex.Male), "Men Only"),
                CreateUptakeRecord(rawUptake.Where(x=> x.Age >= 60), "Aged 60+"),
                CreateUptakeRecord(rawUptake.Where(x=> x.Ethnicity.HasValue &&  blackAsianOrMixedEthnicities.Contains(x.Ethnicity.Value) ), "Black Asian or Mixed Ethnicity"),
                CreateUptakeRecord(rawUptake.Where(x=> !string.IsNullOrEmpty(x.Postcode) && x.IMDQuintile != null && x.IMDQuintile <=2 ), "Deprivation Lowest 2 Quintiles"),
                CreateUptakeRecord(rawUptake.Where(x=> x.SmokingStatus == SmokingStatus.Light || x.SmokingStatus == SmokingStatus.Moderate || x.SmokingStatus == SmokingStatus.Heavy), "Smokers"),
            };
        }

        UptakeRecord CreateUptakeRecord(IEnumerable<DigitalHealthCheckEF.DetailedUptakeReport> rawUptake, string cohort)
        {
            return new UptakeRecord
            {
                Cohort = cohort,
                OpenedLink = rawUptake.Sum(x=> x.OpenedLink),
                ClickedStartNow = rawUptake.Sum(x=> x.ClickedStartNow),
                HeightAndWeight = rawUptake.Sum(x=> x.HeightAndWeight),
                Sex = rawUptake.Sum(x=> x.Sex),
                EthnicitySubmitted = rawUptake.Sum(x=> x.EthnicitySubmitted),
                Smoking = rawUptake.Sum(x=> x.Smoking),
                DrinksAlcohol = rawUptake.Sum(x=> x.DrinksAlcohol),
                PhysicalActivityStarted = rawUptake.Sum(x=> x.PhysicalActivityStarted),
                PhysicalActivityFinished = rawUptake.Sum(x=> x.PhysicalActivityFinished),
                BloodSugar = rawUptake.Sum(x=> x.BloodSugar),
                RiskFactors1 = rawUptake.Sum(x=> x.RiskFactors1),
                RiskFactors2 = rawUptake.Sum(x=> x.RiskFactors2),
                RiskFactors3 = rawUptake.Sum(x=> x.RiskFactors3),
                BloodPressure = rawUptake.Sum(x=> x.BloodPressure),
                Cholesterol = rawUptake.Sum(x=> x.Cholesterol),
                Validation = rawUptake.Sum(x=> x.Validation),
                SuccessfulValidation = rawUptake.Sum(x=> x.SuccessfulValidation),
                SkippedValidation = rawUptake.Sum(x=> x.SkippedValidation),
                CheckYourAnswers = rawUptake.Sum(x=> x.CheckYourAnswers),
                CheckYourAnswersWithHeightAndWeight = rawUptake.Sum(x=> x.CheckYourAnswersWithHeightAndWeight),
                CheckYourAnswersWithoutHeightAndWeight = rawUptake.Sum(x=> x.CheckYourAnswersWithoutHeightAndWeight),
                FollowUpHeightAndWeight = rawUptake.Sum(x=> x.FollowUpHeightAndWeight),
                GeneratedResults = rawUptake.Sum(x=> x.GeneratedResults),
                SubmittedHealthPrioritiesAfterResults = rawUptake.Sum(x=> x.SubmittedHealthPrioritiesAfterResults),
                BookGPAppointmentFollowUp = rawUptake.Sum(x=> x.BookGPAppointmentFollowUp),
                TotalFollowedUpOnFirstPriority = rawUptake.Sum(x=> x.TotalFollowedUpOnFirstPriority),
                FollowedUpOnFirstPriorityNoGPFollowUp = rawUptake.Sum(x=> x.FollowedUpOnFirstPriorityNoGPFollowUp),
                FollowedUpOnFirstPriorityGPFollowUp = rawUptake.Sum(x=> x.FollowedUpOnFirstPriorityGPFollowUp),
                TotalFollowedUpOnSecondPriority = rawUptake.Sum(x=> x.TotalFollowedUpOnSecondPriority),
                FollowedUpOnSecondPriorityNoGPFollowUp = rawUptake.Sum(x=> x.FollowedUpOnSecondPriorityNoGPFollowUp),
                FollowedUpOnSecondPriorityGPFollowUp = rawUptake.Sum(x=> x.FollowedUpOnSecondPriorityGPFollowUp),
                TotalFollowedUpOnThreeOrMorePriorities = rawUptake.Sum(x=> x.TotalFollowedUpOnThreeOrMorePriorities),
                FollowedUpOnThirdPriorityNoGPFollowUp = rawUptake.Sum(x=> x.FollowedUpOnThirdPriorityNoGPFollowUp),
                FollowedUpOnThirdPriorityGPFollowUp = rawUptake.Sum(x=> x.FollowedUpOnThirdPriorityGPFollowUp),
                AnyBarriers = rawUptake.Sum(x=> x.AnyBarriers),
                AnyInterventions = rawUptake.Sum(x=> x.AnyInterventions),
                Completed = rawUptake.Sum(x=> x.Completed),
            };
        }

        private sealed class UptakeRecordMap : ClassMap<UptakeRecord>
        {
            public UptakeRecordMap()
            {
                Map(x => x.Cohort);
                Map(x=> x.OpenedLink);
                Map(x=> x.ClickedStartNow);
                Map(x=> x.HeightAndWeight);
                Map(x=> x.Sex);
                Map(x=> x.EthnicitySubmitted);
                Map(x=> x.Smoking);
                Map(x=> x.DrinksAlcohol);
                Map(x=> x.PhysicalActivityStarted);
                Map(x=> x.PhysicalActivityFinished);
                Map(x=> x.BloodSugar);
                Map(x=> x.RiskFactors1);
                Map(x=> x.RiskFactors2);
                Map(x=> x.RiskFactors3);
                Map(x=> x.BloodPressure);
                Map(x=> x.Cholesterol);
                Map(x=> x.Validation);
                Map(x=> x.SuccessfulValidation);
                Map(x=> x.SkippedValidation);
                Map(x=> x.CheckYourAnswers);
                Map(x=> x.CheckYourAnswersWithHeightAndWeight);
                Map(x=> x.CheckYourAnswersWithoutHeightAndWeight);
                Map(x=> x.FollowUpHeightAndWeight);
                Map(x=> x.GeneratedResults);
                Map(x=> x.SubmittedHealthPrioritiesAfterResults);
                Map(x=> x.BookGPAppointmentFollowUp);
                Map(x=> x.TotalFollowedUpOnFirstPriority);
                Map(x=> x.FollowedUpOnFirstPriorityNoGPFollowUp);
                Map(x=> x.FollowedUpOnFirstPriorityGPFollowUp);
                Map(x=> x.TotalFollowedUpOnSecondPriority);
                Map(x=> x.FollowedUpOnSecondPriorityNoGPFollowUp);
                Map(x=> x.FollowedUpOnSecondPriorityGPFollowUp);
                Map(x=> x.TotalFollowedUpOnThreeOrMorePriorities);
                Map(x=> x.FollowedUpOnThirdPriorityNoGPFollowUp);
                Map(x=> x.FollowedUpOnThirdPriorityGPFollowUp);
                Map(x=> x.AnyBarriers);
                Map(x=> x.AnyInterventions);
                Map(x=> x.Completed);
            }

        }
    }
}
