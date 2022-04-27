using Microsoft.AspNetCore.Http;

namespace DigitalHealthCheckWeb.Helpers
{
    /// <summary>
    /// URL helper operations
    /// </summary>
    public interface IUrlBuilder
    {
        /// <summary>
        /// Gets the base URL from a request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The base URL for the site.</returns>
        string GetBaseUrl(HttpRequest request);
    }
}