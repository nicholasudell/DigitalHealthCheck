using System.Threading.Tasks;

namespace DigitalHealthCheckWeb.Model
{
    /// <summary>
    /// Interface for a postcode-based deprivation lookup system.
    /// </summary>
    public interface IPostcodeLookupService
    {
        /// <summary>
        /// Gets the "townsend" deprivation-based heart risk score for a given postcode.
        /// </summary>
        /// <param name="postcode">The postcode.</param>
        /// <returns>
        /// The townsend deprivation score for that postcode if it is valid, otherwise null.
        /// </returns>
        Task<double?> GetTownsendScoreAsync(string postcode);

        /// <summary>
        /// Gets the Index of Multiple Deprivation (IMD) quintile for a given postcode.
        /// </summary>
        /// <param name="postcode">The postcode.</param>
        /// <returns>
        /// The IMD quintile for that postcode if it is valid and exists in the database, otherwise null.
        /// </returns>
        Task<int?> GetIndexOfMultipleDeprivationQuintileAsync(string postcode);
    }
}