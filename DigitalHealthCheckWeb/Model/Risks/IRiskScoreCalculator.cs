using DigitalHealthCheckEF;

namespace DigitalHealthCheckWeb.Model
{

    /// <summary>
    /// Calculates risk scores based on a user's submitted measurements.
    /// </summary>
    public interface IRiskScoreCalculator
    {
        /// <summary>
        /// Calculates the 10-year risk of developing cardiovascular disease (CVD).
        /// </summary>
        /// <param name="check">The health check.</param>
        /// <param name="townsendScore">The townsend deprivation score.</param>
        /// <returns>A value between 0 and 100 representing your percentage chance.</returns>
        double Calculate10YearCVDRiskScore(HealthCheck check, double townsendScore, bool useVariant);

        /// <summary>
        /// Calculates the 10-year risk of developing Type 2 diabetes.
        /// </summary>
        /// <param name="check">The health check.</param>
        /// <param name="townsendScore">The townsend deprivation score.</param>
        /// <returns>A value between 0 and 100 representing your percentage chance.</returns>
        double Calculate10YearDiabetesRiskScore(HealthCheck check, double townsendScore, bool useVariant);

        /// <summary>
        /// Calculates the Alcohol Use Disorders Identification Test (AUDIT) score.
        /// </summary>
        /// <param name="check">The health check.</param>
        /// <returns>A value between 0 and 40, where 0 indicates less evidence of alcohol use disorders.</returns>
        int CalculateAuditScore(HealthCheck check, bool useVariant);

        /// <summary>
        /// Calculates the General Anxiety Disorder-2 (GAD-2) score.
        /// </summary>
        /// <param name="check">The health check.</param>
        /// <returns>
        /// A value between 0 and 7, where 0 indicates unlikely to have general anxiety disorder.
        /// </returns>
        int CalculateGAD2Score(HealthCheck check, bool useVariant);

        /// <summary>
        /// Calculates the General Practice Physical Activity Questionnaire (GPPAQ) score.
        /// </summary>
        /// <param name="check">The health check.</param>
        /// <returns>A value between 0 and 3, where 0 indicates no activity.</returns>
        int CalculateGPPAQScore(HealthCheck check, bool useVariant);

        /// <summary>
        /// Calculates the relative age a person with no modifiable risk factors 
        /// would have to be to get the same CVD risk score provided.
        /// </summary>
        /// <param name="check">The health check.</param>
        /// <param name="tenYearCVDRiskScore">The CVD risk score to find a comparable heart age for.</param>
        /// <returns>An age between 0 and 84. If the age is above 84 it returns 84 still.</returns>
        int CalculateHeartAge(HealthCheck check, double tenYearCVDRiskScore, bool useVariant);
    }
}