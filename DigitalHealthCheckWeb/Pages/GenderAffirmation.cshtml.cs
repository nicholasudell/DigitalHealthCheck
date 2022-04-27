using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Helpers;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class GenderAffirmationModel : HealthCheckPageModel
    {
        public class UnsanitisedModel
        {

            public string GenderAffirmation { get; set; }
        }

        private class SanitisedModel
        {
            public bool GenderAffirmation { get; set; }
        }

        public string Identity { get; set; }
        public string CustomIdentity { get; set; }

        public UnsanitisedModel Model { get; set; }

        public string GenderAffirmationError { get; set; }

        public GenderAffirmationModel(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) : 
            base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();

            Model = new UnsanitisedModel
            {
                GenderAffirmation = healthCheck?.GenderAffirmation.AsYesNoOrNull()
            };

            Identity = healthCheck?.Identity;
            CustomIdentity = healthCheck?.CustomIdentity;
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
                healthCheck.GenderAffirmation = sanitisedModel.GenderAffirmation;

                if (!sanitisedModel.GenderAffirmation)
                {
                    healthCheck.SexForResults = healthCheck.SexAtBirth;
                }
            }

            await Database.SaveChangesAsync();

            if (sanitisedModel.GenderAffirmation)
            {
                if(NextPage is not null)
                {
                    if (healthCheck.SexAtBirth == Sex.Female &&
                    (Variant ? healthCheck.Variant.Sex : healthCheck.SexForResults) == Sex.Female &&
                (healthCheck.GestationalDiabetes == null || healthCheck.PolycysticOvaries == null))
                    {
                        return RedirectToPage("./PolycysticOvariesAndGestationalDiabetes", new
                        {
                            id = UserId,
                            next = NextPage,
                            then = ThenPage,
                            variant = Variant
                        });
                    }
                }

                return RedirectWithId("./GenderIdentity");
            }
            else
            {
                if (NextPage is not null)
                {
                    if (healthCheck.SexAtBirth == Sex.Female &&
                    (Variant ? healthCheck.Variant.Sex : healthCheck.SexForResults) == Sex.Female &&
                (healthCheck.GestationalDiabetes == null || healthCheck.PolycysticOvaries == null))
                    {
                        return RedirectToPage("./PolycysticOvariesAndGestationalDiabetes", new
                        {
                            id = UserId,
                            next = NextPage,
                            then = ThenPage,
                            variant = Variant
                        });
                    }
                }

                return RedirectWithId("./Ethnicity");
            }
        }

        SanitisedModel ValidateAndSanitise(UnsanitisedModel model)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel();

            if (!model.GenderAffirmation.SanitiseYesNo(out var sanitisedGenderAffirmation))
            {
                GenderAffirmationError = "Select whether you are currently undergoing or have undergone gender affirmation treatment";
                AddError(GenderAffirmationError, "#genderAffirmation");

                isValid = false;
            }
            else
            {
                sanitisedModel.GenderAffirmation = sanitisedGenderAffirmation;
            }

            return isValid ? sanitisedModel : null;
        }
    }
}
