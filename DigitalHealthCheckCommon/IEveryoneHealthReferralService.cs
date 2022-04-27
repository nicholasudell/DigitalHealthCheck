using System.Collections.Generic;
using DigitalHealthCheckEF;

namespace DigitalHealthCheckCommon
{
    public interface IEveryoneHealthReferralService
    {
        string EveryoneHealthReferralEmail { get; }
        IEnumerable<Intervention> GetEveryoneHealthReferrals(HealthCheck check);
        bool HasEveryoneHealthReferrals(HealthCheck check);
        IEnumerable<string> ReferralNames(IEnumerable<Intervention> interventions);
    }
}