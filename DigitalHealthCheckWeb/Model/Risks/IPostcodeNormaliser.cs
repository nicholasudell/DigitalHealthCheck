namespace DigitalHealthCheckWeb.Model.Risks
{
    /// <summary>
    /// Interface for a service to normalise postcodes.
    /// </summary>
    public interface IPostcodeNormaliser
    {
        /// <summary>
        /// Normalises the given postcode.
        /// </summary>
        /// <param name="postcode">The postcode.</param>
        /// <returns>The normalised postcode.</returns>
        string Normalise(string postcode);

        /// <summary>
        /// Validates where a given postcode is valid.
        /// </summary>
        /// <param name="postcode">The postcode.</param>
        /// <returns>True if the postcode is valid; otherwise false.</returns>
        bool Validate(string postcode);
    }
}