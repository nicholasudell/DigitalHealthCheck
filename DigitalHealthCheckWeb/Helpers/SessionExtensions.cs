using System;
using Microsoft.AspNetCore.Http;

namespace DigitalHealthCheckWeb.Helpers
{
    public static class SessionExtensions
    {
        /// <summary>
        /// Gets a value of type bool with matching key from the user's session.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="key">The key.</param>
        /// <returns>The value if it exists, otherwise null.</returns>
        public static bool? GetBool(this ISession session, string key)
        {
            if (session is null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            return bool.TryParse(session.GetString(key), out var parsedValue) ? parsedValue : null;
        }

        /// <summary>
        /// Gets a value of type DateTime with matching key from the user's session.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="key">The key.</param>
        /// <returns>The value if it exists, otherwise null.</returns>
        public static DateTime? GetDateTime(this ISession session, string key)
        {
            if (session is null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            return DateTime.TryParse(session.GetString(key), out var parsedValue) ? parsedValue : null;
        }

        /// <summary>
        /// Gets an enum value of type T with matching key from the user's session.
        /// </summary>
        /// <typeparam name="T">The type of enum to get</typeparam>
        /// <param name="session">The session.</param>
        /// <param name="key">The key.</param>
        /// <returns>
        /// The value if it exists, otherwise null.
        /// </returns>
        public static T? GetEnum<T>(this ISession session, string key)
            where T : struct
        {
            if (session is null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            return Enum.TryParse<T>(session.GetString(key), out var parsedValue) ? parsedValue : null;
        }

        /// <summary>
        /// Sets a boolean value with the provided key in the user's session.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value to store.</param>
        public static void SetBool(this ISession session, string key, bool value)
        {
            if (session is null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            session.SetString(key, value.ToString());
        }
    }
}