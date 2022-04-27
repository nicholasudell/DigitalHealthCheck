using System.Collections.Generic;
using System.Linq;
using DigitalHealthCheckEF;

namespace DigitalHealthCheckCommon
{
    public class EveryoneHealthReferralService : IEveryoneHealthReferralService
    {
        public EveryoneHealthReferralService(string everyoneHealthReferralEmail)
        {
            if (string.IsNullOrWhiteSpace(everyoneHealthReferralEmail))
            {
                throw new System.ArgumentException($"'{nameof(everyoneHealthReferralEmail)}' cannot be null or whitespace.", nameof(everyoneHealthReferralEmail));
            }

            EveryoneHealthReferralEmail = everyoneHealthReferralEmail;
        }

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

        public string EveryoneHealthReferralEmail { get; }

        public bool HasEveryoneHealthReferrals(HealthCheck check) =>
            GetEveryoneHealthReferrals(check).Any();

        public IEnumerable<Intervention> GetEveryoneHealthReferrals(HealthCheck check) =>
            check.ChosenInterventions
                .Where(x => EveryoneHealthInterventionIds.Contains(x.Id));

        public IEnumerable<string> ReferralNames(IEnumerable<Intervention> interventions) =>
            interventions.Select(x => x.Category switch
            {
                "smoking" => "Smoking Cessation Services",
                "move" => "Physical Activity Services",
                "mental" => "Mental Wellbeing Services",
                "weight" => "Weight Management Services",
                "alcohol" => "Stop Drinking Services",
                "improvecholesterol" => "Cholesterol Services",
                "improvebloodsugar" => "Blood Sugar Services",
                "improvebloodpressure" => "Blood Pressure Services",
                _ => null
            }).Where(x => x is not null);
    }
}
