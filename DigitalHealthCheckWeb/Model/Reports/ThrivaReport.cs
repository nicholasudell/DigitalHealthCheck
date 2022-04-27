using System;
using System.Linq;
using System.Threading.Tasks;
using DigitalHealthCheckEF;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using CsvHelper.Configuration;
using System.Collections.Generic;

namespace DigitalHealthCheckWeb.Model
{
    public class ThrivaReport : Report<ThrivaReport.ThrivaRecord>
    {
        public class ThrivaRecord
        {
            public ThrivaRecord(string firstName, string surname, string email)
            {
                FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
                Surname = surname ?? throw new ArgumentNullException(nameof(surname));
                Email = email ?? throw new ArgumentNullException(nameof(email));
            }

            public string FirstName { get; set; }
            public string Surname { get; set; }
            public string Email { get; set; }

            public string Sex { get; set;  } //Intentionally left blank
            public string Dob { get; set; } //Intentionally left blank
        }

        private readonly Database database;
        private readonly DateTime? from;
        private readonly DateTime? to;

        public ThrivaReport(Database database, DateTime? from = null, DateTime? to = null) : base
        (
            new CsvConfiguration(CultureInfo.CurrentCulture), 
            csv =>
            {
                csv.Context.RegisterClassMap<ThrivaMap>();
            }
        )
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
            this.from = from;
            this.to = to;
        }

        protected override async Task<IList<ThrivaRecord>> GetRecordsAsync()
        {
            var fromNormalised = (from ?? DateTime.MinValue).Date;
            var toNormalised = (to ?? DateTime.MaxValue).Date;

            return await database.BloodKitRequests
                .Include(nameof(BloodKitRequest.Check))
                .Where(x => x.DateRequested >= fromNormalised && x.DateRequested < toNormalised)
                .Select(x => new ThrivaRecord(x.Check.FirstName, x.Check.Surname, x.Email))
                .ToListAsync();
        }

        private sealed class ThrivaMap : ClassMap<ThrivaRecord>
        {
            public ThrivaMap()
            {
                Map(x => x.Email).Name("email").Index(1);
                Map(x => x.FirstName).Name("first_name").Index(2);
                Map(x => x.Surname).Name("last_name").Index(3);
                
                //Empty fields intentionally.
                Map(x => x.Sex).Name("sex").Index(4).Constant(string.Empty); 
                Map(x => x.Dob).Name("dob").Index(5).Constant(string.Empty);
            }

        }
    }
}
