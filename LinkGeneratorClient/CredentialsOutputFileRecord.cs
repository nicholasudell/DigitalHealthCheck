using LinkGeneratorCommon;

namespace LinkGeneratorClient
{
    public class CredentialsOutputFileRecord : CredentialsFileRecord
    {
        public CredentialsOutputFileRecord(CredentialsFileRecord @base)
        {
            DateOfBirth = @base.DateOfBirth;
            GPEmail = @base.GPEmail;
            GPSurgery = @base.GPSurgery;
            NHSNumber = @base.NHSNumber;
            Postcode = @base.Postcode;
            Surname = @base.Surname;
            Url = string.Empty;
            GPODS = @base.GPODS;
        }

        public string Url { get; set; }
    }
}
