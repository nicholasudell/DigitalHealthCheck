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

    public class UptakeRateReport : Report<UptakeRateReport.UptakeRecord>
    {
        public class UptakeRecord
        {
            public string Cohort { get; set; }
            public int Invited { get; set; }
            public int OpenedLink { get; set; }
            public int AnsweredAQuestion { get; set; }
            public int Validation { get; set; }
            public int Results { get; set; }
            public int AtLeast1FollowUp { get; set; }
            public int AtLeast1Intervention { get; set; }
            public int HealthCheckCompleted { get; set; }
            public int UpdatedMeasurements { get; set; }
        }

        private readonly Database database;
        private readonly DateTime? from;
        private readonly DateTime? to;

        public UptakeRateReport(Database database, DateTime? from = null, DateTime? to = null) : base
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

            var rawUptake = await database.UptakeRateReport(from, to).ToListAsync();

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

        UptakeRecord CreateUptakeRecord(IEnumerable<DigitalHealthCheckEF.UptakeRateReport> rawUptake, string cohort)
        {
            return new UptakeRecord
            {
                Cohort = cohort,
                Invited = rawUptake.Sum(x => x.Invited),
                OpenedLink = rawUptake.Sum(x => x.OpenedLink),
                AnsweredAQuestion = rawUptake.Sum(x => x.AnsweredAQuestion),
                Validation = rawUptake.Sum(x => x.Validation),
                Results = rawUptake.Sum(x => x.Results),
                AtLeast1FollowUp = rawUptake.Sum(x => x.AtLeast1FollowUp),
                AtLeast1Intervention = rawUptake.Sum(x => x.AtLeast1Intervention),
                HealthCheckCompleted = rawUptake.Sum(x => x.HealthCheckCompleted),
                UpdatedMeasurements = rawUptake.Sum(x => x.UpdatedMeasurements)
            };
        }

        private sealed class UptakeRecordMap : ClassMap<UptakeRecord>
        {
            public UptakeRecordMap()
            {
                Map(x => x.Cohort);
                Map(x => x.Invited);
                Map(x=> x.OpenedLink);
                Map(x=> x.AnsweredAQuestion);
                Map(x=> x.Validation);
                Map(x=> x.Results);
                Map(x=> x.AtLeast1FollowUp);
                Map(x=> x.AtLeast1Intervention);
                Map(x=> x.HealthCheckCompleted);
                Map(x=> x.UpdatedMeasurements);
            }

        }
    }
}
