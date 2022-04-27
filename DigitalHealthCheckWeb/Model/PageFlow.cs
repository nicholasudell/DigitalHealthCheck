using System.Collections.Generic;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Helpers;

namespace DigitalHealthCheckWeb.Model
{
    /// <summary>
    /// Represents the page flow throughout the website.
    /// </summary>
    /// <seealso cref="IPageFlow" />
    public class PageFlow : IPageFlow
    {
        private readonly IHealthPriorityRouter healthPriorityRouter;
        private readonly IEveryoneHealthReferralService everyoneHealthReferralService;
        private readonly Database database;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageFlow"/> class.
        /// </summary>
        /// <param name="healthPriorityRouter">The health priority router.</param>
        /// <exception cref="System.ArgumentNullException">healthPriorityRouter</exception>
        public PageFlow(IHealthPriorityRouter healthPriorityRouter, IEveryoneHealthReferralService everyoneHealthReferralService, Database database)
        {
            this.healthPriorityRouter = healthPriorityRouter ?? throw new System.ArgumentNullException(nameof(healthPriorityRouter));
            this.everyoneHealthReferralService = everyoneHealthReferralService;
            this.database = database;
        }

        /// <summary>
        /// Gets the next page for the user.
        /// </summary>
        /// <param name="check">The user's current health check.</param>
        /// <param name="currentPage">The page the user is currently on.</param>
        /// <returns>
        /// The page the user should go to next.
        /// </returns>
        public async Task<string> NextPage(HealthCheck check, string currentPage) =>
            await Flow(check).NextInSequence(x => x == currentPage);

        /// <summary>
        /// Gets the previous page for the user.
        /// </summary>
        /// <param name="check">The user's current health check.</param>
        /// <param name="currentPage">The page the user is currently on.</param>
        /// <returns>
        /// The page the user was last shown.
        /// </returns>
        /// <remarks>
        /// This is used to populate the Back button on each page without requiring javascript.
        /// </remarks>
        public async Task<string> PreviousPage(HealthCheck check, string currentPage) =>
            await Flow(check, true).PreviousInSequence(x => x == currentPage);

        private async IAsyncEnumerable<string> Flow(HealthCheck check, bool visitedFollowUpsOnly = false)
        {
            yield return "Index";
            yield return "HeightAndWeight";
            yield return "Sex";

            if (!string.IsNullOrEmpty(check?.Identity) && check?.Identity != "cis")
            {
                yield return "GenderAffirmation";

                if (check?.GenderAffirmation == true)
                {
                    yield return "GenderIdentity";
                }
            }

            yield return "Ethnicity";
            yield return "Smoking";

            switch (check?.SmokingStatus)
            {
                case SmokingStatus.Heavy:
                case SmokingStatus.Moderate:
                case SmokingStatus.Light:
                case null: //Not yet set, which means we're going to the Smoker page.
                    yield return "Smoker";
                    break;

                default:
                    break;
            }

            yield return "DoYouDrinkAlcohol";

            if (check?.DrinksAlcohol == true)
            {
                yield return "DrinkingFrequency";
                yield return "AUDIT1";
                if (check?.MSASQ > AUDITFrequency.LessThanMonthly)
                {
                    yield return "AUDIT2";
                    yield return "AUDIT3";
                    yield return "AUDIT4";
                    yield return "AUDIT5";
                }
            }

            yield return "PhysicalActivity";
            yield return "GPPAQ1";
            yield return "GPPAQ2";
            yield return "GPPAQ3";
            yield return "Diabetes";

            if (check?.SexAtBirth == Sex.Female && check?.SexForResults == Sex.Female)
            {
                yield return "PolycysticOvariesAndGestationalDiabetes";
            }

            yield return "BloodSugar";
            yield return "RiskFactors1";
            yield return "RiskFactors2";
            yield return "RiskFactors3";
            yield return "BloodPressure";
            yield return "Cholesterol";
            yield return "MentalWellbeing";
            yield return "Complete";
            yield return "Validation";
            yield return "CheckYourAnswers";

            if (check is null || check.QRisk is null || check.Height is null || check.Weight is null)
            {
                yield return "FollowUpHeightAndWeight";
                yield return "HealthCheckInComplete";
            }
            else if (check is not null && check.QRisk is not null && check.Height is not null && check.Weight is not null)
            {
                yield return "Results";

                if (check.FirstHealthPriorityAfterResults is not null)
                {
                    if (visitedFollowUpsOnly && check.BookGPAppointmentFollowUp is not null)
                    {
                        yield return "FollowUpGP";
                    }

                    foreach (var followup in (visitedFollowUpsOnly ? healthPriorityRouter.AllFollowupsVisitedFor(check) : healthPriorityRouter.AllFollowupsFor(check)))
                    {
                        //All the routes for health priorities start with ./ so we strip that.
                        yield return healthPriorityRouter.GetRouteForHealthPriority(followup)[2..];
                    }

                    yield return "FollowUpActions";

                    await database.Entry(check).Collection(c => c.ChosenInterventions).LoadAsync();

                    if (everyoneHealthReferralService.HasEveryoneHealthReferrals(check))
                    {
                        yield return "EveryoneHealthConsent";
                    }

                    yield return "HealthCheckComplete";
                }
            }
        }
    }
}