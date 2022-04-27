using System;
using System.Threading.Tasks;
using DigitalHealthCheckWeb.Helpers;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace DigitalHealthCheckWeb.Tests.Helpers
{


    [TestFixture]
    public class HttpRequestExtensionsTests
    {
        [Test]
        public void BaseURL_WhenRequestIsNull_ThrowsException() => 
            Should.Throw<ArgumentNullException>(() => ((HttpRequest)null).BaseUrl());

        [Test]
        public void BaseUrl_ReturnsBasePath()
        {
            var expected = "foo://bar/bash";

            var request = Mock.Of<HttpRequest>(x=> x.Scheme == "foo" && x.Host == new HostString("bar") && x.PathBase == new PathString("/bash"));

            var actual = request.BaseUrl();

            actual.ShouldBe(expected);
        }

        [Test]
        public void CurrentPage_WhenRequestIsNull_ThrowsException() =>
            Should.Throw<ArgumentNullException>(() => ((HttpRequest)null).BaseUrl());

        [Test]
        public void CurrentPage_ReturnsCurrentPageWithoutBaseURL()
        {
            var expected = "foo";

            var request = Mock.Of<HttpRequest>(x => x.Path == new PathString("/bar/foo") && x.PathBase == new PathString("/bar/"));

            var actual = request.CurrentPage();

            actual.ShouldBe(expected);
        }

        [Test]
        public void CurrentPage_WhenPathIsNull_ReturnsNull()
        {
            var expected = (string)null;

            var request = Mock.Of<HttpRequest>(x => x.Path == null);

            var actual = request.CurrentPage();

            actual.ShouldBe(expected);
        }

        [Test]
        public void CurrentPage_WhenPathBaseIsNull_ReturnsFullPath()
        {
            var expected = "/foo";

            var request = Mock.Of<HttpRequest>(x => x.Path == new PathString("/foo"));

            var actual = request.CurrentPage();

            actual.ShouldBe(expected);
        }
    }
}
