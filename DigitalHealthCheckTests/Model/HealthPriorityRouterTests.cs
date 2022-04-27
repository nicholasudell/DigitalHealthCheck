using System.Collections.Generic;
using System.Linq;
using DigitalHealthCheckCommon;
using Moq;
using NUnit.Framework;

namespace DigitalHealthCheckWeb.Model.Tests
{
    [TestFixture()]
    public class HealthPriorityRouterTests
    {
        static readonly IDictionary<string, string> ExpectedHealthPriorityRouteMap = new Dictionary<string, string>()
        {
            { "Alcohol","./FollowUpAlcohol" },
            { "Move","./FollowUpMoveMore" },
            { "Smoking","./FollowUpSmoking" },
            { "Mental","./FollowUpMentalWellbeing" },
            { "Weight","./FollowUpWeight" },
            { "BloodPressure","./FollowUpBloodPressure" },
            { "Cholesterol","./FollowUpCholesterol" },
            { "BloodSugar","./FollowUpBloodSugar" },
            { "ImproveBloodPressure","./FollowUpImproveBloodPressure" },
            { "ImproveCholesterol","./FollowUpImproveCholesterol" },
            { "ImproveBloodSugar","./FollowUpImproveBloodSugar" }
        };

        Mock<IHealthCheckResultFactory> mockResultFactory;

        [Test()]
        public void GetRouteForHealthPriority_IgnoresCasing()
        {
            var cut = CreateTarget();

            var actual = cut.GetRouteForHealthPriority("ALCOHOL");

            Assert.That(actual, Is.EqualTo(ExpectedHealthPriorityRouteMap["Alcohol"]));
        }

        [TestCaseSource(nameof(TestData))]
        public void GetRouteForHealthPriority_TestRouteMatchesExpected(string priority, string expectedRoute)
        {
            var cut = CreateTarget();

            var actual = cut.GetRouteForHealthPriority(priority);

            Assert.That(actual, Is.EqualTo(expectedRoute));
        }

        [SetUp]
        public void Setup() => mockResultFactory = new Mock<IHealthCheckResultFactory>();

        static IEnumerable<TestCaseData> TestData()
            => ExpectedHealthPriorityRouteMap.Select(x =>
                new TestCaseData(x.Key, x.Value)
                    .SetDescription($"The HealthPriorityRouter should respond with route {x.Value} when asked for a route for priority {x.Key}.")
                    .SetName($"GetRouteForHealthPriority_TestRouteFor{x.Key}GoesTo{x.Value.Replace("./", string.Empty)}.")
            );

        HealthPriorityRouter CreateTarget() => new(mockResultFactory.Object);
    }
}