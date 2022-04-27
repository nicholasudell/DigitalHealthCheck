using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;
using static DigitalHealthCheckWeb.Components.Pages.EthnicityPageDetailed;

namespace DigitalHealthCheckWeb.Pages
{
    public class EthnicityModel : HealthCheckPageModel
    {
        public class EthnicGrouping
        {
            public IEnumerable<EthnicityItem> DetailedItems { get; set; }

            public string Legend { get; set; }

            public string Value { get; set; }
        }

        readonly IEnumerable<EthnicGrouping> ethnicGroups = new[]
        {
            new EthnicGrouping
            {
                Value = "white",
                DetailedItems = new[]
                {
                    new EthnicityItem { Text="English, Welsh, Scottish, Northern Irish or British", Value= "whitebritish"},
                    new EthnicityItem { Text="Irish", Value= "irish"},
                    new EthnicityItem { Text="Gypsy or Irish Traveller", Value= "gypsyortraveller"},
                    new EthnicityItem { Text="Any other White background", Value= "whiteother"}
                },
                 Legend = "Which of the following best describes your White background?"
            },
            new EthnicGrouping
            {
                Value = "mixed",
                DetailedItems = new[]
            {
                new EthnicityItem { Text="White and Black Caribbean", Value= "whiteblackcaribbean"},
                new EthnicityItem { Text="White and Black African", Value= "whiteblackafrican"},
                new EthnicityItem { Text="White and Asian", Value= "whiteasian"},
                new EthnicityItem { Text="Any other Mixed or Multiple ethnic background", Value= "mixedother"}
            },
                 Legend = "Which of the following best describes your Mixed or Multiple ethnic groups background?"
            },
            new EthnicGrouping
            {
                Value = "asian",
                DetailedItems =new[]
            {
                new EthnicityItem { Text="Indian", Value= "indian"},
                new EthnicityItem { Text="Pakistani", Value= "pakistani"},
                new EthnicityItem { Text="Bangladeshi", Value= "bangladeshi"},
                new EthnicityItem { Text="Chinese", Value= "chinese"},
                new EthnicityItem { Text="Any other Asian background", Value= "asianother"}
            },
                 Legend = "Which of the following best describes your Asian or Asian British background?"
            },
            new EthnicGrouping
            {
                Value = "black",
                DetailedItems = new[]
            {
                new EthnicityItem { Text="African", Value= "african"},
                new EthnicityItem { Text="Caribbean", Value= "caribbean"},
                new EthnicityItem { Text="Any other Black, African or Caribbean background", Value= "blackother"}
            },
                 Legend = "Which of the following best describes your Black, African, Caribbean or Black British background?"
            },
            new EthnicGrouping
            {
                Value = "other",
                DetailedItems = new[]
            {
                new EthnicityItem { Text="Arab", Value= "arab"},
                new EthnicityItem { Text="Any other ethnic group", Value= "otherother"}
            },
                 Legend = "Which of the following best describes your background?"
            }
        };

        public EthnicGrouping EthnicGroup { get; set; }

        public string EthnicGroupValue { get; set; }

        public string Ethnicity { get; set; }

        public string EthnicityError { get; set; }

        [BindProperty]
        public bool ShowGroup { get; set; }

        public EthnicityModel(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) : base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();
            Ethnicity = healthCheck?.Ethnicity?.ToString().ToLowerInvariant();
            EthnicGroup = ethnicGroups.FirstOrDefault(x => x.DetailedItems.Any(y => y.Value == Ethnicity));
            EthnicGroupValue = EthnicGroup?.Value;
            ShowGroup = true;
        }

        public async Task<IActionResult> OnPostAsync(string ethnicity, string ethnicGroup)
        {
            if (ShowGroup)
            {
                return await OnPostGroup(ethnicity, ethnicGroup);
            }
            else
            {
                return await OnPostEthnicityAsync(ethnicity, ethnicGroup);
            }
        }

        public async Task<IActionResult> OnPostEthnicityAsync(string ethnicity, string ethnicGroup)
        {
            Ethnicity = ethnicity;

            EthnicGroupValue = ethnicGroup;

            EthnicGroup = ValidateAndSanitizeEthnicGroup(ethnicGroup);

            var sanitisedEthnicity = ValidateAndSanitise(ethnicity);

            if (sanitisedEthnicity is null)
            {
                return await Reload();
            }

            var healthCheck = await GetHealthCheckAsync();

            if (healthCheck is not null)
            {
                healthCheck.Ethnicity = sanitisedEthnicity;
            }

            await Database.SaveChangesAsync();

            return RedirectWithId("./Smoking");
        }

        public async Task<IActionResult> OnPostGroup(string ethnicity, string ethnicGroup)
        {
            //This would only really be populated when returning and progressing from the same ethnic group.
            Ethnicity = ethnicity;

            EthnicGroupValue = ethnicGroup;

            EthnicGroup = ValidateAndSanitizeEthnicGroup(ethnicGroup);

            if (EthnicGroup == null)
            {
                return await Reload();
            }

            ShowGroup = false;

            return await Reload();
        }

        Ethnicity? ValidateAndSanitise(string ethnicity)
        {
            if (string.IsNullOrEmpty(ethnicity) || !Enum.TryParse<Ethnicity>(ethnicity, true, out var sanitisedEthnicity))
            {
                EthnicityError = "Select the option that best describes your background";
                AddError(EthnicityError, "#ethnicity");

                return null;
            }

            return sanitisedEthnicity;
        }

        EthnicGrouping ValidateAndSanitizeEthnicGroup(string ethnicGroup)
        {
            var group = ethnicGroups.FirstOrDefault(x => x.Value == ethnicGroup);

            if (group == null)
            {
                EthnicityError = "Select your ethnic group";
                AddError(EthnicityError, "#ethnic-group");
            }

            return group;
        }
    }
}