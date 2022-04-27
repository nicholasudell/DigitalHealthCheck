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
    public class BiometricAssessmentSuccessReport : Report<BiometricAssessmentSuccessReport.BiometricAssessmentSuccessRecord>
    {
        public class BiometricAssessmentSuccessRecord
        {
            public string Cohort { get; set; }
            public int UnknownBP {get;set;}
            public int UnknownCholesterol {get;set;}
            public int UnknownHeightAndWeight {get;set;}
            public int UnknownBloodSugar {get;set;}
            public int HasAllBiometrics {get;set;}
            public int BloodPressureUpdated {get;set;}
            public int CholesterolUpdated {get;set;}
            public int BloodSugarUpdated {get;set;}
            public int HeightAndWeightUpdated {get;set;}
            public int NeedsAnyBiometric {get;set;}
            public int GotAtLeastOneBiometric {get;set;}
        }

        private readonly Database database;
        private readonly DateTime? from;
        private readonly DateTime? to;

        public BiometricAssessmentSuccessReport(Database database, DateTime? from = null, DateTime? to = null) : base
        (
            new CsvConfiguration(CultureInfo.CurrentCulture),
            csv =>
            {
                csv.Context.RegisterClassMap<BiometricAssessmentSuccessMap>();
            }
        )
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
            this.from = from;
            this.to = to;
        }

        protected override async Task<IList<BiometricAssessmentSuccessRecord>> GetRecordsAsync()
        {
            database.Database.SetCommandTimeout(TimeSpan.FromMinutes(10));

            var rawUptake = await database.BiometricAssessmentSuccessReport(from, to).ToListAsync();

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

            return new List<BiometricAssessmentSuccessRecord>()
            {
                CreateRecord(rawUptake, "Full Group"),
                CreateRecord(rawUptake.Where(x=> x.SexForResults == Sex.Male), "Men Only"),
                CreateRecord(rawUptake.Where(x=> x.Age >= 60), "Aged 60+"),
                CreateRecord(rawUptake.Where(x=> x.Ethnicity.HasValue &&  blackAsianOrMixedEthnicities.Contains(x.Ethnicity.Value) ), "Black Asian or Mixed Ethnicity"),
                CreateRecord(rawUptake.Where(x=> !string.IsNullOrEmpty(x.Postcode) && x.IMDQuintile != null && x.IMDQuintile <=2 ), "Deprivation Lowest 2 Quintiles"),
                CreateRecord(rawUptake.Where(x=> x.SmokingStatus == SmokingStatus.Light || x.SmokingStatus == SmokingStatus.Moderate || x.SmokingStatus == SmokingStatus.Heavy), "Smokers"),
            };
        }

        BiometricAssessmentSuccessRecord CreateRecord(IEnumerable<DigitalHealthCheckEF.BiometricAssessmentSuccessReport> rawUptake, string cohort)
        {
            return new BiometricAssessmentSuccessRecord
            {
                Cohort = cohort,
                UnknownBP = rawUptake.Sum(x=> x.UnknownBP),
                UnknownCholesterol = rawUptake.Sum(x=> x.UnknownCholesterol),
                UnknownHeightAndWeight = rawUptake.Sum(x=> x.UnknownHeightAndWeight),
                UnknownBloodSugar = rawUptake.Sum(x=> x.UnknownBloodSugar),
                HasAllBiometrics = rawUptake.Sum(x=> x.HasAllBiometrics),
                BloodPressureUpdated = rawUptake.Sum(x=> x.BloodPressureUpdated),
                CholesterolUpdated = rawUptake.Sum(x=> x.CholesterolUpdated),
                BloodSugarUpdated = rawUptake.Sum(x=> x.BloodSugarUpdated),
                HeightAndWeightUpdated = rawUptake.Sum(x=> x.HeightAndWeightUpdated),
                NeedsAnyBiometric = rawUptake.Sum(x=> x.NeedsAnyBiometric),
                GotAtLeastOneBiometric = rawUptake.Sum(x=> x.GotAtLeastOneBiometric)
            };
        }

        private sealed class BiometricAssessmentSuccessMap : ClassMap<BiometricAssessmentSuccessRecord>
        {
            public BiometricAssessmentSuccessMap()
            {
                Map(x => x.Cohort);
                Map(x => x.UnknownBP);
                Map(x => x.UnknownCholesterol);
                Map(x => x.UnknownHeightAndWeight);
                Map(x => x.UnknownBloodSugar);
                Map(x => x.HasAllBiometrics);
                Map(x => x.BloodPressureUpdated);
                Map(x => x.CholesterolUpdated);
                Map(x => x.BloodSugarUpdated);
                Map(x => x.HeightAndWeightUpdated);
                Map(x=> x.NeedsAnyBiometric);
                Map(x=> x.GotAtLeastOneBiometric);
            }
        }
    }
}
