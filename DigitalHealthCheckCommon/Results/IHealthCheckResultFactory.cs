using DigitalHealthCheckEF;

namespace DigitalHealthCheckCommon
{
    /// <summary>
    /// Creates <see cref="Result"/> instances by analysing <see cref="HealthCheck"/> instances.
    /// </summary>
    public interface IHealthCheckResultFactory
    {
        /// <summary>
        /// Analyses a health check to determine the health state of the user.
        /// Each status is along the lines of "Healthy", "Warning", "Danger"
        /// although some vary (e.g. Blood Pressure where it's possible to be
        /// too low).
        /// </summary>
        /// <param name="check">The check to analyse.</param>
        /// <param name="useVariant">Whether to use the values stored in the identity variant or not.</param>
        /// <returns>A <see cref="Result"/> object.</returns>
        Result GetResult(HealthCheck check, bool useVariant);
    }
}