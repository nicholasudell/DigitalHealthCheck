using System;
using System.Linq;
using System.Text;
using DigitalHealthCheckWeb.Helpers;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using Shouldly;
using SessionExtensions = DigitalHealthCheckWeb.Helpers.SessionExtensions;

namespace DigitalHealthCheckWeb.Tests.Helpers
{
    [TestFixture]
    public class SessionExtensionsTests
    {
        readonly Mock<ISession> session = new();

        void SessionHasData(string key, string data)
        {
            var expected = Encoding.ASCII.GetBytes(data);
            session.Setup(x => x.TryGetValue(key, out expected)).Returns(true);
        }

        [Test]
        public void GetBool_WhenSessionIsNull_ThrowsException()
        {
            ISession session = null;

            Should.Throw<ArgumentNullException>(() => session.GetBool("foo"));
        }

        [Test]
        public void GetBool_WhenValueIsNotBool_ReturnsNull()
        {
            SessionHasData("foo","foo");

            var actual = SessionExtensions.GetBool(session.Object,"foo");

            actual.ShouldBe(null);
        }

        [Test]
        public void GetBool_WhenValueIsTrue_ReturnsTrue()
        {
            SessionHasData("foo", "true");

            var actual = SessionExtensions.GetBool(session.Object, "foo");

            actual.ShouldBe(true);
        }

        [Test]
        public void GetBool_WhenValueIsFalse_ReturnsFalse()
        {
            SessionHasData("foo", "false");

            var actual = SessionExtensions.GetBool(session.Object, "foo");

            actual.ShouldBe(false);
        }

        [Test]
        public void GetDateTime_WhenSessionIsNull_ThrowsException()
        {
            ISession session = null;

            Should.Throw<ArgumentNullException>(() => session.GetDateTime("foo"));
        }

        [Test]
        public void GetDateTime_WhenValueIsNotDateTime_ReturnsNull()
        {
            SessionHasData("foo", "foo");

            var actual = SessionExtensions.GetDateTime(session.Object, "foo");

            actual.ShouldBe(null);
        }

        [Test]
        public void GetDateTime_WhenValueIsDateTime_ReturnsDateTime()
        {
            var expected = DateTime.MinValue;

            SessionHasData("foo", expected.ToString());

            var actual = SessionExtensions.GetDateTime(session.Object, "foo");

            actual.ShouldBe(expected);
        }

        private enum TestEnum
        {
            Value1,
            Value2
        }

        [Test]
        public void GetEnum_WhenSessionIsNull_ThrowsException()
        {
            ISession session = null;

            Should.Throw<ArgumentNullException>(() => session.GetEnum<TestEnum>("foo"));
        }

        [Test]
        public void GetEnum_WhenValueIsNotEnum_ReturnsNull()
        {
            SessionHasData("foo", "foo");

            var actual = SessionExtensions.GetEnum<TestEnum>(session.Object, "foo");

            actual.ShouldBe(null);
        }

        [Test]
        public void GetEnum_WhenValueIsTrue_ReturnsTrue()
        {
            var expected = TestEnum.Value1;

            SessionHasData("foo", expected.ToString());

            var actual = SessionExtensions.GetEnum<TestEnum>(session.Object, "foo");

            actual.ShouldBe(expected);
        }

    }
}
