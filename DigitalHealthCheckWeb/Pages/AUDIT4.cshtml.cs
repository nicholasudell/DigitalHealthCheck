using System;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class AUDIT4Model : AuditHealthCheckPageModel
    {
        public class UnsanitisedModel
        {
            public string Injured { get; set; }

            public string MemoryLoss { get; set; }
        }
        private class SanitisedModel
        {
            public AUDITYesNotInLastYearNo Injured { get; set; }

            public AUDITFrequency MemoryLoss { get; set; }
        }

        public UnsanitisedModel Model { get; set; }

        public string InjuredError { get; set; }

        public string MemoryLossError { get; set; }

        public AUDIT4Model(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) : base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();

            Model = new UnsanitisedModel
            {
                Injured = healthCheck?.InjuryCausedByDrinking?.ToString()?.ToLowerInvariant(),
                MemoryLoss = healthCheck?.MemoryLossAfterDrinking?.ToString()?.ToLowerInvariant()
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
                healthCheck.InjuryCausedByDrinking = sanitisedModel.Injured;
                healthCheck.MemoryLossAfterDrinking = sanitisedModel.MemoryLoss;
            }

            await Database.SaveChangesAsync();

            //If we need to show more alcohol questions, we have to override the next page
            //but preserve the next page value until we don't need to show any more questions
            //that way, it'll then finally follow the redirect.

            if (!AllAuditQuestionsAnswered(healthCheck))
            {
                return RedirectToPage("./AUDIT5", new
                {
                    id = UserId,
                    next = NextPage,
                    then = ThenPage
                });
            }
            else
            {
                return RedirectWithId("./AUDIT5");
            }
        }

        SanitisedModel ValidateAndSanitise(UnsanitisedModel model)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel();

            if (string.IsNullOrEmpty(model.MemoryLoss) || !Enum.TryParse<AUDITFrequency>(model.MemoryLoss, true, out var sanitisedMemoryLoss))
            {
                MemoryLossError = $"Select how often you have been unable to remember what happened the night before because you had been drinking";
                AddError(MemoryLossError, "#memory-loss");
                isValid = false;
            }
            else
            {
                sanitisedModel.MemoryLoss = sanitisedMemoryLoss;
            }

            if (string.IsNullOrEmpty(model.Injured) || !Enum.TryParse<AUDITYesNotInLastYearNo>(model.Injured, true, out var sanitisedInjured))
            {
                InjuredError = $"Select how often you or somebody else has been injured as a result of your drinking";
                AddError(InjuredError, "#injured");
                isValid = false;
            }
            else
            {
                sanitisedModel.Injured = sanitisedInjured;
            }

            return isValid ? sanitisedModel : null;
        }
    }
}