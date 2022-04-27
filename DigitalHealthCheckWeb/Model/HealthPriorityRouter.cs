using System.Collections.Generic;
using System.Linq;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;

namespace DigitalHealthCheckWeb.Model
{
    /// <summary>
    /// A router to determine which follow up page to show 
    /// based on a user's results and chosen health priorities.
    /// </summary>
    /// <seealso cref="DigitalHealthCheckWeb.Model.IHealthPriorityRouter" />
    public class HealthPriorityRouter : IHealthPriorityRouter
    {
        static readonly IEnumerable<string> orderedPriorities = new[]
        {
            "bloodpressure",
            "improvebloodpressure",
            "cholesterol",
            "improvecholesterol",
            "smoking",
            "alcohol",
            "weight",
            "bloodsugar",
            "improvebloodsugar",
            "move",
            "mental"
        };

        private readonly IHealthCheckResultFactory healthCheckResultFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthPriorityRouter"/> class.
        /// </summary>
        /// <param name="healthCheckResultFactory">The health check result factory.</param>
        /// <exception cref="System.ArgumentNullException">healthCheckResultFactory</exception>
        public HealthPriorityRouter(IHealthCheckResultFactory healthCheckResultFactory) =>
            this.healthCheckResultFactory = healthCheckResultFactory ?? throw new System.ArgumentNullException(nameof(healthCheckResultFactory));

        /// <summary>
        /// Gets all the valid follow up page routes for a person's check in order.
        /// </summary>
        /// <param name="check">The user's health check.</param>
        /// <returns>
        /// A list of strings where each string is the route to a FollowUp page (e.g. './FollowUpBloodPressure')
        /// </returns>
        /// <remarks>
        /// The resulting list will be ordered as follows:
        /// 1) The first and second priorities chosen by the user.
        /// 2) The remaining followups the user is eligible for, in order of burden of disease.
        /// </remarks>
        public IEnumerable<string> AllFollowupsFor(HealthCheck check)
        {
            if (check is null)
            {
                throw new System.ArgumentNullException(nameof(check));
            }

            if (check.FirstHealthPriorityAfterResults != null && check.SecondHealthPriorityAfterResults != null)
            {
                yield return check.FirstHealthPriorityAfterResults;
                yield return check.SecondHealthPriorityAfterResults;
            }

            var remainingRoutes = orderedPriorities.Except
            (
                new[] { check.FirstHealthPriorityAfterResults, check.SecondHealthPriorityAfterResults }
            );

            var results = healthCheckResultFactory.GetResult(check, false);

            if (results.Smoker != DefaultStatus.Healthy && remainingRoutes.Contains("smoking"))
            {
                yield return "smoking";
            }

            if (results.BloodPressure is null && remainingRoutes.Contains("bloodpressure"))
            {
                yield return "bloodpressure";
            }

            if (results.BloodPressure is not null && results.BloodPressure != BloodPressureStatus.Healthy && remainingRoutes.Contains("improvebloodpressure"))
            {
                yield return "improvebloodpressure";
            }

            if (results.CholesterolRatio is null && remainingRoutes.Contains("cholesterol"))
            {
                yield return "cholesterol";
            }

            if (results.CholesterolRatio is not null &&
                (
                    results.CholesterolRatio != DefaultStatus.Healthy ||
                    results.HdlCholesterol != DefaultStatus.Healthy ||
                    results.Cholesterol != DefaultStatus.Healthy
                ) && remainingRoutes.Contains("improvecholesterol"))
            {
                yield return "improvecholesterol";
            }

            if (results.BloodSugar is null && (results.BodyMassIndex > BodyMassIndexStatus.Overweight || results.BloodPressure >= BloodPressureStatus.High || results.Diabetes == DefaultStatus.Danger) && remainingRoutes.Contains("bloodsugar"))
            {
                yield return "bloodsugar";
            }
            if (results.BloodSugar is not null && results.BloodSugar != BloodSugarStatus.Healthy && remainingRoutes.Contains("improvebloodsugar"))
            {
                yield return "improvebloodsugar";
            }

            if (results.BodyMassIndex != BodyMassIndexStatus.Healthy && remainingRoutes.Contains("weight"))
            {
                yield return "weight";
            }

            if (results.Alcohol != DefaultStatus.Healthy && remainingRoutes.Contains("alcohol"))
            {
                yield return "alcohol";
            }

            if (results.PhysicalActivity != PhysicalActivityStatus.Active && remainingRoutes.Contains("move"))
            {
                yield return "move";
            }

            if ((check.GAD2 >= 2 || check.Disinterested == true || check.FeelingDown == true) && remainingRoutes.Contains("mental"))
            {
                yield return "mental";
            }
        }

