using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;

namespace QRiskEstimator
{
    public class PatientsFileLoader
    {
        public IEnumerable<OutputFileRecord> LoadPatients(string filename)
        {
            using var reader = new StreamReader(filename);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Context.RegisterClassMap<InputClassMap>();

            return csv.GetRecords<OutputFileRecord>().ToList();
        }
    }
}
