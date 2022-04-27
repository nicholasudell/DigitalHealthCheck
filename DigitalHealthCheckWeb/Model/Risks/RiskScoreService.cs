using System;
using System.Threading.Tasks;
using DigitalHealthCheckEF;
using Microsoft.Extensions.Logging;

namespace DigitalHealthCheckWeb.Model.Risks
{
    /// <summary>
    /// Calculates and saves result sets for the health check.
    /// </summary>
    /// <seealso cref="DigitalHealthCheckWeb.Model.Risks.IRiskScoreService" />
    public class RiskScoreService : IRiskScoreService
    {
        private readonly IRiskScoreCalculator calculator;
        private readonly Database database;
        private readonly ILogger<RiskScoreService> logger;
        private readonly IPostcodeLookupService postcodeLookupService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RiskScoreService" /> class.
        /// </summary>
        /// <param name="postcodeLookupService">The postcode lookup service.</param>
        /// <param name="riskScoreCalculator">The risk score calculator.</param>
        /// <param name="database">The database.</param>
        /// <exception cref="System.ArgumentNullException">
        /// postcodeLookupService
        /// or
        /// riskScoreCalculator
        /// or
        /// database
        /// </exception>
        public RiskScoreService(IPostcodeLookupService postcodeLookupService, IRiskScoreCalculator riskScoreCalculator, Database database, ILogger<RiskScoreService> logger)
        {
            this.postcodeLookupService = postcodeLookupService ?? throw new System.ArgumentNullException(nameof(postcodeLookupService));
            calculator = riskScoreCalculator ?? throw new System.ArgumentNullException(nameof(riskScoreCalculator));
            this.database = database ?? throw new System.ArgumentNullException(nameof(database));
            this.logger = logger;
        }

        public async Task UpdateRisksForCheckAsync(string id, bool useVariant)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException($"'{nameof(id)}' cannot be null or empty.", nameof(id));
            }

            logger.LogInformation($"Loading check for id {id}");

            var check = await database.HealthChecks.FindAsync(Guid.Parse(id));

            logger.LogInformation($"Got check for id {id}");

            //Because we're loading asynchronously, we need to force a reload here, or sometimes we don't find the variant.

            await database.Entry(check).ReloadAsync();

            logger.LogInformation($"Reloaded check for id {id}");

            if (useVariant)
            {
                await database.Entry(check).Reference(c => c.Variant).LoadAsync();
            }

            if (check is null)
            {
                throw new System.InvalidOperationException($"Check with id {id} could not be found.");
            }

            await UpdateRisksForCheckAsync(check, useVariant);
        }

        /// <summary>
        /// Calculates new risks for a health check and updates that record in the database.
        /// </summary>
        /// <param name="check">The check to update.</param>
        /// <exception cref="System.ArgumentNullException">check</exception>
        public async Task UpdateRisksForCheckAsync(HealthCheck check, bool useVariant)
        {
            if (check is null)
            {
                throw new System.ArgumentNullException(nameof(check));
            }

            //Default townsend is always 0 because the townsend score represents deviation from the national average
            logger.LogInformation($"Getting townsend score");
            var townsend = await postcodeLookupService.GetTownsendScoreAsync(check.Postcode) ?? 0;

            logger.LogInformation($"Calculating QRISK");

            var qrisk = calculator.Calculate10YearCVDRiskScore(check, townsend, useVariant);

            if(useVariant)
            {
                check.Variant.QRisk = qrisk;
            }
            else
            {
                check.QRisk = qrisk;
            }

            logger.LogInformation($"Calculating Heart Age");

            var heartAge = calculator.CalculateHeartAge(check, check.QRisk.Value, useVariant);

            if (useVariant)
            {
                check.Variant.HeartAge = heartAge;
            }
            else
            {
                check.HeartAge = heartAge;
            }

            logger.LogInformation($"Calculating QDiabetes");

            var qDiabetes = calculator.Calculate10YearDiabetesRiskScore(check, townsend, useVariant);

            if (useVariant)
            {
                check.Variant.QDiabetes = qDiabetes;
            }
            else
            {
                check.QDiabetes = qDiabetes;
            }

            logger.LogInformation($"Calculating GPPAQ");

            var physicalActivity = calculator.CalculateGPPAQScore(check, useVariant);

            if (useVariant)
            {
                check.Variant.GPPAQ = physicalActivity;
            }
            else
            {
                check.GPPAQ = physicalActivity;
            }

            logger.LogInformation($"Calculating AUDIT");

            var audit = calculator.CalculateAuditScore(check, useVariant);

            if (useVariant)
            {
                check.Variant.AUDIT = audit;
            }
            else
            {
                check.AUDIT = audit;
            }

            logger.LogInformation($"Calculating GAD2");

            if (check.SkipMentalHealthQuestions == false)
            {
                var anxiety = calculator.CalculateGAD2Score(check, useVariant);

                if (useVariant)
                {
                    check.Variant.GAD2 = anxiety;
                }
                else
                {
                    check.GAD2 = anxiety;
                }
            }

            logger.LogInformation($"Calculation finished at {DateTime.UtcNow}");

            check.CalculatedDate = DateTime.UtcNow;

            logger.LogInformation($"Updating calculation date to {check.CalculatedDate}");

            await database.SaveChangesAsync();

            logger.LogInformation($"Saved changes.");
        }
    }
}