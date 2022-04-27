using System;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class SexModel : HealthCheckPageModel
    {
        public class UnsanitisedModel
        {
            public string Identity { get; set; }

            public string CustomIdentity { get; set; }

            public string Sex { get; set; }
        }

        private class SanitisedModel
        {
            public string Identity { get; set; }

            public string CustomIdentity { get; set; }

            public Sex Sex { get; set; }
        }

        public string IdentityError { get; set; }

        public string CustomIdentityError { get; set; }

        public UnsanitisedModel Model { get; set; }

        public string SexError { get; set; }

        public SexModel(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) :
            base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();

            Model = new UnsanitisedModel
            {
                Sex = healthCheck?.SexAtBirth?.ToString().ToLowerInvariant(),
                Identity = healthCheck?.Identity,
                CustomIdentity = healthCheck?.CustomIdentity
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
                healthCheck.SexAtBirth = sanitisedModel.Sex;

                if (healthCheck.SexAtBirth == Sex.Male)
                {
                    healthCheck.GestationalDiabetes = null;
                    healthCheck.PolycysticOvaries = null;
                }

                healthCheck.Identity = sanitisedModel.Identity;
                healthCheck.CustomIdentity = sanitisedModel.CustomIdentity;

                if (sanitisedModel.Identity == "cis")
                {
                    healthCheck.SexForResults = sanitisedModel.Sex;
                    healthCheck.GenderAffirmation = null;
                }
            }

            await Database.SaveChangesAsync();

            if (sanitisedModel.Identity != "cis")
            {
                if (NextPage is not null)
                {
                    if (healthCheck.GenderAffirmation is null)
                    {
                        return RedirectToPage("./GenderAffirmation", new
                        {
                            id = UserId,
                            next = NextPage,
                            then = ThenPage,
                            variant = Variant
                        });
                    }
                    else
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
                }

                return RedirectWithId("./GenderAffirmation");
            }
            else
            {
                //If you've changed Sex from male to female you have to answer another page of questions later.
                //This is naturally shown in the normal flow, but if you've been redirected (i.e. NextPage is not null)
                //We need to redirect over to that page first, to get answers, before redirecting back.

                if (!string.IsNullOrEmpty(NextPage) &&
                    sanitisedModel.Sex == Sex.Female &&
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

                return RedirectWithId("./Ethnicity");
            }
        }

        SanitisedModel ValidateAndSanitise(UnsanitisedModel model)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel();

            if (string.IsNullOrEmpty(model.Sex) || !Enum.TryParse<Sex>(model.Sex, true, out var sanitisedSex))
            {
                SexError = "Select your sex at birth";
                AddError(SexError, "#sex");

                isValid = false;
            }
            else
            {
                sanitisedModel.Sex = sanitisedSex;
            }

            if (string.IsNullOrEmpty(model.Identity))
            {
                IdentityError = "Specify your gender identity.";
                AddError(IdentityError, "#identity");

                isValid = false;
            }
            else
            {
                sanitisedModel.Identity = model.Identity;
            }

            if (sanitisedModel.Identity == "custom")
            {
                if (string.IsNullOrEmpty(model.CustomIdentity))
                {
                    CustomIdentityError = "Enter your gender identity.";
                    AddError(CustomIdentityError, "#custom-identity");

                    isValid = false;
                }
                else
                {
                    sanitisedModel.CustomIdentity = model.CustomIdentity;
                }
            }

            return isValid ? sanitisedModel : null;
        }
    }
}