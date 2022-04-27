using Bunit;
using NUnit.Framework;

namespace BlazorComponentTests
{
    public abstract class BunitTestContext : TestContextWrapper
    {
        [SetUp]
        public void Setup() => TestContext = new Bunit.TestContext();

        [TearDown]
        public void TearDown() => TestContext?.Dispose();
    }
}