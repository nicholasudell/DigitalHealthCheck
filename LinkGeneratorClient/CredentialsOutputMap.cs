using CsvHelper.Configuration;

namespace LinkGeneratorClient
{
    public sealed class CredentialsOutputMap : ClassMap<CredentialsOutputFileRecord>
    {
        public CredentialsOutputMap()
        {
            Map(m => m.Surname);
            Map(m => m.Postcode);
            Map(m => m.NHSNumber);
            Map(m => m.DateOfBirth).TypeConverterOption.Format("dd/MM/yyyy");
            Map(m => m.GPEmail);
            Map(m => m.GPSurgery);
            Map(m => m.GPODS);
            Map(m => m.Url);
        }
    }
}
