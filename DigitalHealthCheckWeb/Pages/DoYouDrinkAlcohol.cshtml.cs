using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class DoYouDrinkAlcoholModel : AuditHealthCheckPageModel
    {
        public string DrinksAlcohol { get; set; }

        public string Error { get; set; }

        public DoYouDrinkAlcoholModel(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) :
            base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();
            DrinksAlcohol = (healthCheck?.DrinksAlcohol).HasValue ? (healthCheck.DrinksAlcohol.Value ? "yes" : "no") : null;
        }

        public async Task<IActionResult> OnPostAsync(string drinkAlcohol)
        {
            DrinksAlcohol = drinkAlcohol;

            var sanitisedDrinksAlcohol = ValidateAndSanitise(drinkAlcohol);

            if (sanitisedDrinksAlcohol is null)
            {
                return await Reload();
            }

            var healthCheck = await GetHealthCheckAsync();

            if (healthCheck is not null)
            {
                if (!sanitisedDrinksAlcohol.Value)
                {
                    healthCheck.DrinkingFrequency = null;
                    healthCheck.MSASQ = null;
                    healthCheck.MemoryLossAfterDrinking = null;
                    healthCheck.GuiltAfterDrinking = null;
                    healthCheck.InjuryCausedByDrinking = null;
                    healthCheck.NeededToDrinkAlcoholMorningAfter = null;
                    healthCheck.TypicalDayAlcoholUnits = null;
                    healthCheck.UnableToStopDrinking = null;
                    healthCheck.ContactsConcernedByDrinking = null;
                    healthCheck.FailedResponsibilityDueToAlcohol = null;
                }

                healthCheck.DrinksAlcohol = sanitisedDrinksAlcohol;
            }

            await Database.SaveChangesAsync();

            if (sanitisedDrinksAlcohol.Value)
            {
                //If we need to show more alcohol questions, we have to override the next page
                //but preserve the next page value until we don't need to show any more questions
                //that way, it'll then finally follow the redirect.
                if (!AllAuditQuestionsAnswered(healthCheck))
                {
                    return RedirectToPage("./DrinkingFrequency", new
                    {
                        id = UserId,
                        next = NextPage,
                        then = ThenPage
                    });
                }
                else
                {
                    return RedirectWithId("./DrinkingFrequency");
                }
            }
            else
            {
                return RedirectWithId("./PhysicalActivity");
            }
        }

        bool? ValidateAndSanitise(string value)
        {
            if (string.IsNullOrEmpty(value) || (value != "yes" && value != "no"))
            {
                Error = "Select yes if you drink alcohol";
                AddError(Error, "#drink-alcohol");

                return null;
            }

            return value == "yes";
        }
    }
}