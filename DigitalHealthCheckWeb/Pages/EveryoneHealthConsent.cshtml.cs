using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Helpers;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class EveryoneHealthConsentModel : HealthCheckPageModel
    {
        public class UnsanitisedModel
        {
            public string EmailAddress { get; set; }

            public string PhoneNumber { get; set; }

            public IEnumerable<string> ContactMethod { get; set; }
        }

        private class SanitisedModel
        {
            public string EmailAddress { get; set; }

            public string PhoneNumber { get; set; }

            public bool? ConsentToContact { get; set; }
            public bool? WillContactMyself { get; set; }
        }

        public UnsanitisedModel Model { get; set; }

        public string EmailAddressError { get; set; }

        public string PhoneNumberError { get; set; }

        public bool? ConsentToContact { get; set; }
        public bool? WillContactMyself { get; set; }

        private readonly IEveryoneHealthReferralService everyoneHealthReferralService;

        public string Error { get; set; }

        public IList<string> ReferralInterventions { get; set; }

        public EveryoneHealthConsentModel(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow, IEveryoneHealthReferralService everyoneHealthReferralService) :
            base(database, credentialsDecrypter, pageFlow)
        {
            this.everyoneHealthReferralService = everyoneHealthReferralService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!IsValidated())
            {
                return RedirectToValidation();
            }

            var healthCheck = await GetHealthCheckAsync();

            await Database.Entry(healthCheck).Collection(c => c.ChosenInterventions).LoadAsync();

            ConsentToContact = healthCheck?.EveryoneHealthConsent;
            WillContactMyself = healthCheck?.PrefersToContactEveryoneHealth;
            ReferralInterventions = everyoneHealthReferralService.GetEveryoneHealthReferrals(healthCheck).Select(x=> x.Text).Distinct().ToList();

            var email = healthCheck?.EmailAddress?.ToString();
            var phone = healthCheck?.PhoneNumber?.ToString();

            Model = new UnsanitisedModel
            {
                EmailAddress = email,
                PhoneNumber = phone,
            };

            return Page();
        }

        protected override async Task<IActionResult> Reload()
        {
            var healthCheck = await GetHealthCheckAsync();

            await Database.Entry(healthCheck).Collection(c => c.ChosenInterventions).LoadAsync();

            ReferralInterventions = everyoneHealthReferralService.GetEveryoneHealthReferrals(healthCheck).Select(x => x.Text).Distinct().ToList();

            return await base.Reload();
        }

        public async Task<IActionResult> OnPostAsync(UnsanitisedModel model)
        {
            Model = model;

            ConsentToContact = model.ContactMethod?.Contains("contactMe") == true;
            WillContactMyself = model.ContactMethod?.Contains("giveMeNumber") == true;

            var healthCheck = await GetHealthCheckAsync();

            var sanitisedModel = await ValidateAndSanitise(healthCheck, model);

            if (sanitisedModel is null)
            {
                await Database.SaveChangesAsync();
                return await Reload();
            }

            if (healthCheck is not null)
            {
                healthCheck.EveryoneHealthConsent = ConsentToContact;
                healthCheck.PrefersToContactEveryoneHealth = WillContactMyself;

                if(ConsentToContact == true)
                {
                    healthCheck.EmailAddress = sanitisedModel.EmailAddress;
                    healthCheck.PhoneNumber = sanitisedModel.PhoneNumber;
                }
            }

            await Database.SaveChangesAsync();

            return RedirectWithId("./HealthCheckComplete");
        }

        async Task<SanitisedModel> ValidateAndSanitise(HealthCheck check, UnsanitisedModel model)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel
            {
                ConsentToContact = model.ContactMethod?.Contains("contactMe") == true,
                WillContactMyself = model.ContactMethod?.Contains("giveMeNumber") == true
            };

            if (!(sanitisedModel.ConsentToContact == true))
            {
                return sanitisedModel;
            }

            if (string.IsNullOrEmpty(model.EmailAddress))
            {
                EmailAddressError = "Enter your email address";
                AddError(EmailAddressError, "#contact-by-email");
                isValid = false;
            }
            else if (!model.EmailAddress.IsValidEmail())
            {
                EmailAddressError = "Enter an email address in the correct format, like name@example.com";
                await AddError(check, EmailAddressError, "#contact-by-email");
                isValid = false;
            }
            else
            {
                sanitisedModel.EmailAddress = model.EmailAddress;
            }

            if (string.IsNullOrEmpty(model.PhoneNumber))
            {
                PhoneNumberError = "Enter a UK telephone number";
                AddError(PhoneNumberError, "#contact-by-phone");
                isValid = false;
            }
            else if (!model.PhoneNumber.IsValidPhoneNumber())
            {
                PhoneNumberError = "Enter a UK telephone number, like 01632 960 001, 07700 900 982 or +44 808 157 0192";
                await AddError(check, PhoneNumberError, "#contact-by-phone");
                isValid = false;
            }
            else
            {
                sanitisedModel.PhoneNumber = model.PhoneNumber;
            }
            
            return isValid ? sanitisedModel : null;
        }
    }
}