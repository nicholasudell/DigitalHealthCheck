using System;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class SmokingModel : HealthCheckPageModel
    {
        private enum EverSmoked
        {
            Yes,
            No,
            Ex
        }

        public string Error { get; set; }

        public string Smoker { get; set; }

        public SmokingModel(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) :
            base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();
            var smokingStatus = healthCheck?.SmokingStatus;

            Smoker = smokingStatus switch
            {
                SmokingStatus.NonSmoker => "no",
                SmokingStatus.ExSmoker => "ex",
                null => null,
                _ => "yes"
            };
        }

        public async Task<IActionResult> OnPostAsync(string EverSmoked)
        {
            Smoker = EverSmoked;

            var sanitisedEverSmoked = ValidateAndSanitise(EverSmoked);

            if (sanitisedEverSmoked is null)
            {
                return await Reload();
            }

            var check = await GetHealthCheckAsync();

            if (sanitisedEverSmoked.Value == SmokingModel.EverSmoked.Yes)
            {
                if (sanitisedEverSmoked.Value == SmokingModel.EverSmoked.Yes &&
                    !(check.SmokingStatus == SmokingStatus.Heavy ||
                        check.SmokingStatus == SmokingStatus.Moderate ||
                        check.SmokingStatus == SmokingStatus.Light))
                {
                    // Their status is incongruous with their answer on this page, so reset it.
                    // Don't need to worry about this when they set non-smoker, because we change
                    // that on this page anyway.
                    check.SmokingStatus = null;
                    await Database.SaveChangesAsync();
                }

                //Because the value is actually stored on the next page,
                //we need to override the default redirect behaviour
                //for instances when you're coming from Check Your Answers
                //otherwise the user gets sent straight back to Check Your Answers from here,
                //without answering the following questions!

                return RedirectToPage("./Smoker", new
                {
                    id = UserId,
                    next = NextPage,
                    then = ThenPage
                });
            }
            else
            {
                check.SmokingStatus = sanitisedEverSmoked.Value switch
                {
                    SmokingModel.EverSmoked.No => SmokingStatus.NonSmoker,
                    SmokingModel.EverSmoked.Ex => SmokingStatus.ExSmoker
                };

                await Database.SaveChangesAsync();

                return RedirectWithId("./DoYouDrinkAlcohol");
            }
        }

        EverSmoked? ValidateAndSanitise(string value)
        {
            if (string.IsNullOrEmpty(value) || !Enum.TryParse<EverSmoked>(value, true, out var sanitisedValue))
            {
                Error = "Select yes if you smoke";
                AddError(Error, "#ever-smoked");
                return null;
            }
            else
            {
                return sanitisedValue;
            }
        }
    }
}