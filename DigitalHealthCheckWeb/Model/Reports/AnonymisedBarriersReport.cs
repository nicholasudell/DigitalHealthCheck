using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.EntityFrameworkCore;

public class AnonymisedBarriersReport : Report<AnonymisedBarrier>
{
    private readonly Database database;
    private readonly DateTime? from;
    private readonly DateTime? to;

    public AnonymisedBarriersReport(Database database, DateTime? from = null, DateTime? to = null) : base
    (
        new CsvConfiguration(CultureInfo.CurrentCulture), 
        csv =>
        {
            csv.Context.RegisterClassMap<AnonymisedBarriersMap>();
        }
    )
    {
        this.database = database ?? throw new ArgumentNullException(nameof(database));
        this.from = from;
        this.from = to;
    }

    protected override async Task<IList<AnonymisedBarrier>> GetRecordsAsync() => 
        await database.AnonymisedBarriers(from, to).ToListAsync();

    private sealed class AnonymisedBarriersMap : ClassMap<AnonymisedBarrier>
    {
        public AnonymisedBarriersMap()
        {
            Map(x=> x.PatientIdentifier);
            Map(x=> x.Category);
            Map(x=> x.Barrier);
        }

    }
}