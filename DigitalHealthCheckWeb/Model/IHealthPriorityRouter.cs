using System.Collections.Generic;
using DigitalHealthCheckEF;

namespace DigitalHealthCheckWeb.Model
{
    /// <summary>
    /// A router to determine which follow up page to show 
    /// based on a user's results and chosen health priorities.
    /// </summary>
    public interface IHealthPriorityRouter
    {
        /// <summary>
        /// Gets all the valid follow up page routes for a person's check in order.
        /// </summary>
        /// <remarks>The resulting list will be ordered as follows:
        /// 1) The first and second priorities chosen by the user.
        /// 2) The remaining followups the user is eligible for, in order of burden of disease.
        /// </remarks>
        /// <param name="check">The user's health check.</param>
        /// <returns>A list of strings where each string is the route to a FollowUp page (e.g. './FollowUpBloodPressure')</returns>
        IEnumerable<string> AllFollowupsFor(HealthCheck check);

        /// <summary>
        /// Gets all the valid follow up page routes that a person has already been to during their Health Check.
        /// </summary>
        /// <remarks>The resulting list will be ordered as follows:
        /// 1) The first and second priorities chosen by the user.
        /// 2) The remaining followups the user is eligible for, in order of burden of disease.
        /// </remarks>
        /// <param name="check">The user's health check.</param>
        /// <returns>A list of strings where each string is the route to a FollowUp page (e.g. './FollowUpBloodPressure')</returns>
        IEnumerable<string> AllFollowupsVisitedFor(HealthCheck check);

        /// <summary>
        /// Gets the health priority name for a given page route.
        /// </summary>
        /// <param name="route">The route, e.g. './FollowUpBloodPressure'</param>
        /// <returns>The health priority name for the route, e.g. 'bloodpressure'</returns>
        string GetHealthPriorityForRoute(string route);

        /// <summary>
        /// Gets the page route for a given health priority name.
        /// </summary>
        /// <remarks>
        /// This is a reverse of <see cref="GetHealthPriorityForRoute(string)"/>.</remarks>
        /// <param name="healthPriority">The health priority, e.g. 'bloodpressure'.</param>
        /// <returns>The health priority page route, e.g. './FollowUpBloodPressure'.</returns>
        string GetRouteForHealthPriority(string healthPriority);

        /// <summary>
        /// Gets the next page in the follow up process for the user.
        /// </summary>
        /// <param name="check">The user's health check.</param>
        /// <param name="currentRoute">The current page route that the user is on.</param>
        /// <returns>The page route for the next follow up that the user should be shown.</returns>
        string NextPage(HealthCheck check, string currentRoute);

        /// <summary>
        /// Gets the previous page in the follow up process for the user.
        /// </summary>
        /// <param name="check">The user's health check.</param>
        /// <param name="currentRoute">The current page route that the user is on.</param>
        /// <returns>The page route for the previous follow up that the user was shown.</returns>
        string PreviousPage(HealthCheck check, string currentRoute);
    }
}