using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using DigitalHealthCheckWeb.Model.Risks;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class RecalculatingModel : HealthCheckPageModel
    {
        private readonly IRiskScoreService riskScoreService;

        public bool DrinksAlcohol { get; set; }

        public Sex Sex { get; set; }

        public bool CurrentGender { get; set; }

        public RecalculatingModel(
            Database database,
            ICredentialsDecrypter credentialsDecrypter,
            IPageFlow pageFlow,
            IRiskScoreService riskScoreService) :
            base(database, credentialsDecrypter, pageFlow) => this.riskScoreService = riskScoreService;

        public async Task OnGetAsync()
        {
            var check = await GetHealthCheckAsync();

            Sex = Variant? check.Variant.Sex.Value : check.SexForResults.Value;
            CurrentGender = check.SexAtBirth != Sex;
            
            // If checking sex as male and previously set a never drinking frequency,
            // the higher unit threshold answer for MSASQ on AUDIT1 will still be Never so we don't need to ask
            // However, if we're checking sex as female, because the unit threshold has lowered, we still need to ask.
            DrinksAlcohol = (Sex == Sex.Female && check.DrinksAlcohol.Value) || 
                (Sex == Sex.Male && check.DrinksAlcohol == true && check.DrinkingFrequency != AUDITDrinkingFrequency.Never);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var check = await GetHealthCheckAsync();

            Sex = Variant ? check.Variant.Sex.Value : check.SexForResults.Value;

            if (Sex == Sex.Male)
            {
                if (check.DrinksAlcohol == true && check.DrinkingFrequency != AUDITDrinkingFrequency.Never)
                {
                    return RedirectWithId("./AUDIT1", next:"./CheckYourAnswers");
                }
                else
                {
                    check.Variant.MSASQ = check.MSASQ;

                    await Database.SaveChangesAsync();

                    return RedirectWithId("./Calculating");
                }
            }
            else
            {
                var polycysticOvaries = Variant ? check.Variant.PolycysticOvaries : check.PolycysticOvaries;
                var gestationalDiabetes = Variant ? check.Variant.GestationalDiabetes : check.GestationalDiabetes;

                if (check.SexAtBirth == Sex.Female && (polycysticOvaries is null || gestationalDiabetes is null))
                {
                    if (check.DrinksAlcohol == true)
                    {
                        return RedirectWithId("./AUDIT1", next: "./PolycysticOvariesAndGestationalDiabetes", then: "./CheckYourAnswers");
                    }
                    else
                    {
                        check.Variant.MSASQ = check.MSASQ;

                        await Database.SaveChangesAsync();

                        return RedirectWithId("./PolycysticOvariesAndGestationalDiabetes", next: "./CheckYourAnswers");
                    }
                }
                else
                {
                    if (check.DrinksAlcohol == true)
                    {
                        return RedirectWithId("./AUDIT1", next: "./CheckYourAnswers");
                    }
                    else
                    {
                        check.Variant.MSASQ = check.MSASQ;

                        await Database.SaveChangesAsync();

                        return RedirectWithId("./Calculating");
                    }
                }
            }
        }
    }
}