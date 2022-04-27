using System;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class GenderIdentityModel : HealthCheckPageModel
    {
        public string SexForResults { get; set; }

        public string Identity { get; set; }
        public string CustomIdentity { get; set; }

        public string SexForResultsError { get; set; }

        public GenderIdentityModel
        (
            Database database,
            ICredentialsDecrypter credentialsDecrypter,
            IPageFlow pageFlow
        )
            : base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();
            SexForResults = healthCheck?.SexForResults is not null ? (healthCheck?.SexForResults == healthCheck?.SexAtBirth ? "birth" : "current") : null;
            Identity = healthCheck.Identity;
            CustomIdentity = healthCheck.CustomIdentity;
        }

        public Sex? Alternate(Sex? sex) => sex switch
        {
            Sex.Male => Sex.Female,
            Sex.Female => Sex.Male,
            _ => null
        };

        public async Task<IActionResult> OnPostAsync(string sexForResults)
        {
            SexForResults = sexForResults;

            var sanitisedSexForResults = ValidateAndSanitise(sexForResults);

            if (sanitisedSexForResults is null)
            {
                return await Reload();
            }

            var healthCheck = await GetHealthCheckAsync();

            healthCheck.SexForResults = sanitisedSexForResults == ValidResponse.Birth ? 
                healthCheck.SexAtBirth : 
                Alternate(healthCheck.SexAtBirth);

            healthCheck.InitialSexForResults = (sanitisedSexForResults == ValidResponse.Birth ? "birth" : "current");

            await Database.SaveChangesAsync();

            //If you've changed Sex from male to female you have to answer another page of questions later.
            //This is naturally shown in the normal flow, but if you've been redirected (i.e. NextPage is not null)
            //We need to redirect over to that page first, to get answers, before redirecting back.

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

        enum ValidResponse
        {
            Birth,
            Current
        }

        ValidResponse? ValidateAndSanitise(string sex)
        {
            if (string.IsNullOrEmpty(sex) || !Enum.TryParse<ValidResponse>(sex, true, out var sanitisedSexForResults))
            {
                SexForResultsError = "Select the result you wish to view first";
                AddError(SexForResultsError, "#sex");
                return null;
            }

            return sanitisedSexForResults;
        }
    }
}