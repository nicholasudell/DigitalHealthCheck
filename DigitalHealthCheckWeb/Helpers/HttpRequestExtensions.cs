using System;
using Microsoft.AspNetCore.Http;

namespace DigitalHealthCheckWeb.Helpers
{
    /// <summary>
    /// Extension methods for the <see cref="HttpRequest"/> object.
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Gets the base URL from a request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The base URL for the site.
        /// </returns>
        public static string BaseUrl(this HttpRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return $"{request.Scheme}://{request.Host}{request.PathBase}";
        }

        /// <summary>
        /// Gets the relative path of the current page from a HTTP request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The relative path of the current page, e.g. "FollowUpBloodPressure"</returns>
        public static string CurrentPage(this HttpRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var path = request.Path;
            var pathBase = request.PathBase;

            if (!path.HasValue)
            {
                return null;
            }

            if (!pathBase.HasValue)
            {
                return path.Value;
            }

            if (!path.Value.Contains(pathBase.Value, StringComparison.InvariantCulture))
            {
                return path.Value;
            }

            return path.Value.Remove(path.Value.IndexOf(pathBase.Value), pathBase.Value.Length);
        }
    }
}