        /// <summary>
        /// Gets all the valid follow up page routes that a person has already been to during their Health Check.
        /// </summary>
        /// <param name="check">The user's health check.</param>
        /// <returns>
        /// A list of strings where each string is the route to a FollowUp page (e.g. './FollowUpBloodPressure')
        /// </returns>
        /// <remarks>
        /// The resulting list will be ordered as follows:
        /// 1) The first and second priorities chosen by the user.
        /// 2) The remaining followups the user is eligible for, in order of burden of disease.
        /// </remarks>
        public IEnumerable<string> AllFollowupsVisitedFor(HealthCheck check)
        {
            if (check is null)
            {
                throw new System.ArgumentNullException(nameof(check));
            }

            var allFollowups = AllFollowupsFor(check);

            foreach (var followup in allFollowups)
            {
                if (followup == check.FirstHealthPriorityAfterResults)
                {
                    yield return followup;
                }
                else if (followup == check.SecondHealthPriorityAfterResults)
                {
                    yield return followup;
                }
                else
                {
                    var previousFollowup = followup switch
                    {
                        var f when (f == "bloodpressure" && check.BloodPressureFollowUp is not null) => f,
                        var f when (f == "improvebloodpressure" && check.ImproveBloodPressureFollowUp is not null) => f,
                        var f when (f == "cholesterol" && check.CholesterolFollowUp is not null) => f,
                        var f when (f == "improvecholesterol" && check.ImproveCholesterolFollowUp is not null) => f,
                        var f when (f == "smoking" && check.QuitSmokingFollowUp is not null) => f,
                        var f when (f == "alcohol" && check.DrinkLessFollowUp is not null) => f,
                        var f when (f == "weight" && check.HealthyWeightFollowUp is not null) => f,
                        var f when (f == "bloodsugar" && check.BloodSugarFollowUp is not null) => f,
                        var f when (f == "improvebloodsugar" && check.ImproveBloodSugarFollowUp is not null) => f,
                        var f when (f == "move" && check.MoveMoreFollowUp is not null) => f,
                        var f when (f == "mental" && check.MentalWellbeingFollowUp is not null) => f,
                        _ => null
                    };
                    if (previousFollowup != null)
                    {
                        yield return previousFollowup;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the health priority name for a given page route.
        /// </summary>
        /// <param name="route">The route, e.g. './FollowUpBloodPressure'</param>
        /// <returns>
        /// The health priority name for the route, e.g. 'bloodpressure'
        /// </returns>
        public string GetHealthPriorityForRoute(string route) =>
            orderedPriorities.SingleOrDefault(x => GetRouteForHealthPriority(x) == route);

        /// <summary>
        /// Gets the page route for a given health priority name.
        /// </summary>
        /// <param name="healthPriority">The health priority, e.g. 'bloodpressure'.</param>
        /// <returns>
        /// The health priority page route, e.g. './FollowUpBloodPressure'.
        /// </returns>
        /// <remarks>
        /// This is a reverse of <see cref="GetHealthPriorityForRoute(string)" />.
        /// </remarks>
        public string GetRouteForHealthPriority(string healthPriority) => healthPriority.ToLowerInvariant() switch
        {
            "alcohol" => "./FollowUpAlcohol",
            "move" => "./FollowUpMoveMore",
            "smoking" => "./FollowUpSmoking",
            "weight" => "./FollowUpWeight",
            "mental" => "./FollowUpMentalWellbeing",
            "bloodsugar" => "./FollowUpBloodSugar",
            "cholesterol" => "./FollowUpCholesterol",
            "bloodpressure" => "./FollowUpBloodPressure",
            "improvebloodsugar" => "./FollowUpImproveBloodSugar",
            "improvecholesterol" => "./FollowUpImproveCholesterol",
            "improvebloodpressure" => "./FollowUpImproveBloodPressure",
            _ => throw new KeyNotFoundException("Could not find a route for health priority " + healthPriority)
        };

        /// <summary>
        /// Gets the next page in the follow up process for the user.
        /// </summary>
        /// <param name="check">The user's health check.</param>
        /// <param name="currentRoute">The current page route that the user is on.</param>
        /// <returns>
        /// The page route for the next follow up that the user should be shown.
        /// </returns>
        public string NextPage(HealthCheck check, string currentRoute)
        {
            if (check is null)
            {
                throw new System.ArgumentNullException(nameof(check));
            }

            var followUpSequence = AllFollowupsFor(check).ToList();

            //Going to the follow up - book appointment with your gp means your
            //current route exists ("gp") but is not in the follow up sequence yet.

            if (string.IsNullOrEmpty(currentRoute) || !followUpSequence.Contains(currentRoute))
            {
                return GetRouteForHealthPriority(followUpSequence.First());
            }

            for (var i = 0; i < followUpSequence.Count; i++)
            {
                var followUp = followUpSequence[i];

                if (followUp == currentRoute)
                {
                    if (followUp == followUpSequence.Last())
                    {
                        return null;
                    }

                    return GetRouteForHealthPriority(followUpSequence[i + 1]);
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the previous page in the follow up process for the user.
        /// </summary>
        /// <param name="check">The user's health check.</param>
        /// <param name="currentRoute">The current page route that the user is on.</param>
        /// <returns>
        /// The page route for the previous follow up that the user was shown.
        /// </returns>
        public string PreviousPage(HealthCheck check, string currentRoute)
        {
            if (check is null)
            {
                throw new System.ArgumentNullException(nameof(check));
            }

            var followUpSequence = AllFollowupsVisitedFor(check).ToList();

            if (string.IsNullOrEmpty(currentRoute) || !followUpSequence.Contains(currentRoute))
            {
                return null;
            }

            for (var i = followUpSequence.Count; i >= 0; i--)
            {
                var followUp = followUpSequence[i];

                if (followUp == currentRoute)
                {
                    if (followUp == followUpSequence.First())
                    {
                        return null;
                    }

                    return GetRouteForHealthPriority(followUpSequence[i - 1]);
                }
            }

            return null;
        }
    }
}