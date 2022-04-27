using System;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{


    public class AUDIT5Model : HealthCheckPageModel
    {
        public class UnsanitisedModel
        {
            public string ConcernedContacts { get; set; }
        }

        private class SanitisedModel
        {
            public AUDITYesNotInLastYearNo ConcernedContacts { get; set; }
        }

        public UnsanitisedModel Model { get; set; }

        public string ConcernedContactsError { get; set; }

        public AUDIT5Model(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) : base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();

            Model = new UnsanitisedModel
            {
                ConcernedContacts = healthCheck?.ContactsConcernedByDrinking?.ToString()?.ToLowerInvariant(),
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
                healthCheck.ContactsConcernedByDrinking = sanitisedModel.ConcernedContacts;
            }

            await Database.SaveChangesAsync();

            return RedirectWithId("./PhysicalActivity");
        }

        SanitisedModel ValidateAndSanitise(UnsanitisedModel model)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel();

            if (string.IsNullOrEmpty(model.ConcernedContacts) || !Enum.TryParse<AUDITYesNotInLastYearNo>(model.ConcernedContacts, true, out var sanitisedConcernedContacts))
            {
                ConcernedContactsError = $"Select how often a relative or friend, doctor or other health worker has been concerned about your drinking or suggested that you cut down";
                AddError(ConcernedContactsError, "#concerned-contacts");
                isValid = false;
            }
            else
            {
                sanitisedModel.ConcernedContacts = sanitisedConcernedContacts;
            }

            return isValid ? sanitisedModel : null;
        }
    }
}