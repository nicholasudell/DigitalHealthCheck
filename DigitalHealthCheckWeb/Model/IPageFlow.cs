using System.Threading.Tasks;
using DigitalHealthCheckEF;

namespace DigitalHealthCheckWeb.Model
{
    /// <summary>
    /// Represents the page flow throughout the website.
    /// </summary>
    /// <remarks>
    public interface IPageFlow
    {
        /// <summary>
        /// Gets the next page for the user.
        /// </summary>
        /// <param name="check">The user's current health check.</param>
        /// <param name="currentPage">The page the user is currently on.</param>
        /// <returns>The page the user should go to next.</returns>
        Task<string> NextPage(HealthCheck check, string currentPage);

        /// <summary>
        /// Gets the previous page for the user.
        /// </summary>
        /// <remarks>This is used to populate the Back button on each page without requiring javascript.</remarks>
        /// <param name="check">The user's current health check.</param>
        /// <param name="currentPage">The page the user is currently on.</param>
        /// <returns>The page the user was last shown.</returns>
        Task<string> PreviousPage(HealthCheck check, string currentPage);
    }
}