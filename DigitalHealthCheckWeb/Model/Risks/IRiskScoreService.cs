using System.Threading.Tasks;
using DigitalHealthCheckEF;

namespace DigitalHealthCheckWeb.Model.Risks
{
    /// <summary>
    /// Interface for a service that calculates and saves a set of results for a health check.
    /// </summary>
    public interface IRiskScoreService
    {
        /// <summary>
        /// Calculates new risks for a health check and updates that record in the database.
        /// </summary>
        /// <param name="id">The id of the check to update.</param>
        /// <returns>An awaitable task.</returns>
        Task UpdateRisksForCheckAsync(string id, bool useVariant);

        /// <summary>
        /// Calculates new risks for a health check and updates that record in the database.
        /// </summary>
        /// <param name="check">The check to update.</param>
        /// <returns>An awaitable task.</returns>
        Task UpdateRisksForCheckAsync(HealthCheck check, bool useVariant);
    }
}