using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace DigitalHealthCheckWeb.Model
{
    public abstract class Report
    {
        /// <summary>
        /// Generates an in-memory byte string containing the report data.
        /// </summary>
        /// <returns>An awaitable task that, once awaited, includes an array of bytes containing the report data.</returns>
        public abstract Task<byte[]> Generate();
    }

    public abstract class Report<TRecord> : Report
    {
        private readonly CsvConfiguration configuration;
        private readonly Action<CsvWriter> configureCsv;

        protected abstract Task<IList<TRecord>> GetRecordsAsync();

        public Report(CsvConfiguration configuration, Action<CsvWriter> configureCsv = null)
        {
            this.configuration = configuration;
            this.configureCsv = configureCsv;
        }

        /// <summary>
        /// Generates an in-memory byte string containing the report data.
        /// </summary>
        /// <returns>An awaitable task that, once awaited, includes an array of bytes containing the report data.</returns>
        public async override Task<byte[]> Generate()
        {
            var records = await GetRecordsAsync();
            return await CreateCsvFileInMemory(records, configuration, configureCsv);
        }

        static async Task<byte[]> CreateCsvFileInMemory(IEnumerable<TRecord> records, CsvConfiguration configuration, Action<CsvWriter> configureCsv = null)
        {
            using var stream = new MemoryStream();

            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, configuration))
            {
                configureCsv?.Invoke(csv);

                await csv.WriteRecordsAsync(records);
            }

            return stream.ToArray();
        }
    }
}
