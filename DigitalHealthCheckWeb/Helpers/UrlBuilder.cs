using System;
using Microsoft.AspNetCore.Http;

namespace DigitalHealthCheckWeb.Helpers
{
    /// <summary>
    /// URL helper operations
    /// </summary>
    /// <seealso cref="IUrlBuilder" />
    public class UrlBuilder : IUrlBuilder
    {
        /// <summary>
        /// Gets the base URL from a request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The base URL for the site.
        /// </returns>
        /// <exception cref="ArgumentNullException">request</exception>
        [Obsolete("Use the extension method Request.BaseUrl() instead.")]
        public string GetBaseUrl(HttpRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return request.BaseUrl();
        }
    }
}