using System;
using System.Collections.Generic;
using System.Linq;
using DigitalHealthCheckWeb.Helpers;
using NUnit.Framework;
using Shouldly;

namespace DigitalHealthCheckWeb.Tests.Helpers
{
    [TestFixture]
    public class IEnumerableExtensionsTests
    {
        [Test]
        public void NextInSequence_WhenSourceIsNull_ThrowsException() =>
            Should.Throw<ArgumentNullException>(() => ((IEnumerable<int>)null).NextInSequence(x=> true));

        [Test]
        public void NextInSequence_WhenSearchIsNull_ThrowsException() =>
            Should.Throw<ArgumentNullException>(() => (Enumerable.Range(1,5)).NextInSequence(null));

        [Test]
        public void NextInSequence_ReturnsNextItem()
        {
            var expected = 3;

            var source = Enumerable.Range(1, 5);

            var actual = source.NextInSequence(x => x == 2);

            actual.ShouldBe(expected);
        }

        [Test]
        public void NextInSequence_WhenSearchNeverReturnsTrue_ReturnsDefault()
        {
            var expected = 0;

            var source = Enumerable.Range(1, 5);

            var actual = source.NextInSequence(x => false);

            actual.ShouldBe(expected);
        }

        [Test]
        public void NextInSequence_WhenSearchNeverReturnsTrue_AndDefaultIsSpecified_ReturnsDefault()
        {
            var expected = 10;

            var source = Enumerable.Range(1, 5);

            var actual = source.NextInSequence(x => false, 10);

            actual.ShouldBe(expected);
        }

        [Test]
        public void NextInSequence_DoesNotEvaluateFollowingItems()
        {
            var expected = 3;

            IEnumerable<int> Source()
            {
                yield return 1;
                yield return 2;
                yield return 3;
                throw new InvalidOperationException("Tried to execute past item 3");
#pragma warning disable CS0162 // Unreachable code detected. Justification: This is on purpose as part of the test.
                yield return 4;
                yield return 5;
#pragma warning restore CS0162 // Unreachable code detected
            }

            var source = Source();

            var actual = source.NextInSequence(x => x == 2);

            actual.ShouldBe(expected);
        }

        [Test]
        public void PreviousInSequence_WhenSourceIsNull_ThrowsException() =>
            Should.Throw<ArgumentNullException>(() => ((IEnumerable<int>)null).PreviousInSequence(x => true));

        [Test]
        public void PreviousInSequence_WhenSearchIsNull_ThrowsException() =>
            Should.Throw<ArgumentNullException>(() => (Enumerable.Range(1, 5)).PreviousInSequence(null));

        [Test]
        public void PreviousInSequence_ReturnsPreviousItem()
        {
            var expected = 2;

            var source = Enumerable.Range(1, 5);

            var actual = source.PreviousInSequence(x => x == 3);

            actual.ShouldBe(expected);
        }

        [Test]
        public void PreviousInSequence_WhenSearchNeverReturnsTrue_ReturnsDefault()
        {
            var expected = 0;

            var source = Enumerable.Range(1, 5);

            var actual = source.PreviousInSequence(x => false);

            actual.ShouldBe(expected);
        }

        [Test]
        public void PreviousInSequence_WhenSearchNeverReturnsTrue_AndDefaultIsSpecified_ReturnsDefault()
        {
            var expected = 10;

            var source = Enumerable.Range(1, 5);

            var actual = source.PreviousInSequence(x => false, 10);

            actual.ShouldBe(expected);
        }

        [Test]
        public void PreviousInSequence_DoesNotEvaluateFollowingItems()
        {
            var expected = 2;

            IEnumerable<int> Source()
            {
                yield return 1;
                yield return 2;
                yield return 3;
                throw new InvalidOperationException("Tried to execute past item 3");
#pragma warning disable CS0162 // Unreachable code detected. Justification: This is on purpose as part of the test.
                yield return 4;
                yield return 5;
#pragma warning restore CS0162 // Unreachable code detected
            }

            var source = Source();

            var actual = source.PreviousInSequence(x => x == 3);

            actual.ShouldBe(expected);
        }
    }
}
