using DigitalHealthCheckCommon;
using DigitalHealthCheckCommon.Mail;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Helpers;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DigitalHealthCheckWeb.Pages
{
    public class Priorities
    {
        public string FirstHealthPriority { get; set; }
        public string SecondHealthPriority { get; set; }

        public string SubmitAction { get; set; }
    }

    public class ResultsModel : HealthCheckPageModel
    {
        private readonly IHealthCheckResultFactory resultFactory;
        private readonly IHealthPriorityRouter healthPriorityRouter;
        private readonly ILogger<ResultsModel> logger;
        private readonly IMailNotificationEngine mailEngine;
        private readonly IPageRenderer pageRenderer;

        public ResultsModel(
            Database database,
            ICredentialsDecrypter credentialDecrypter,
            IHealthCheckResultFactory resultFactory,
            IHealthPriorityRouter healthPriorityRouter,
            IPageFlow pageFlow,
            ILogger<ResultsModel> logger,
            IMailNotificationEngine mailEngine,
            IPageRenderer pageRenderer) 
            : base(database, credentialDecrypter, pageFlow)
        {
            this.resultFactory = resultFactory;
            this.healthPriorityRouter = healthPriorityRouter;
            this.logger = logger;
            this.mailEngine = mailEngine;
            this.pageRenderer = pageRenderer;
        }

        public Result Result { get; set; }
        public HealthCheck Check { get; set; }

        public Priorities Priorities { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if(!IsValidated())
            {
                return RedirectToValidation();
            }

            var healthCheck = await GetHealthCheckAsync();

            if (healthCheck is null)
            {
                return RedirectWithId("./Index");
            }

            if(healthCheck.GenderAffirmation != true)
            {
                // We need to send a GP copy of the results as soon as we can.
                // With those on Gender Affirmation treatment, we have one more step to confirm which
                // set of results they wish to continue with for follow ups, etc.
                // For those patients, we send on the OnPost in this Page, but everybody else we can send now.

                await SendGPEmail();
            }

            Priorities = new Priorities
            {
                FirstHealthPriority = healthCheck.FirstHealthPriorityAfterResults,
                SecondHealthPriority = healthCheck.SecondHealthPriorityAfterResults
            };

            LoadResult(healthCheck);

            return Page();
        }

        void LoadResult(HealthCheck healthCheck)
        {
            Check = healthCheck;
            logger.LogInformation($"Get results for healthcheck Id {healthCheck.Id}");
            Result = resultFactory.GetResult(healthCheck, Variant);
        }

        class PrioritiesSanitised
        {
            public string FirstHealthPriority { get; set; }
            public string SecondHealthPriority { get; set; }
        }

        public string FirstHealthPriorityErrorMessage { get; set; }
        public string SecondHealthPriorityErrorMessage { get; set; }

        PrioritiesSanitised ValidateAndSanitise(Priorities model)
        {
            var isValid = true;

            if (string.IsNullOrEmpty(model.FirstHealthPriority))
            {
                FirstHealthPriorityErrorMessage = "Select your first health priority.";
                AddError(FirstHealthPriorityErrorMessage, "#first-priority");

                isValid = false;
            }

            if (string.IsNullOrEmpty(model.SecondHealthPriority))
            {
                SecondHealthPriorityErrorMessage = "Select your second health priority.";
                AddError(SecondHealthPriorityErrorMessage, "#second-priority");
                isValid = false;
            }

            if (isValid && model.FirstHealthPriority == model.SecondHealthPriority)
            {
                FirstHealthPriorityErrorMessage = "Select two different health priorities.";
                SecondHealthPriorityErrorMessage = "Select two different health priorities.";

                AddError(FirstHealthPriorityErrorMessage, "#first-priority");

                isValid = false;
            }

            return isValid ? new PrioritiesSanitised
            {
                 FirstHealthPriority = model.FirstHealthPriority,
                 SecondHealthPriority = model.SecondHealthPriority
            } : null;
        }

        public bool Validated { get; set; }

        private async Task SendGPEmail()
        {
            var validationStatus = HttpContext.Session.GetEnum<YesNoSkip>("Validated") ?? YesNoSkip.No;

            Validated = validationStatus == YesNoSkip.Yes;

            var sentGPEmail = HttpContext.Session.GetBool("SentGPEmail") == true;

            Check = await GetHealthCheckAsync();

            if (Validated && !sentGPEmail && Check.GPEmail is not null)
            {
                await Database.Entry(Check).Collection(c => c.ChosenInterventions).LoadAsync();

                Result = resultFactory.GetResult(Check, Variant);
            
                try
                {
                    var html = await pageRenderer.RenderHtmlAsync("GPEmail", this);

                    RetryPolicy.Retry(async () =>
                    {
                        const string subject = "Digital NHS Health Check Results - GP REPORT";
                        var mailMessage = mailEngine
                            .NewMessage()
                            .To(Check.GPEmail)
                            .WithSubject(subject)
                            .WithHtmlBody(html);

                        using var message = mailMessage.Create();

                        try
                        {
                            mailEngine.SendMessage(message);

                            Check.GPEmailSent = true;

                            await Database.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            logger.LogWarning($"Email failed to send for healthcheck id {Check.Id}. Retrying...");
                            throw new EmailFailedException(Check.GPEmail, subject, ex);
                        }
                    }, new[] { typeof(EmailFailedException) }, TimeSpan.FromSeconds(1), 3, 1f);
                }
                catch (Exception ex)
                {
                    logger.LogCritical(ex, $"Failed to send a GP email for Health Check {Check.Id}.");
                }

                //Prevent spamming GPs by refreshing the page.
                HttpContext.Session.SetBool("SentGPEmail", true);
            }
        }

        public async Task<IActionResult> OnPostAsync(Priorities priorities)
        {
            Priorities = priorities;

            var healthCheck = await GetHealthCheckAsync();

            if (healthCheck is null)
            {
                return RedirectWithId("./Index");
            }

            var sanitisedModel = ValidateAndSanitise(priorities);

            if (sanitisedModel is not null)
            {
                // Store the priorities here even if they've entered them when they want to recalculate,
                // so we don't have to ask again

                healthCheck.FirstHealthPriorityAfterResults = sanitisedModel.FirstHealthPriority;
                healthCheck.SecondHealthPriorityAfterResults = sanitisedModel.SecondHealthPriority;
            }

            if (priorities.SubmitAction == "Recalculate")
            {
                Variant = true;

                if (healthCheck.Variant is null)
                {
                    healthCheck.Variant = new IdentityVariant()
                    {
                        Sex = healthCheck.SexForResults == Sex.Male ? Sex.Female : Sex.Male,
                        PolycysticOvaries = healthCheck.PolycysticOvaries,
                        GestationalDiabetes = healthCheck.GestationalDiabetes,
                        MSASQ = healthCheck.MSASQ
                    };

                    await Database.SaveChangesAsync();

                    Database.Dispose(); //Seems to force flushing this record to the table first.
                }

                var needsPolycysticOvariesAnswered = healthCheck.SexAtBirth == Sex.Female && healthCheck.Variant.Sex == Sex.Female;
                var needsDrinkAnswered = (healthCheck.Variant.Sex == Sex.Female && healthCheck.DrinksAlcohol.Value) ||
                    (healthCheck.Variant.Sex == Sex.Male && healthCheck.DrinksAlcohol == true && healthCheck.DrinkingFrequency != AUDITDrinkingFrequency.Never);

                if (needsDrinkAnswered || needsPolycysticOvariesAnswered)
                {
                    return RedirectWithId("./Recalculating");
                }
                else
                {
                    return RedirectWithId("./Calculating");
                }
            }

            LoadResult(healthCheck);

            if (sanitisedModel is null)
            {
                return await Reload();
            }

            await Database.SaveChangesAsync();

            if (priorities.SubmitAction == "GPSexAtBirth")
            {
                if(Check.SexAtBirth == Check.Variant.Sex)
                {
                    // After this point, we use the variant to store which check they did not complete the full follow up process for,
                    // therefore, we need to swap the details from the variant into the primary check and vise versa

                    SwapCheckAndVariant(Check);

                    await Database.SaveChangesAsync();
                }

                Variant = false;

                await SendGPEmail();

                return RedirectWithId("./FollowUpGP");
            }
            else if (priorities.SubmitAction == "GPCurrentGender")
            {
                if (Check.SexAtBirth != Check.Variant.Sex)
                {
                    // After this point, we use the variant to store which check they did not complete the full follow up process for,
                    // therefore, we need to swap the details from the variant into the primary check and vise versa

                    SwapCheckAndVariant(Check);

                    await Database.SaveChangesAsync();
                }

                Variant = false;

                await SendGPEmail();

                return RedirectWithId("./FollowUpGP");
            }
            else if (priorities.SubmitAction == "PrioritiesSexAtBirth")
            {
                if (Check.SexAtBirth == Check.Variant.Sex)
                {
                    // After this point, we use the variant to store which check they did not complete the full follow up process for,
                    // therefore, we need to swap the details from the variant into the primary check and vise versa

                    SwapCheckAndVariant(Check);

                    await Database.SaveChangesAsync();
                }

                Variant = false;

                await SendGPEmail();

                return RedirectWithId(healthPriorityRouter.GetRouteForHealthPriority(healthCheck.FirstHealthPriorityAfterResults));
            }
            else if (priorities.SubmitAction == "PrioritiesCurrentGender")
            {
                if (Check.SexAtBirth != Check.Variant.Sex)
                {
                    // After this point, we use the variant to store which check they did not complete the full follow up process for,
                    // therefore, we need to swap the details from the variant into the primary check and vise versa

                    SwapCheckAndVariant(Check);

                    await Database.SaveChangesAsync();
                }

                Variant = false;

                await SendGPEmail();

                return RedirectWithId(healthPriorityRouter.GetRouteForHealthPriority(healthCheck.FirstHealthPriorityAfterResults));
            }
            else if (priorities.SubmitAction == "GP")
            {
                Variant = false;
                return RedirectWithId("./FollowUpGP");
            }
            else
            {
                Variant = false;
                return RedirectWithId(healthPriorityRouter.GetRouteForHealthPriority(healthCheck.FirstHealthPriorityAfterResults));
            }
        }

        void SwapCheckAndVariant(HealthCheck check)
        {
            Swap(check.SexForResults, check.Variant.Sex, x => check.SexForResults = x, x => check.Variant.Sex = x);
            Swap(check.HeartAge, check.Variant.HeartAge, x => check.HeartAge = x, x => check.Variant.HeartAge = x);
            Swap(check.GPPAQ, check.Variant.GPPAQ, x => check.GPPAQ = x, x => check.Variant.GPPAQ = x);
            Swap(check.MSASQ, check.Variant.MSASQ, x => check.MSASQ = x, x => check.Variant.MSASQ = x);
            Swap(check.AUDIT, check.Variant.AUDIT, x => check.AUDIT = x, x => check.Variant.AUDIT = x);
            Swap(check.GAD2, check.Variant.GAD2, x => check.GAD2 = x, x => check.Variant.GAD2 = x);
            Swap(check.PolycysticOvaries, check.Variant.PolycysticOvaries, x => check.PolycysticOvaries = x, x => check.Variant.PolycysticOvaries = x);
            Swap(check.GestationalDiabetes, check.Variant.GestationalDiabetes, x => check.GestationalDiabetes = x, x => check.Variant.GestationalDiabetes = x);
            Swap(check.QDiabetes, check.Variant.QDiabetes, x => check.QDiabetes = x, x => check.Variant.QDiabetes = x);
            Swap(check.QRisk, check.Variant.QRisk, x => check.QRisk = x, x => check.Variant.QRisk = x);
        }

        void Swap<T>(T one, T two, Action<T> oneSetter, Action<T> twoSetter)
        {
            oneSetter(two);
            twoSetter(one);
        }
    }
}
