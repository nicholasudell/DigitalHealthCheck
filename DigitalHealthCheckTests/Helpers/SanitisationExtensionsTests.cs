using DigitalHealthCheckWeb.Helpers;
using NUnit.Framework;
using Shouldly;

namespace DigitalHealthCheckWeb.Tests.Helpers
{
    [TestFixture]
    public class SanitisationExtensionsTests
    {
        private enum TestEnum
        {
            Value1,
            Value2
        }

        [Test]
        public void AsYesNoOrNull_WhenValueIsNull_ReturnsNull() => 
            ((bool?)null).AsYesNoOrNull().ShouldBe(null);

        [Test]
        public void AsYesNoOrNull_WhenValueIsTrue_ReturnsYes() =>
            ((bool?)true).AsYesNoOrNull().ShouldBe("yes");

        [Test]
        public void AsYesNoOrNull_WhenValueIsFalse_ReturnsNo() =>
            ((bool?)false).AsYesNoOrNull().ShouldBe("no");

        static void TestSanitiseEnum(string value, bool expectedResult, TestEnum? expectedSanitisedValue)
        {
            var actual = value.SanitiseEnum<TestEnum>(out var sanitisedValue);

            actual.ShouldBe(expectedResult);

            sanitisedValue.ShouldBe(expectedSanitisedValue);
        }

        [Test]
        public void SanitiseEnum_WhenValueIsNull_ReturnsFalse_AndSetsSanitisedValueToNull() =>
            TestSanitiseEnum(null, false, null);

        [Test]
        public void SanitiseEnum_WhenValueIsEmptyString_ReturnsFalse_AndSetsSanitisedValueToNull() =>
            TestSanitiseEnum(string.Empty, false, null);

        [Test]
        public void SanitiseEnum_WhenValueIsNotEnumValue_ReturnsFalse_AndSetsSanitisedValueToNull() =>
            TestSanitiseEnum("foo", false, null);

        [Test]
        public void SanitiseEnum_WhenValueIsValid_ReturnsTrue_AndSetsSanitisedValueToValue() =>
            TestSanitiseEnum(TestEnum.Value1.ToString(), true, TestEnum.Value1);

        static void TestSanitiseYesNo(string value, bool expectedResult, bool expectedSanitisedValue)
        {
            var actual = value.SanitiseYesNo(out var sanitisedValue);

            actual.ShouldBe(expectedResult);

            sanitisedValue.ShouldBe(expectedSanitisedValue);
        }

        [Test]
        public void SanitiseYesNo_WhenValueIsNull_ReturnsFalse_AndSetsSanitisedValueToFalse() =>
            TestSanitiseYesNo(null, false, false);

        [Test]
        public void SanitiseYesNo_WhenValueIsEmptyString_ReturnsFalse_AndSetsSanitisedValueToFalse() =>
            TestSanitiseYesNo(string.Empty, false, false);

        [Test]
        public void SanitiseYesNo_WhenValueIsNotYesOrNo_ReturnsFalse_AndSetsSanitisedValueToFalse() =>
            TestSanitiseYesNo("foo", false, false);

        [Test]
        public void SanitiseYesNo_WhenValueIsNo_ReturnsTrue_AndSetsSanitisedValueToFalse() =>
            TestSanitiseYesNo("no", true, false);

        [Test]
        public void SanitiseYesNo_WhenValueIsYes_ReturnsTrue_AndSetsSanitisedValueToTrue() =>
            TestSanitiseYesNo("yes", true, true);
    }
}
