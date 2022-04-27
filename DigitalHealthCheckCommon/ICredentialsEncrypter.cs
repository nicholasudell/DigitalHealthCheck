namespace DigitalHealthCheckCommon
{
    public interface ICredentialsEncrypter
    {
        string Encrypt(Credentials credentials);
    }
}