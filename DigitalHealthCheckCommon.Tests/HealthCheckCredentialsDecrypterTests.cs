using NUnit.Framework;
using System;
using Moq;
using QMSUK.DigitalHealthCheck.Encryption;
using Shouldly;

namespace DigitalHealthCheckCommon.Tests
{

    [TestFixture()]
    public class HealthCheckCredentialsDecrypterTests
    {
        readonly Mock<IDecrypter> decrypter = new();
        readonly Mock<IJsonDeserializer<Credentials>> deserialiser = new();

        HealthCheckCredentialsDecrypter CreateTarget() => new(decrypter.Object,deserialiser.Object);

        [Test()]
        public void Constructor_WhenDecrypterIsNull_Throws() => 
            Should.Throw<ArgumentNullException>
            (
                () => new HealthCheckCredentialsDecrypter(null, deserialiser.Object)
            );

        [Test()]
        public void Constructor_WhenDeserialiserIsNull_Throws() =>
            Should.Throw<ArgumentNullException>
            (
                () => new HealthCheckCredentialsDecrypter(decrypter.Object, null)
            );

        [Test()]
        public void Decrypt_DecryptsAndDeserialises_Input()
        {
            var expected = new Credentials();

            decrypter.Setup(x => x.Decrypt("foo")).Returns("bar");
            deserialiser.Setup(x => x.Deserialize("bar")).Returns(expected);

            var target = CreateTarget();

            target.Decrypt("foo").ShouldBe(expected);
        }

        [Test()]
        public void Decrypt_WhenEncryptedStringIsNull_Throws()
        {
            var target = CreateTarget();

            Should.Throw<ArgumentException>(() => target.Decrypt(null));
        }

        [Test()]
        public void Decrypt_WhenEncryptedStringIsEmpty_Throws()
        {
            var target = CreateTarget();

            Should.Throw<ArgumentException>(() => target.Decrypt(string.Empty));
        }
    }
}