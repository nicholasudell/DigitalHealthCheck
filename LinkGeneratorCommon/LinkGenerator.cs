using System;
using DigitalHealthCheckCommon;

namespace LinkGeneratorCommon
{
    public class LinkGenerator
    {
        private readonly ICredentialsEncrypter encrypter;
        private readonly string linkFormat;

        public LinkGenerator(ICredentialsEncrypter encrypter, string linkFormat)
        {
            if (string.IsNullOrWhiteSpace(linkFormat))
            {
                throw new ArgumentException($"'{nameof(linkFormat)}' cannot be null or whitespace.", nameof(linkFormat));
            }

            this.encrypter = encrypter ?? throw new ArgumentNullException(nameof(encrypter));
            this.linkFormat = linkFormat;
        }

        public string GenerateLink(Guid id, Credentials credentials) =>
            string.Format(linkFormat, id, encrypter.Encrypt(credentials));
    }
}
