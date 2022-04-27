using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.EntityFrameworkCore;

public class AnonymisedInterventionsReport : Report<AnonymisedIntervention>
{
    private readonly Database database;
    private readonly DateTime? from;
    private readonly DateTime? to;

    public AnonymisedInterventionsReport(Database database, DateTime? from = null, DateTime? to = null) : base
    (
        new CsvConfiguration(CultureInfo.CurrentCulture), 
        csv =>
        {
            csv.Context.RegisterClassMap<AnonymisedInterventionsMap>();
        }
    )
    {
        this.database = database ?? throw new ArgumentNullException(nameof(database));
        this.from = from;
        this.to = to;
    }

    protected override async Task<IList<AnonymisedIntervention>> GetRecordsAsync() => 
        await database.AnonymisedInterventions(from, to).ToListAsync();

    private sealed class AnonymisedInterventionsMap : ClassMap<AnonymisedIntervention>
    {
        public AnonymisedInterventionsMap()
        {
            Map(x=>x.PatientIdentifier);
            Map(x=>x.Category);
            Map(x=>x.Barrier);
            Map(x=>x.Intervention);
            Map(x=>x.Url);
        }

    }
}