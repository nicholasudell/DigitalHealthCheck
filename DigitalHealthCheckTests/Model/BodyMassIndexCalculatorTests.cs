using System;
using System.Collections.Generic;
using System.Linq;
using DigitalHealthCheckCommon;
using DigitalHealthCheckWeb.Model;
using NUnit.Framework;
using Shouldly;

namespace DigitalHealthCheckTests.Model
{
    [TestFixture]
    public class BodyMassIndexCalculatorTests
    {
        //There will be some variance when comparing floating point numbers. Anything up to this amount is acceptable.
        const double AcceptableVariance = 0.1d;

        [TestCaseSource(nameof(TestData))]
        public void BMI_ShouldMatchForValues(double height, double weight, double expected)
        {
            var calculator = CreateCalculator();

            var actual = calculator.CalculateBodyMassIndex(height, weight);

            actual.ShouldBe
            (
                expected,
                AcceptableVariance,
                $"Got {actual} instead of {expected} for height {height}m and weight {weight}kg."
            );
        }

        static BodyMassIndexCalculator CreateCalculator() => new();

        static IEnumerable<TestCaseData> TestData()
            => new[]
            {
                new Tuple<double, double, double>(1d,50d,50d),
                new Tuple<double, double, double>(1.25d,50d,32d),
                new Tuple<double, double, double>(1.5d,50d,22.22d),
                new Tuple<double, double, double>(1.75d,50d,16.3d),
                new Tuple<double, double, double>(2d,50d,12.5d),

                new Tuple<double, double, double>(1d,75d,75d),
                new Tuple<double, double, double>(1.25d,75d,48d),
                new Tuple<double, double, double>(1.5d,75d,33.3d),
                new Tuple<double, double, double>(1.75d,75d,24.5d),
                new Tuple<double, double, double>(2d,75d,18.8d),

                new Tuple<double, double, double>(1d,100d,100d),
                new Tuple<double, double, double>(1.25d,100d,64d),
                new Tuple<double, double, double>(1.5d,100d,44.4d),
                new Tuple<double, double, double>(1.75d,100d,32.7d),
                new Tuple<double, double, double>(2d,100d,25d),
            }.Select(x =>
                new TestCaseData(x.Item1, x.Item2, x.Item3)
                    .SetDescription($"BMI should be {x.Item3} for height {x.Item1} and weight {x.Item2}")
                    .SetName($"BMI_ForHeight_{x.Item1.ToString().Replace('.', '_')}_Weight_{x.Item2.ToString().Replace('.', '_')}")
            ).ToList();
    }
}