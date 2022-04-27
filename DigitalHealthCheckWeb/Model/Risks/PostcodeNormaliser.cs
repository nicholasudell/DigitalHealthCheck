using System.Text.RegularExpressions;

namespace DigitalHealthCheckWeb.Model.Risks
{
    /// <summary>
    /// Postcode normaliser that strips spaces and converts the postcode to upper case.
    /// </summary>
    /// <seealso cref="IPostcodeNormaliser"/>
    /// <seealso cref="Utilities.validateAndRegularisePostcode(string)"/>
    public class PostcodeNormaliser : IPostcodeNormaliser
    {
        /// <summary>
        /// Normalises the given postcode.
        /// </summary>
        /// <param name="postcode">The postcode.</param>
        /// <returns>The normalised postcode.</returns>
        public string Normalise(string postcode) => postcode.Replace(" ", "").ToUpper();

        /// <summary>
        /// Validates where a given postcode is valid.
        /// </summary>
        /// <param name="postcode">The postcode.</param>
        /// <returns>True if the postcode is valid; otherwise false.</returns>
        public bool Validate(string postcode)

        {
            //Government's postcode regex reportedly has a flaw (https://stackoverflow.com/a/51885364/315544). This simpler regex will allow *some* invalid regexes, but should predominantly work still.
            var regex = new Regex(@"^([A-Z][A-HJ-Y]?\d[A-Z\d]? ?\d[A-Z]{2}|GIR ?0A{2})$");

            return regex.IsMatch(postcode);
        }
    }
}