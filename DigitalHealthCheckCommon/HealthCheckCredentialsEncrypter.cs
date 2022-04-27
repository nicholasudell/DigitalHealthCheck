using System;
using QMSUK.DigitalHealthCheck.Encryption;

namespace DigitalHealthCheckCommon
{
    public class HealthCheckCredentialsEncrypter : ICredentialsEncrypter
    {
        private readonly IJsonSerializer<Credentials> credentialSerializer;
        private readonly IEncrypter encrypter;

        public HealthCheckCredentialsEncrypter(IEncrypter encrypter, IJsonSerializer<Credentials> credentialSerializer)
        {
            this.encrypter = encrypter ?? throw new ArgumentNullException(nameof(encrypter));
            this.credentialSerializer = credentialSerializer ?? throw new ArgumentNullException(nameof(credentialSerializer));
        }

        public string Encrypt(Credentials credentials)
        {
            if (credentials is null)
            {
                throw new ArgumentNullException(nameof(credentials));
            }

            return encrypter.Encrypt(credentialSerializer.Serialize(credentials));
        }
    }
}