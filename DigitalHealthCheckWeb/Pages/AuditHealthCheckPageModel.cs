using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;

namespace DigitalHealthCheckWeb.Pages
{
    public class AuditHealthCheckPageModel : HealthCheckPageModel
    {
        public AuditHealthCheckPageModel(
            Database database,
            ICredentialsDecrypter credentialsDecrypter,
            IPageFlow pageFlow)
            : base(database, credentialsDecrypter, pageFlow)
        {
        }

        protected bool AllAuditQuestionsAnswered(HealthCheck check)
        {
            if (!check.DrinksAlcohol.HasValue)
            {
                return false;
            }

            if (!check.DrinksAlcohol.Value)
            {
                //Doesn't drink, no further questions
                return true;
            }

            if (!check.DrinkingFrequency.HasValue || !check.TypicalDayAlcoholUnits.HasValue || !check.MSASQ.HasValue)
            {
                return false;
            }

            if (check.MSASQ.Value < AUDITFrequency.Monthly)
            {
                //MSASQ passed, no further questions
                return true;
            }

            return check.ContactsConcernedByDrinking.HasValue &&
                check.FailedResponsibilityDueToAlcohol.HasValue &&
                check.GuiltAfterDrinking.HasValue &&
                check.InjuryCausedByDrinking.HasValue &&
                check.MemoryLossAfterDrinking.HasValue &&
                check.NeededToDrinkAlcoholMorningAfter.HasValue &&
                check.UnableToStopDrinking.HasValue;
        }
    }
}