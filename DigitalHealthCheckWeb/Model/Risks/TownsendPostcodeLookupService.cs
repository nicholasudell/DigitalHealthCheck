using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalHealthCheckWeb.Model.Risks
{
    /// <summary>
    /// Postcode lookup service using the QRISK Townsend database.
    /// </summary>
    /// <seealso cref="IPostcodeLookupService"/>
    public class TownsendPostcodeLookupService : IPostcodeLookupService
    {
        private readonly IPostcodeNormaliser postcodeNormaliser;
        private readonly ISqlCommandSender sqlCommandSender;

        /// <summary>
        /// Initializes a new instance of the <see cref="TownsendPostcodeLookupService" /> class.
        /// </summary>
        /// <param name="sqlCommandSender">The SQL command sender.</param>
        /// <param name="postcodeNormaliser">The postcode normaliser.</param>
        /// <exception cref="ArgumentNullException">
        /// sqlCommandSender
        /// or
        /// postcodeNormaliser
        /// </exception>
        public TownsendPostcodeLookupService(ISqlCommandSender sqlCommandSender, IPostcodeNormaliser postcodeNormaliser)
        {
            this.sqlCommandSender = sqlCommandSender ?? throw new ArgumentNullException(nameof(sqlCommandSender));
            this.postcodeNormaliser = postcodeNormaliser ?? throw new ArgumentNullException(nameof(postcodeNormaliser));
        }

        /// <summary>
        /// Gets the "townsend" deprivation-based heart risk score for a given postcode.
        /// </summary>
        /// <param name="postcode">The postcode.</param>
        /// <returns>
        /// The townsend deprivation score for that postcode if it is valid, otherwise null.
        /// </returns>
        /// <exception cref="ArgumentException">'{nameof(postcode)}' cannot be null or empty. - postcode</exception>
        public async Task<double?> GetTownsendScoreAsync(string postcode)
        {
            if (string.IsNullOrEmpty(postcode))
            {
                throw new ArgumentException($"'{nameof(postcode)}' cannot be null or empty.", nameof(postcode));
            }

            postcode = postcodeNormaliser.Normalise(postcode);

            var result = await sqlCommandSender.SendScalarAsync
            (
                "SELECT townsend FROM [Townsend] WHERE postcode = @postCode",
                new Dictionary<string, object> { { "@postCode", postcode } }
            );

            return result == null || result == DBNull.Value ? null : Convert.ToDouble(result);
        }

        public async Task<int?> GetIndexOfMultipleDeprivationQuintileAsync(string postcode)
        {
            if (string.IsNullOrEmpty(postcode))
            {
                throw new ArgumentException($"'{nameof(postcode)}' cannot be null or empty.", nameof(postcode));
            }

            postcode = postcodeNormaliser.Normalise(postcode);

            var result = await sqlCommandSender.SendScalarAsync
            (
                "SELECT IMDQuintile FROM [Townsend] WHERE postcode = @postCode",
                new Dictionary<string, object> { { "@postCode", postcode } }
            );

            return result == null || result == DBNull.Value ? null : Convert.ToInt32(result);
        }
    }
}