using NUnit.Framework;
using System;
using Moq;
using QMSUK.DigitalHealthCheck.Encryption;
using Shouldly;

namespace DigitalHealthCheckCommon.Tests
{
    [TestFixture()]
    public class HealthCheckCredentialsEncrypterTests
    {
        readonly Mock<IEncrypter> encrypter = new();
        readonly Mock<IJsonSerializer<Credentials>> serialiser = new();

        HealthCheckCredentialsEncrypter CreateTarget() => new(encrypter.Object, serialiser.Object);


        [Test()]
        public void Constructor_WhenEncrypterIsNull_Throws() =>
            Should.Throw<ArgumentNullException>
            (
                () => new HealthCheckCredentialsEncrypter(null, serialiser.Object)
            );

        [Test()]
        public void Constructor_WhenSerialiserIsNull_Throws() =>
            Should.Throw<ArgumentNullException>
            (
                () => new HealthCheckCredentialsEncrypter(encrypter.Object, null)
            );

        [Test()]
        public void Encrypt_DecryptsAndDeserialises_Input()
        {
            var credentials = new Credentials();

            encrypter.Setup(x => x.Encrypt("bar")).Returns("foo");
            serialiser.Setup(x => x.Serialize(credentials)).Returns("bar");

            var target = CreateTarget();

            target.Encrypt(credentials).ShouldBe("foo");
        }

        [Test()]
        public void Encrypt_WhenCredentialsIsNull_Throws()
        {
            var target = CreateTarget();

            Should.Throw<ArgumentNullException>(() => target.Encrypt(null));
        }
    }
}