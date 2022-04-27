using System;

namespace DigitalHealthCheckWeb.Helpers
{
    public static class SanitisationExtensions
    {
        /// <summary>
        /// Converts the value to either "yes", "no", or null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>"yes" if the value was true, "no" if the value was false, null if the value was null.</returns>
        public static string AsYesNoOrNull(this bool? value) =>
             value.HasValue ? (value.Value ? "yes" : "no") : null;

        /// <summary>
        /// Attempts to convert a string into an enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="sanitisedValue">The sanitised value.</param>
        /// <returns></returns>
        public static bool SanitiseEnum<T>(this string value, out T? sanitisedValue) where T : struct
        {
            if (string.IsNullOrEmpty(value) ||
                !Enum.TryParse<T>(value, true, out var sanitised))
            {
                sanitisedValue = null;
                return false;
            }
            else
            {
                sanitisedValue = sanitised;
                return true;
            }
        }

        /// <summary>
        /// Attempts to convert a string containing "yes" or "no" into a boolean.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="sanitisedValue">if value is "yes", <c>true</c>; otherwise false.</param>
        /// <returns>True if the value could be converted to bool, otherwise false.</returns>
        public static bool SanitiseYesNo(this string value, out bool sanitisedValue)
        {
            if (string.IsNullOrEmpty(value) || (value != "yes" && value != "no"))
            {
                sanitisedValue = default;
                return false;
            }
            else
            {
                sanitisedValue = value == "yes";
                return true;
            }
        }
    }
}