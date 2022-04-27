using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckCommon.Mail;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Helpers;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DigitalHealthCheckWeb.Pages
{
    public class HealthCheckInCompleteModel : HealthCheckPageModel
    {
        private class SanitisedModel
        {
            public string EmailAddress { get; set; }

            public string PhoneNumber { get; set; }

            public string SmsNumber { get; set; }
        }

        private enum ContactMethod
        {
            Email,
            SMS
        }

        private class Email
        {
            public string HtmlBody { get; set; }

            public string Subject { get; set; }
        }

        private readonly ILogger<HealthCheckInCompleteModel> logger;
        private readonly IMailNotificationEngine mailEngine;
        private readonly IPageRenderer pageRenderer;

        public HealthCheck Check { get; set; }

        public CompleteModel.UnsanitisedModel Complete { get; set; }

        public string EmailAddressError { get; set; }

        public bool RequestedNoContact { get; set; }

        public Result Result { get; set; }

        public string SmsNumberError { get; set; }

        public bool Validated { get; set; }

        public HealthCheckInCompleteModel(
            Database database,
            ICredentialsDecrypter credentialsDecrypter,
            IPageRenderer pageRenderer,
            IMailNotificationEngine mailEngine,
            IPageFlow pageFlow,
            ILogger<HealthCheckInCompleteModel> logger)
            : base(database, credentialsDecrypter, pageFlow)
        {
            this.pageRenderer = pageRenderer;
            this.mailEngine = mailEngine;
            this.logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Request.RouteValues["page"].ToString().ToLower() != "/HealthCheckInComplete".ToLower())
            {
                //This page is being used to preview an email.
                //Load that information only, don't try to send an email

                await LoadEmailPages();
                return Page();
            }

            Check = await GetHealthCheckAsync();

            await Database.Entry(Check).Collection(c => c.ChosenInterventions).LoadAsync();

            await Database.Entry(Check).Reference(c => c.HeightAndWeightFollowUp).LoadAsync();

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

            Complete = new CompleteModel.UnsanitisedModel
            {
                EmailAddress = email,
                SmsNumber = sms,
                Contact = contact
            };

            await SendHealthCheckInCompletionEmails();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CompleteModel.UnsanitisedModel model)
        {
            Complete = model;

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

            await SendHealthCheckInCompletionEmails();

            return await Reload();
        }

        protected override async Task<IActionResult> Reload()
        {
            Check = await GetHealthCheckAsync();

            await Database.Entry(Check).Collection(c => c.ChosenInterventions).LoadAsync();

            await Database.Entry(Check).Reference(c => c.HeightAndWeightFollowUp).LoadAsync();

            return await base.Reload();
        }

        void DispatchEmail(Email email, string recipient) =>
            RetryPolicy.Retry(() =>
            {
                var mailMessage = mailEngine
                    .NewMessage()
                    .To(recipient)
                    .WithSubject(email.Subject)
                    .WithHtmlBody(email.HtmlBody);

                using var message = mailMessage.Create();

                try
                {
                    mailEngine.SendMessage(message);
                }
                catch (Exception ex)
                {
                    //logger.Log(LogLevel.Warning, $"Email failed to send. Retrying...");
                    throw new EmailFailedException(recipient, email.Subject, ex);
                }
            }, new[] { typeof(EmailFailedException) }, TimeSpan.FromSeconds(1), 3, 1f);

        async Task LoadEmailPages()
        {
            Check = await GetHealthCheckAsync();

            await Database.Entry(Check).Collection(c => c.ChosenInterventions).LoadAsync();
            await Database.Entry(Check).Reference(c => c.HeightAndWeightFollowUp).LoadAsync();

            var validationStatus = HttpContext.Session.GetEnum<YesNoSkip>("Validated") ?? YesNoSkip.No;

            Validated = validationStatus == YesNoSkip.Yes;
        }

        private async Task SendHealthCheckInCompletionEmails()
        {
            await LoadEmailPages();

            if (Check?.EmailAddress is not null)
            {
                await SendPatientEmail();

                await Database.SaveChangesAsync();
            }
        }

        private async Task SendPatientEmail()
        {
            try
            {
                var html = await pageRenderer.RenderHtmlAsync("PatientEmailIncomplete", this);

                DispatchEmail(new Email
                {
                    HtmlBody = html,
                    Subject = "Digital NHS Health Check Next Steps - PATIENT SUMMARY"
                }, Check.EmailAddress);

                Check.ReceivedHealthCheckIncompleteEmail = true;
                Check.LastContactDate = DateTime.Now;
                Check.ReminderStatus = ReminderStatus.Unsent;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, $"Failed to send a patient email for Health Check {Check.Id}.");
            }
        }

        async Task<SanitisedModel> ValidateAndSanitise(HealthCheck check, CompleteModel.UnsanitisedModel model)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel();

            var sanitisedContactMethods = new Collection<ContactMethod>();

            if (model.Contact != null && model.Contact.Any(x => x == "none"))
            {
                RequestedNoContact = true;

                return sanitisedModel;
            }

            if (model.Contact != null && !model.Contact.Any(x => x == "none"))
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
    }
}