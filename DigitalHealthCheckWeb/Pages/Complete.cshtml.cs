using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Helpers;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class CompleteModel : HealthCheckPageModel
    {
        public class UnsanitisedModel
        {
            public IEnumerable<string> Contact { get; set; }

            public string EmailAddress { get; set; }

            public string PhoneNumber { get; set; }

            public string SmsNumber { get; set; }
        }

        private class SanitisedModel
        {
            public string EmailAddress { get; set; }

            public string PhoneNumber { get; set; }

            public string SmsNumber { get; set; }
        }

        private enum ContactMethod
        {
            Email,
            Phone,
            SMS
        }

        public UnsanitisedModel Model { get; set; }

        public string EmailAddressError { get; set; }

        public string PhoneNumberError { get; set; }

        public string SmsNumberError { get; set; }

        public CompleteModel(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) : base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();

            var email = healthCheck?.EmailAddress?.ToString();
            var sms = healthCheck?.SmsNumber?.ToString();
            var phone = healthCheck?.PhoneNumber?.ToString();

            var contact = new List<string>();

            if (!string.IsNullOrEmpty(email))
            {
                contact.Add(ContactMethod.Email.ToString().ToLower());
            }
            if (!string.IsNullOrEmpty(sms))
            {
                contact.Add(ContactMethod.SMS.ToString().ToLower());
            }
            if (!string.IsNullOrEmpty(phone))
            {
                contact.Add(ContactMethod.Phone.ToString().ToLower());
            }

            Model = new UnsanitisedModel
            {
                EmailAddress = email,
                SmsNumber = sms,
                PhoneNumber = phone,
                Contact = contact
            };
        }

        public async Task<IActionResult> OnPostAsync(UnsanitisedModel model)
        {
            Model = model;

            var healthCheck = await GetHealthCheckAsync();

            var sanitisedModel = await ValidateAndSanitise(healthCheck, model);

            if (sanitisedModel is null)
            {
                await Database.SaveChangesAsync();
                return await Reload();
            }

            if (healthCheck is not null)
            {
                healthCheck.EmailAddress = sanitisedModel.EmailAddress;
                healthCheck.PhoneNumber = sanitisedModel.PhoneNumber;
                healthCheck.SmsNumber = sanitisedModel.SmsNumber;
                healthCheck.CompletePageSubmitted = true;
            }

            await Database.SaveChangesAsync();

            return RedirectWithId("./Validation");
        }

        async Task<SanitisedModel> ValidateAndSanitise(HealthCheck check, UnsanitisedModel model)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel();

            var sanitisedContactMethods = new Collection<ContactMethod>();

            if (model.Contact != null)
            {
                foreach (var contactMethod in model.Contact)
                {
                    if (contactMethod.SanitiseEnum<ContactMethod>(out var sanitisedContactMethod))
                    {
                        sanitisedContactMethods.Add(sanitisedContactMethod.Value);
                    }
                }
            }

            if (sanitisedContactMethods.Contains(ContactMethod.Email))
            {
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
            }

            if (sanitisedContactMethods.Contains(ContactMethod.Phone))
            {
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
            }

            if (sanitisedContactMethods.Contains(ContactMethod.SMS))
            {
                if (string.IsNullOrEmpty(model.SmsNumber))
                {
                    SmsNumberError = "Enter a UK mobile telephone number";
                    AddError(SmsNumberError, "#contact-by-text");
                    isValid = false;
                }
                else if (!model.SmsNumber.IsValidPhoneNumber())
                {
                    SmsNumberError = "Enter a UK mobile telephone number, like 07700 900 000, or +44 7700 900 045";
                    await AddError(check, SmsNumberError, "#contact-by-text");
                    isValid = false;
                }
                else
                {
                    sanitisedModel.SmsNumber = model.SmsNumber;
                }
            }

            return isValid ? sanitisedModel : null;
        }
    }
}