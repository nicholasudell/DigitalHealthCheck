using System.Linq;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Helpers;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class IndexModel : HealthCheckPageModel
    {
        private readonly IUrlBuilder urlBuilder;

        public IndexModel(
            IUrlBuilder urlBuilder,
            Database database,
            ICredentialsDecrypter credentialsDecrypter,
            IPageFlow pageFlow) :
            base(database, credentialsDecrypter, pageFlow)
        {
            this.urlBuilder = urlBuilder;
        }

        public bool IsFirstTime { get; set; }
        public string ContinuePage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var arrivedWithId = !string.IsNullOrEmpty(Id);

            var check = await GetOrCreateHealthCheck();

            if(arrivedWithId)
            {
                IsFirstTime = check?.ClickedStartNow != true;

                if (!IsFirstTime)
                {
                    ContinuePage = FirstUnansweredPage(check);
                }

                return Page();
            }

            // Using a redirect here to force the new Id query parameter to be part of the URL from now on.

            // Because of a bit of a faff with how Blazor manages URLs and trailing slashes
            // (and how the rest of ASP.NET is eager to ignore those trailing slashes)
            // We have to build the URL ourselves.

            var baseUrl = urlBuilder.GetBaseUrl(HttpContext.Request);

            if(!baseUrl.EndsWith('/'))
            {
                baseUrl += "/";
            }

            return Redirect($"{baseUrl}?id={Id}");

        }

        public async Task<IActionResult> OnPost()
        {
            var check = await GetHealthCheckAsync();

            check.ClickedStartNow = true;

            await Database.SaveChangesAsync();

            return RedirectToPage("./HeightAndWeight", new
            {
                id = UserId,
                hash = Hash
            });
        }


        private static string FirstUnansweredPage(HealthCheck check)
        {
            static string PageForNulls(string page, params object[] values)
                => values.Any(x => x is null) ? page : null;

            return PageForNulls("HeightAndWeight", check.Height, check.Weight) ??
                PageForNulls("Sex", check.SexForResults) ??
                (check.Identity != "cis" ?
                (
                    PageForNulls("GenderAffirmation", check.GenderAffirmation)
                ) : null) ??
                (check.GenderAffirmation == true ?
                (
                    PageForNulls("GenderIdentity", check.GenderAffirmation)
                ) : null) ??
                PageForNulls("Ethnicity", check.Ethnicity) ??
                PageForNulls("Smoking", check.SmokingStatus) ??
                PageForNulls("DoYouDrinkAlcohol", check.DrinksAlcohol) ??
                (check.DrinksAlcohol.Value ?
                (
                    PageForNulls("DrinkingFrequency", check.DrinkingFrequency) ??
                    PageForNulls("AUDIT1", check.TypicalDayAlcoholUnits, check.MSASQ) ??
                    (((int)(check.MSASQ.Value) > (int)AUDITFrequency.LessThanMonthly) ?
                    (
                        PageForNulls("AUDIT2", check.UnableToStopDrinking, check.FailedResponsibilityDueToAlcohol) ??
                        PageForNulls("AUDIT3", check.NeededToDrinkAlcoholMorningAfter, check.GuiltAfterDrinking) ??
                        PageForNulls("AUDIT4", check.MemoryLossAfterDrinking, check.InjuryCausedByDrinking) ??
                        PageForNulls("AUDIT5", check.ContactsConcernedByDrinking)
                    ) : null)
                ) : null) ??
                PageForNulls("PhysicalActivity", check.WorkActivity) ??
                PageForNulls("GPPAQ1", check.PhysicalActivity, check.Cycling) ??
                PageForNulls("GPPAQ2", check.Housework, check.Gardening) ??
                PageForNulls("GPPAQ3", check.Walking, check.WalkingPace) ??
                PageForNulls("Diabetes", check.FamilyHistoryDiabetes, check.Steroids) ??
                (check.SexForResults.Value == Sex.Female && check.SexAtBirth.Value == Sex.Female ?
                (
                    PageForNulls("PolycysticOvariesAndGestationalDiabetes", check.PolycysticOvaries, check.GestationalDiabetes)
                ) : null) ??
                PageForNulls("BloodSugar", check.KnowYourHbA1c) ??
                PageForNulls("RiskFactors1", check.FamilyHistoryCVD, check.ChronicKidneyDisease, check.AtrialFibrillation) ??
                PageForNulls("RiskFactors2", check.BloodPressureTreatment, check.Migraines, check.RheumatoidArthritis) ??
                PageForNulls("RiskFactors3", check.SystemicLupusErythematosus, check.SevereMentalIllness) ??
                (check.SevereMentalIllness.Value == YesNoSkip.Yes ?
                (
                    PageForNulls("RiskFactors3", check.AtypicalAntipsychoticMedication)
                ) : null) ??
                PageForNulls("BloodPressure", check.KnowYourBloodPressure) ??
                PageForNulls("Cholesterol", check.KnowYourCholesterol) ??
                (check.SkipMentalHealthQuestions != true ?
                (
                    PageForNulls("MentalWellbeing", check.UnderCare, check.Anxious, check.Control, check.Disinterested, check.FeelingDown)
                ) : null) ??
                PageForNulls("Validation", check.Postcode, check.DateOfBirth, check.Surname) ??
                "CheckYourAnswers";
        }
    }
}