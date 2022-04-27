using System;
using System.Text.RegularExpressions;

namespace ServiceComponents
{

    public static class StringExtensions
    {
        /// <summary>
        /// Converts the first letter in a message to lower case.
        /// </summary>
        /// <param name="text">The message to convert</param>
        /// <returns></returns>
        public static string FromTitleCase(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            if (text.Length == 1)
            {
                return text.ToLower();
            }

            return text[..1].ToLower() + text[1..];
        }

        /// <summary>
        /// Checks if an email is valid by attempting to create a <see cref="MailAddress"/> instance from it.
        /// </summary>
        /// <remarks>
        /// This isn't true email validation, which is hopelessly complex, but it catches most common issues.
        /// </remarks>
        /// <param name="email">The address to test.</param>
        /// <returns>True if the email is valid according to <see cref="MailAddress"/>; otherwise false.</returns>
        public static bool IsValidEmail(this string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if a postcode is valid.
        /// </summary>
        /// <param name="postcode">The postcode to check</param>
        /// <returns>True if the postcode is a valid UK postcode; otherwise false.</returns>
        public static bool IsValidPostcode(this string postcode)
        {
            //Validation regex as defined here:
            //https://en.wikipedia.org/wiki/Postcodes_in_the_United_Kingdom#Validation

            var test = new Regex("^([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z])))) {0,1}[0-9][A-Za-z]{2})$");

            return test.IsMatch(postcode.Trim());
        }
        /// <summary>
        /// Removes any of the possible list of suffixes from a string.
        /// </summary>
        /// <remarks>Use this for numeric entry that also has units, e.g. "10kg" can be turned into "10".</remarks>
        /// <param name="value">The value to remove suffixes from.</param>
        /// <param name="possibleSuffixes">A list of suffixes to remove if any are found.</param>
        /// <returns>The value with any suffixes removed if found.</returns>
        public static string RemoveSuffix(this string value, params string[] possibleSuffixes)
        {
            foreach (var suffix in possibleSuffixes)
            {
                value = value.RemoveSuffix(suffix);
            }

            return value;
        }

        /// <summary>
        /// Converts the first letter in a message to upper case.
        /// </summary>
        /// <param name="text">The message to convert</param>
        /// <returns></returns>
        public static string ToTitleCase(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            if (text.Length == 1)
            {
                return text.ToUpper();
            }

            return text[..1].ToUpper() + text[1..];
        }

        private static string RemoveSuffix(this string value, string suffix)
        {
            var suffixIndex = value.LastIndexOf(suffix, StringComparison.OrdinalIgnoreCase);

            if (suffixIndex == -1)
            {
                return value;
            }

            return value.Remove(suffixIndex);
        }
    }
}