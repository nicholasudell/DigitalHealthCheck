using DigitalHealthCheckCommon;
using DigitalHealthCheckCommon.Mail;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Helpers;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalHealthCheckWeb.Pages
{

    public class HealthCheckCompleteModel : HealthCheckPageModel
    {
        private readonly IPageRenderer pageRenderer;
        private readonly IHealthCheckResultFactory healthCheckResultFactory;
        private readonly IMailNotificationEngine mailEngine;
        private readonly ILogger<HealthCheckCompleteModel> logger;
        private readonly IEveryoneHealthReferralService everyoneHealthReferralService;

        public class UnsanitisedModel
        {
            public string SubmitAction { get; set; }
            public IEnumerable<string> Contact { get; set; }

            public string EmailAddress { get; set; }

            public string PhoneNumber { get; set; }

            public string SmsNumber { get; set; }
        }

        private class Email
        {
            public string HtmlBody { get; set; }
            public string Subject { get; set; }
        }

        public IEnumerable<Intervention> EveryoneHealthReferrals { get; set; }

        private enum ContactMethod
        {
            Email,
            SMS
        }

        public HealthCheckCompleteModel(
            Database database,
            ICredentialsDecrypter credentialsDecrypter,
            IPageRenderer pageRenderer,
            IHealthCheckResultFactory healthCheckResultFactory,
            IMailNotificationEngine mailEngine,
            IPageFlow pageFlow,
            ILogger<HealthCheckCompleteModel> logger,
            IEveryoneHealthReferralService everyoneHealthReferralService) 
            : base(database, credentialsDecrypter, pageFlow)
        {
            this.pageRenderer = pageRenderer;
            this.healthCheckResultFactory = healthCheckResultFactory;
            this.mailEngine = mailEngine;
            this.logger = logger;
            this.everyoneHealthReferralService = everyoneHealthReferralService;
        }

        private class SanitisedModel
        {
            public string EmailAddress { get; set; }
            public string SmsNumber { get; set; }
            public string PhoneNumber { get; set; }

        }

        public bool Preview { get; set; }

        public HealthCheck Check { get; set; }

        public Result Result { get; set; }

        public bool Validated { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if(HttpContext.Request.RouteValues["page"].ToString().ToLower() != "/HealthCheckComplete".ToLower())
            {
                //This page is being used to preview an email.
                //Load that information only, don't try to send an email

                Preview = true;

                await LoadEmailPages();
                return Page();
            }

            if (!IsValidated())
            {
                return RedirectToValidation();
            }

            Check = await GetHealthCheckAsync();

            Check.HealthCheckCompleted = true;
            Check.HealthCheckCompletedDate = DateTime.Now;

            await Database.SaveChangesAsync();

            await Database.Entry(Check).Collection(c => c.ChosenInterventions).LoadAsync();
            await Database.Entry(Check).Collection(c => c.CustomBarriers).LoadAsync();

            await Database.Entry(Check).Reference(c => c.BloodPressureFollowUp).LoadAsync();
            await Database.Entry(Check).Reference(c => c.BloodSugarFollowUp).LoadAsync();
            await Database.Entry(Check).Reference(c => c.CholesterolFollowUp).LoadAsync();

            await Database.Entry(Check).Reference(c => c.Variant).LoadAsync();

            var email = Check?.EmailAddress?.ToString();
            var sms = Check?.SmsNumber?.ToString();

            var contact = new List<string>();

            if (!string.IsNullOrEmpty(email))
            {
                contact.Add(ContactMethod.Email.ToString().ToLower());
            }
            if (!string.IsNullOrEmpty(sms))
            {
                contact.Add(ContactMethod.SMS.ToString().ToLower());
            }

            Result = healthCheckResultFactory.GetResult(Check, false);

            Model = new UnsanitisedModel
            {
                EmailAddress = email,
                SmsNumber = sms,
                Contact = contact
            };

            EveryoneHealthReferrals = everyoneHealthReferralService.GetEveryoneHealthReferrals(Check).ToList();

            await SendHealthCheckCompletionEmails();

            return Page();
        }

        private async Task SendHealthCheckCompletionEmails()
        {
            await LoadEmailPages();

            var sentPatientEmail = HttpContext.Session.GetBool("SentPatientEmail") == true;

            if (!sentPatientEmail && Check?.EmailAddress is not null)
            {
                await SendPatientEmail();
                HttpContext.Session.SetBool("SentPatientEmail", true);
                await Database.SaveChangesAsync();
            }

            var sentEveryoneHealthEmail = HttpContext.Session.GetBool("SentEveryoneHealthEmail") == true;

            if (!sentEveryoneHealthEmail && Check?.EveryoneHealthConsent == true && everyoneHealthReferralService.HasEveryoneHealthReferrals(Check))
            {
                await SendEveryoneHealthEmail();
                HttpContext.Session.SetBool("SentEveryoneHealthEmail", true);
                await Database.SaveChangesAsync();
            }
        }

        private async Task SendPageAsEmail(string pageRoute, string subject, string recipient)
        {
            var html = await pageRenderer.RenderHtmlAsync(pageRoute, this);

            RetryPolicy.Retry(() =>
            {
                var mailMessage = mailEngine
                    .NewMessage()
                    .To(recipient)
                    .WithSubject(subject)
                    .WithHtmlBody(html);

                using var message = mailMessage.Create();

                try
                {
                    mailEngine.SendMessage(message);
                }
                catch (Exception ex)
                {
                    logger.LogWarning($"Email failed to send for healthcheck id {Check.Id}. Retrying...");
                    throw new EmailFailedException(recipient, subject, ex);
                }
            }, new[] { typeof(EmailFailedException) }, TimeSpan.FromSeconds(1), 3, 1f);
        }

        private async Task SendPatientEmail()
        {
            try
            {
                await SendPageAsEmail
                (
                    "PatientEmail", 
                    $"Digital NHS Health Check{(Variant ? " Other" : "")} Results - PATIENT SUMMARY", 
                    Check.EmailAddress
                );
                Check.ReceivedHealthCheckCompleteEmail = true;
                Check.LastContactDate = DateTime.Now;
                Check.ReminderStatus = ReminderStatus.Unsent;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, $"Failed to send a patient email for Health Check {Check.Id}.");
            }
        }

        

        private async Task SendEveryoneHealthEmail()
        {
            try
            {
                await SendPageAsEmail
                (
                    "EveryoneHealth",
                    "Patient Referral",
                    everyoneHealthReferralService.EveryoneHealthReferralEmail
                );

                Check.EveryoneHealthReferralSent = true;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, $"Failed to send an Everyone Health referral email for Health Check {Check.Id}.");
            }
        }

        async Task LoadEmailPages()
        {
            Check = await GetHealthCheckAsync();

            await Database.Entry(Check).Collection(c => c.ChosenInterventions).LoadAsync();

            await Database.Entry(Check).Reference(c => c.BloodPressureFollowUp).LoadAsync();
            await Database.Entry(Check).Reference(c => c.BloodSugarFollowUp).LoadAsync();
            await Database.Entry(Check).Reference(c => c.CholesterolFollowUp).LoadAsync();
            await Database.Entry(Check).Reference(c => c.ImproveBloodPressureFollowUp).LoadAsync();
            await Database.Entry(Check).Reference(c => c.ImproveBloodSugarFollowUp).LoadAsync();
            await Database.Entry(Check).Reference(c => c.ImproveCholesterolFollowUp).LoadAsync();
            await Database.Entry(Check).Reference(c => c.DrinkLessFollowUp).LoadAsync();
            await Database.Entry(Check).Reference(c => c.HealthyWeightFollowUp).LoadAsync();
            await Database.Entry(Check).Reference(c => c.MentalWellbeingFollowUp).LoadAsync();
            await Database.Entry(Check).Reference(c => c.MoveMoreFollowUp).LoadAsync();
            await Database.Entry(Check).Reference(c => c.QuitSmokingFollowUp).LoadAsync();

            Result = healthCheckResultFactory.GetResult(Check, Variant);

            var validationStatus = HttpContext.Session.GetEnum<YesNoSkip>("Validated") ?? YesNoSkip.No;

            Validated = validationStatus == YesNoSkip.Yes;

            EveryoneHealthReferrals = everyoneHealthReferralService.GetEveryoneHealthReferrals(Check).ToList();
        }


        public string EmailAddressError { get; set; }
        public string SmsNumberError { get; set; }

        public UnsanitisedModel Model { get; set; }

        public bool RequestedNoContact { get; set; }

        async Task<SanitisedModel> ValidateAndSanitise(HealthCheck check, UnsanitisedModel model)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel();

            var sanitisedContactMethods = new Collection<ContactMethod>();

            if(model.Contact != null && model.Contact.Any(x => x == "none"))
            {
                RequestedNoContact = true;

                return sanitisedModel;
            }

            if (model.Contact != null && !model.Contact.Any(x=> x == "none"))
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

        protected override async Task<IActionResult> Reload()
        {
            Check = await GetHealthCheckAsync();

            await Database.Entry(Check).Collection(c => c.ChosenInterventions).LoadAsync();
            await Database.Entry(Check).Collection(c => c.CustomBarriers).LoadAsync();

            await Database.Entry(Check).Reference(c => c.BloodPressureFollowUp).LoadAsync();
            await Database.Entry(Check).Reference(c => c.BloodSugarFollowUp).LoadAsync();
            await Database.Entry(Check).Reference(c => c.CholesterolFollowUp).LoadAsync();

            await Database.Entry(Check).Reference(c => c.Variant).LoadAsync();

            Result = healthCheckResultFactory.GetResult(Check, false);

            var email = Check?.EmailAddress?.ToString();
            var sms = Check?.SmsNumber?.ToString();

            var contact = new List<string>();

            if (!string.IsNullOrEmpty(email))
            {
                contact.Add(ContactMethod.Email.ToString().ToLower());
            }
            if (!string.IsNullOrEmpty(sms))
            {
                contact.Add(ContactMethod.SMS.ToString().ToLower());
            }

            Model = new UnsanitisedModel
            {
                EmailAddress = email,
                SmsNumber = sms,
                Contact = contact
            };

            return await base.Reload();
        }

        public async Task<IActionResult> OnPostAsync(UnsanitisedModel model)
        {
            Model = model;

            if (model.SubmitAction == "UpdateContactDetails")
            {
                Check = await GetHealthCheckAsync();

                var sanitisedModel = await ValidateAndSanitise(Check, model);

                if (sanitisedModel is null)
                {
                    await Database.SaveChangesAsync();
                    return await Reload();
                }

                if (Check is not null)
                {
                    Check.EmailAddress = sanitisedModel.EmailAddress;
                    Check.SmsNumber = sanitisedModel.SmsNumber;
                    Check.ContactInformationUpdatedOnComplete = true;

                    await Database.SaveChangesAsync();
                }

                await SendHealthCheckCompletionEmails();

                return await Reload();
            }
            else
            {
                // submit action is SendSecondEmail. Because variant is set to true, we'll send the email for the variant.
                Variant = true;
                await LoadEmailPages();
                await SendPatientEmail();

                return await Reload();
            }
        }
    }
}
