using DigitalHealthCheckEF;

namespace DigitalHealthCheckWeb.Model
{
    public class StubRiskScoreCalculator : IRiskScoreCalculator
    {
        public double Calculate10YearCVDRiskScore(HealthCheck check, double townsendScore, bool useVariant) => 0d;
        public double Calculate10YearDiabetesRiskScore(HealthCheck check, double townsendScore, bool useVariant) => 0d;
        public int CalculateAuditScore(HealthCheck check, bool useVariant) => 0;
        public int CalculateGAD2Score(HealthCheck check, bool useVariant) => 0;
        public int CalculateGPPAQScore(HealthCheck check, bool useVariant) => 0;
        public int CalculateHeartAge(HealthCheck check, double tenYearCVDRiskScore, bool useVariant) => 0;
    }
}