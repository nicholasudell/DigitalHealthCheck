using CsvHelper.Configuration;

namespace LinkGeneratorCommon
{
    public sealed class CredentialsMap : ClassMap<CredentialsFileRecord>
    {
        public CredentialsMap()
        {
            Map(m => m.Surname);
            Map(m => m.Postcode);
            Map(m => m.NHSNumber);
            Map(m => m.DateOfBirth).TypeConverterOption.Format("dd/MM/yyyy");
            Map(m => m.GPEmail);
            Map(m => m.GPSurgery);
            Map(m => m.GPODS);
        }
    }
}
