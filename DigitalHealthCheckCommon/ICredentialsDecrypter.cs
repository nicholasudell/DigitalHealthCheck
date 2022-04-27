namespace DigitalHealthCheckCommon
{
    public interface ICredentialsDecrypter
    {
        Credentials Decrypt(string encryptedCredentials);
    }
}