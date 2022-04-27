using System;
using System.Collections.Generic;
using Bunit;
using DigitalHealthCheckCommon;
using DigitalHealthCheckCommon.Mail;
using DigitalHealthCheckEF;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DigitalHealthCheckService
{
    public abstract class PatientEmail<TEmailComponent> : DigitalHealthCheckService.Task
        where TEmailComponent : IComponent
    {
        public override bool CanProcess =>
            enabled &&
                (DateTime.Now.IsBetween(StartTime, StopTime));

        readonly TimeSpan startTime;
        TimeSpan StartTime => startTime;
        readonly TimeSpan stopTime;
        TimeSpan StopTime => stopTime;
        private readonly Database database;

        protected Database Database => database;
        private readonly string websiteBaseUrl;
        protected string WebsiteBaseUrl => websiteBaseUrl;

        void DispatchEmail(string subject, string body, string recipient, Guid checkId)
        {
            var mailMessage = MailEngine
                .NewMessage()
                .To(recipient)
                .WithSubject(subject)
                .WithHtmlBody(body);

            using (var message = mailMessage.Create())
            {
                try
                {
                    MailEngine.SendMessage(message);
                }
                catch (Exception ex)
                {
                    Logger.LogWarning($"Email ({subject}) failed to send for healthcheck id {checkId}. Retrying...");
                    throw new EmailFailedException(recipient, subject, ex);
                }
            }

        }

        protected override string CannotWorkMessage => enabled ?
            $"Task skipped (Time window: from {StartTime} to {StopTime})." :
            "Task skipped (disabled [see PatientEmails.Enabled config] ).";

        private readonly bool enabled;

        public PatientEmail(
            ILogger logger,
            IConfiguration configuration,
            IMailNotificationEngine mailEngine,
            Database database)
            : base(logger, configuration, mailEngine)
        {
            this.database = database;
            var taskConfiguration = configuration.GetRequiredSection("PatientEmails");

            if (!bool.TryParse(taskConfiguration["Enabled"], out enabled))
            {
                throw new InvalidOperationException("Cannot load configuration value for PatientEmails.Enabled");
            }

            if (!TimeSpan.TryParse(taskConfiguration["StartTime"], out startTime))
            {
                throw new InvalidOperationException("Cannot load configuration value for PatientEmails.StartTime");
            }

            if (!TimeSpan.TryParse(taskConfiguration["StopTime"], out stopTime))
            {
                throw new InvalidOperationException("Cannot load configuration value for PatientEmails.StopTime");
            }

            if (!string.IsNullOrEmpty(taskConfiguration["WebsiteBaseUrl"]))
            {
                websiteBaseUrl = taskConfiguration["WebsiteBaseUrl"];
            }
            else
            {
                throw new InvalidOperationException("Cannot load configuration value for PatientEmails.WebsiteBaseUrl");
            }
        }

        protected abstract System.Threading.Tasks.Task<IEnumerable<HealthCheck>> GetEligiblePatientsAsync();

        protected abstract string Subject {get;}

        protected abstract void UpdateCheckAfterEmail(HealthCheck check);

        protected override async System.Threading.Tasks.Task Work()
        {
            var checks = await GetEligiblePatientsAsync();

            foreach(var check in checks)
            {
                Logger.LogDebug($"Sending email for patient {check.Id}");

                var body = new BUnitPageRenderer().RenderHtml<TEmailComponent>(p => SetComponentParameters(check,p));

                try
                {
                    RetryPolicy.Retry
                    (
                        () => DispatchEmail(Subject, body, check.EmailAddress, check.Id),
                        new[] { typeof(EmailFailedException) },
                        TimeSpan.FromSeconds(1),
                        3,
                        1f
                    );

                    UpdateCheckAfterEmail(check);
                }
                catch (AggregateException ex)
                {
                    Logger.LogError($"Error sending email ({Subject}):", ex);

                    NotifySupportByEmail("Error sending email ({Subject})", ex.ToString());
                }
            }

            Logger.LogDebug($"Emails sent, updating database.");

            await database.SaveChangesAsync();
        }

        protected abstract ComponentParameterCollectionBuilder<TEmailComponent> SetComponentParameters(HealthCheck check, ComponentParameterCollectionBuilder<TEmailComponent> parameterCollectionBuilder);

    }
}
