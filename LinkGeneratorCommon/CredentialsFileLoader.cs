using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace LinkGeneratorCommon
{


    public class CredentialsFileLoader
    {
        public IEnumerable<CredentialsFileRecord> LoadCredentials(string filename)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
            };

            using var reader = new StreamReader(filename);
            using var csv = new CsvReader(reader, config);

            csv.Context.RegisterClassMap<CredentialsMap>();

            return csv.GetRecords<CredentialsFileRecord>().ToList();
        }
    }
}
