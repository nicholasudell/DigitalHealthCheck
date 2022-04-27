using System;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class AUDIT1Model : AuditHealthCheckPageModel
    {
        public class UnsanitisedModel
        {
            public string MSASQ { get; set; }

            public string Units { get; set; }
        }

        private class SanitisedModel
        {
            public AUDITFrequency MSASQ { get; set; }

            public int Units { get; set; }
        }

        public UnsanitisedModel Model { get; set; }

        public string MSASQError { get; set; }

        public Sex? Sex { get; set; }

        public string UnitsError { get; set; }

        public AUDIT1Model(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow)
            : base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();

            if (healthCheck?.SexForResults is null)
            {
                //We need this information before we can offer this page because we tailor unit numbers to sex.

                return RedirectWithId("./Sex", next: "./AUDIT1");
            }

            Model = new UnsanitisedModel
            {
                Units = healthCheck?.TypicalDayAlcoholUnits.ToString(),
                MSASQ = (Variant? healthCheck?.Variant?.MSASQ : healthCheck?.MSASQ)?.ToString().ToLowerInvariant()
            };

            Sex = Variant? healthCheck?.Variant.Sex : healthCheck?.SexForResults;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(UnsanitisedModel model)
        {
            Model = model;

            var healthCheck = await GetHealthCheckAsync();

            var sanitisedModel = ValidateAndSanitise(model, CalculateUnits(healthCheck));

            if (sanitisedModel is null)
            {
                return await Reload();
            }

            if (healthCheck is not null)
            {
                if (healthCheck.MSASQ.HasValue && healthCheck.MSASQ.Value < AUDITFrequency.Monthly && sanitisedModel.MSASQ >= AUDITFrequency.Monthly)
                {
                    healthCheck.MemoryLossAfterDrinking = null;
                    healthCheck.GuiltAfterDrinking = null;
                    healthCheck.InjuryCausedByDrinking = null;
                    healthCheck.NeededToDrinkAlcoholMorningAfter = null;
                    healthCheck.UnableToStopDrinking = null;
                    healthCheck.ContactsConcernedByDrinking = null;
                    healthCheck.FailedResponsibilityDueToAlcohol = null;
                }

                healthCheck.TypicalDayAlcoholUnits = sanitisedModel.Units;
                healthCheck.MSASQ = sanitisedModel.MSASQ;
            }

            await Database.SaveChangesAsync();

            if (sanitisedModel.MSASQ == AUDITFrequency.Never || sanitisedModel.MSASQ == AUDITFrequency.LessThanMonthly)
            {
                return RedirectWithId("./PhysicalActivity");
            }
            else
            {
                //If we need to show more alcohol questions, we have to override the next page redirect behaviour
                //but preserve the next page value until we don't need to show any more questions
                //that way, it'll then finally follow the redirect.

                if (!AllAuditQuestionsAnswered(healthCheck))
                {
                    return RedirectToPage("./AUDIT2", new
                    {
                        id = UserId,
                        next = NextPage,
                        then = ThenPage
                    });
                }
                else
                {
                    return RedirectWithId("./AUDIT2");
                }
            }
        }

        private static int CalculateUnits(HealthCheck healthCheck) =>
            healthCheck?.SexForResults switch
            {
                DigitalHealthCheckEF.Sex.Female => 6,
                DigitalHealthCheckEF.Sex.Male => 8,
                _ => 8
            };

        SanitisedModel ValidateAndSanitise(UnsanitisedModel model, int unitCount)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel();

            if (string.IsNullOrEmpty(model.Units))
            {
                UnitsError = "Choose the number of units of alcohol you drink on a typical day when you are drinking.";
                AddError(UnitsError, "#units");
                isValid = false;
            }
            else if (!int.TryParse(model.Units, out var unitsSanitised))
            {
                UnitsError = "The number of units must be a whole number, like 6";
                AddError(UnitsError, "#units");
                isValid = false;
            }
            else
            {
                sanitisedModel.Units = unitsSanitised;
            }

            if (string.IsNullOrEmpty(model.MSASQ) || !Enum.TryParse<AUDITFrequency>(model.MSASQ, true, out var sanitisedMSASQ))
            {
                MSASQError = $"Select how often you have had {unitCount} or more units of alcohol on a single occasion in the last year";
                AddError(MSASQError, "#msasq");
                isValid = false;
            }
            else
            {
                sanitisedModel.MSASQ = sanitisedMSASQ;
            }

            return isValid ? sanitisedModel : null;
        }
    }
}