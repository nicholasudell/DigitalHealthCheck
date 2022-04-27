using DigitalHealthCheckWeb.Helpers;
using NUnit.Framework;
using Shouldly;

namespace DigitalHealthCheckWeb.Tests.Helpers
{
    [TestFixture]
    public class StringExtensionsTests
    {

        [Test]
        public void ToTitleCase_WhenTextIsNull_ReturnsNull() => 
            ((string)null).ToTitleCase().ShouldBe(null);

        [Test]
        public void ToTitleCase_WhenTextIsEmptyString_ReturnsEmptyString() =>
            string.Empty.ToTitleCase().ShouldBe(string.Empty);

        [Test]
        public void ToTitleCase_WhenTextIsOneCharacterLong_ReturnsCharacterCapitalised() =>
            "i".ToTitleCase().ShouldBe("I");

        [Test]
        public void ToTitleCase_WhenTextIsMultipleCharacters_CapitalisesFirstCharacter() =>
            "foo".ToTitleCase().ShouldBe("Foo");

        [Test]
        public void FromTitleCase_WhenTextIsNull_ReturnsNull() =>
           ((string)null).FromTitleCase().ShouldBe(null);

        [Test]
        public void FromTitleCase_WhenTextIsEmptyString_ReturnsEmptyString() =>
            string.Empty.FromTitleCase().ShouldBe(string.Empty);

        [Test]
        public void FromTitleCase_WhenTextIsOneCharacterLong_ReturnsCharacterCapitalised() =>
            "I".FromTitleCase().ShouldBe("i");

        [Test]
        public void FromTitleCase_WhenTextIsMultipleCharacters_LowersFirstCharacter() =>
            "Foo".FromTitleCase().ShouldBe("foo");
    }
}
