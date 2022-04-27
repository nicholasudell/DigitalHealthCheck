using System;
using QMSUK.DigitalHealthCheck.Encryption;

namespace DigitalHealthCheckCommon
{
    public class HealthCheckCredentialsDecrypter : ICredentialsDecrypter
    {
        private readonly IJsonDeserializer<Credentials> credentialDeserializer;
        private readonly IDecrypter decrypter;

        public HealthCheckCredentialsDecrypter(IDecrypter decrypter, IJsonDeserializer<Credentials> credentialDeserializer)
        {
            this.decrypter = decrypter ?? throw new ArgumentNullException(nameof(decrypter));
            this.credentialDeserializer = credentialDeserializer ?? throw new ArgumentNullException(nameof(credentialDeserializer));
        }

        public Credentials Decrypt(string encryptedCredentials)
        {
            if (string.IsNullOrEmpty(encryptedCredentials))
            {
                throw new ArgumentException($"'{nameof(encryptedCredentials)}' cannot be null or empty.", nameof(encryptedCredentials));
            }

            return credentialDeserializer.Deserialize(decrypter.Decrypt(encryptedCredentials));
        }
    }
}