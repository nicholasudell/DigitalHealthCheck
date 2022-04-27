using System;
using Bunit;
using DigitalHealthCheckWeb.Components.Common;
using NUnit.Framework;

namespace BlazorComponentTests
{
    public class AntiForgeryTests : BunitTestContext
    {
        [Test]
        public void AntiForgery_WithToken()
        {
            try
            {
                var component = TestContext.RenderComponent<AntiForgery>(
                    parameters => parameters.Add(x => x.AntiForgeryToken, "foo")
                );

                var expectedHtml = "<input type='hidden' name='__RequestVerificationToken' value='foo'>";

                component.MarkupMatches(expectedHtml, $"Component verification failed.");
            }
            catch (Exception e) when (e is not Bunit.HtmlEqualException)
            {
                throw new Exception($"Component verification failed.", e);
            }
        }
    }
}