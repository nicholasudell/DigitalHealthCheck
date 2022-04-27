using System;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{


    public class AUDIT2Model : AuditHealthCheckPageModel
    {
        public class UnsanitisedModel
        {
            public string FailedResponsibility { get; set; }

            public string UnableToStop { get; set; }
        }

        private class SanitisedModel
        {
            public AUDITFrequency FailedResponsibility { get; set; }

            public AUDITFrequency UnableToStop { get; set; }
        }

        public UnsanitisedModel Model { get; set; }

        public string FailedResponsibilityError { get; set; }

        public string UnableToStopError { get; set; }

        public AUDIT2Model(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) : base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();

            Model = new UnsanitisedModel
            {
                FailedResponsibility = healthCheck?.FailedResponsibilityDueToAlcohol?.ToString()?.ToLowerInvariant(),
                UnableToStop = healthCheck?.UnableToStopDrinking?.ToString()?.ToLowerInvariant()
            };
        }

        public async Task<IActionResult> OnPostAsync(UnsanitisedModel model)
        {
            Model = model;

            var sanitisedModel = ValidateAndSanitise(model);

            if (sanitisedModel is null)
            {
                return await Reload();
            }

            var healthCheck = await GetHealthCheckAsync();

            if (healthCheck is not null)
            {
                healthCheck.FailedResponsibilityDueToAlcohol = sanitisedModel.FailedResponsibility;
                healthCheck.UnableToStopDrinking = sanitisedModel.UnableToStop;
            }

            await Database.SaveChangesAsync();

            //If we need to show more alcohol questions, we have to override the next page
            //but preserve the next page value until we don't need to show any more questions
            //that way, it'll then finally follow the redirect.

            if (!AllAuditQuestionsAnswered(healthCheck))
            {
                return RedirectToPage("./AUDIT3", new
                {
                    id = UserId,
                    next = NextPage,
                    then = ThenPage
                });
            }
            else
            {
                return RedirectWithId("./AUDIT3");
            }
        }

        SanitisedModel ValidateAndSanitise(UnsanitisedModel model)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel();

            if (string.IsNullOrEmpty(model.UnableToStop) || !Enum.TryParse<AUDITFrequency>(model.UnableToStop, true, out var sanitisedUnableToStop))
            {
                UnableToStopError = $"Select how often during the last year that you have found you were unable to stop drinking once you had started.";
                AddError(UnableToStopError, "#unable-to-stop");
                isValid = false;
            }
            else
            {
                sanitisedModel.UnableToStop = sanitisedUnableToStop;
            }

            if (string.IsNullOrEmpty(model.FailedResponsibility) || !Enum.TryParse<AUDITFrequency>(model.FailedResponsibility, true, out var sanitisedFailedResponsibility))
            {
                FailedResponsibilityError = $"Select how often you have failed to do what was normally expected from you because of your drinking in the last year";
                AddError(FailedResponsibilityError, "#failed-responsibility");
                isValid = false;
            }
            else
            {
                sanitisedModel.FailedResponsibility = sanitisedFailedResponsibility;
            }

            return isValid ? sanitisedModel : null;
        }
    }
}