using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using NUnit.Framework;
using Shouldly;

namespace DigitalHealthCheckWeb.Tests.Model
{
    [TestFixture]
    public class EveryoneHealthReferralServiceTests
    {
        static readonly int[] EveryoneHealthInterventionIds = new[]
        {
            Database.EveryoneHealthSmokingReferral,
            Database.EveryoneHealthWeightReferral,
            Database.EveryoneHealthMoveReferral,
            Database.EveryoneHealthAlcoholReferral,
            Database.EveryoneHealthMentalReferral,
            Database.EveryoneHealthCholesterolReferral,
            Database.EveryoneHealthBloodSugarReferral,
            Database.EveryoneHealthBloodPressureReferral
        };

        static IEnumerable<TestCaseData> ValidIdTests()
            => EveryoneHealthInterventionIds.Select(x =>
            {
                var testCase = new TestCaseData(x);
                testCase.SetName($"HasEveryoneHealthReferrals_WhenReferralIdIs{x}_ReturnsTrue");
                return testCase;
            });

        [TestCaseSource(nameof(ValidIdTests))]
        public void HasEveryoneHealthReferrals_WhenAnyReferralIdExists_ReturnsTrue(int id)
        {
            var check = new HealthCheck()
            {
                ChosenInterventions = new List<Intervention>()
                {
                    new Intervention
                    {
                        Id = id
                    }
                }
            };

            var service = CreateService();

            service.HasEveryoneHealthReferrals(check).ShouldBe(true);
        }

        private static EveryoneHealthReferralService CreateService() => new("foo");

        [Test]
        public void HasEveryoneHealthReferrals_WhenNoReferralIdExists_ReturnsFalse()
        {
            var check = new HealthCheck()
            {
                ChosenInterventions = new List<Intervention>()
                {
                    new Intervention
                    {
                        Id = 90000
                    }
                }
            };

            var service = CreateService();

            service.HasEveryoneHealthReferrals(check).ShouldBe(false);
        }

        [Test]
        public void HasEveryoneHealthReferrals_WhenInterventionsIsEmpty_ReturnsFalse()
        {
            var check = new HealthCheck()
            {
                ChosenInterventions = new List<Intervention>()
                {
                }
            };

            var service = CreateService();

            service.HasEveryoneHealthReferrals(check).ShouldBe(false);
        }

        [Test]
        public void HasEveryoneHealthReferrals_WhenInterventionsIsNull_ThrowsException()
        {
            var check = new HealthCheck()
            {
                ChosenInterventions = null
            };

            var service = CreateService();

            Should.Throw<ArgumentNullException>(() => service.HasEveryoneHealthReferrals(check));
        }

        [Test]
        public void ReferralNames_FiltersOutNonMatchingInterventions()
        {
            var interventions = new List<Intervention>()
                {
                    new Intervention
                    {
                        Id = 90000
                    }
                };

            var service = CreateService();

            service.ReferralNames(interventions).Count().ShouldBe(0);
        }

    }
}
