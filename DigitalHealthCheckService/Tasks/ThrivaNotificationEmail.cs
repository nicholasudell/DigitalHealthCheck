using System;
using System.Linq;
using DigitalHealthCheckCommon;
using DigitalHealthCheckCommon.Mail;
using DigitalHealthCheckEF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceComponents.Emails;

namespace DigitalHealthCheckService
{
    public class ThrivaNotificationEmail : Task
    {
        public override bool CanProcess =>
            enabled &&
            (DateTime.Now.IsBetween(StartTime, StopTime));

        public override string Header => "Thriva Notification Email";

        readonly TimeSpan startTime;
        TimeSpan StartTime => startTime;

        readonly TimeSpan stopTime;
        TimeSpan StopTime => stopTime;

        private readonly bool enabled;

        private readonly Database database;
        private readonly string websiteBaseUrl;
        readonly TimeSpan frequency;
        readonly string emailAddress;

        public ThrivaNotificationEmail(
            ILogger<ThrivaNotificationEmail> logger,
            IConfiguration configuration,
            IMailNotificationEngine mailEngine,
            Database database)
            : base(logger, configuration, mailEngine)
        {
            this.database = database;
            var taskConfiguration = configuration.GetRequiredSection("ThrivaNotificationEmail");

            if (!bool.TryParse(taskConfiguration["Enabled"], out enabled))
            {
                throw new InvalidOperationException("Cannot load configuration value for ThrivaNotificationEmail.Enabled");
            }

            if (!TimeSpan.TryParse(taskConfiguration["StartTime"], out startTime))
            {
                throw new InvalidOperationException("Cannot load configuration value for ThrivaNotificationEmail.StartTime");
            }

            if (!TimeSpan.TryParse(taskConfiguration["StopTime"], out stopTime))
            {
                throw new InvalidOperationException("Cannot load configuration value for ThrivaNotificationEmail.StopTime");
            }

            if (!string.IsNullOrEmpty(taskConfiguration["WebsiteBaseUrl"]))
            {
                websiteBaseUrl = taskConfiguration["WebsiteBaseUrl"];
            }
            else
            {
                throw new InvalidOperationException("Cannot load configuration value for ThrivaNotificationEmail.WebsiteBaseUrl");
            }

            if (!string.IsNullOrEmpty(taskConfiguration["EmailAddress"]))
            {
                emailAddress = taskConfiguration["EmailAddress"];
            }
            else
            {
                throw new InvalidOperationException("Cannot load configuration value for ThrivaNotificationEmail.EmailAddress");
            }

            if (!TimeSpan.TryParse(taskConfiguration["Frequency"], out frequency))
            {
                throw new InvalidOperationException("Cannot load configuration value for ThrivaNotificationEmail.Frequency");
            }
        }

        protected override string CannotWorkMessage
        {
            get
            {
                if (!enabled)
                {
                    return "Task skipped (disabled [see ThrivaNotificationEmails.Enabled config] ).";
                }

                if (!DateTime.Now.IsBetween(StartTime, StopTime))
                {
                    return $"Task skipped (Time window: from {StartTime} to {StopTime}).";
                }

                return "Task skipped (unknown reason)";
            }

        }

        protected override string WorkBegunMessage => "Sending Thriva notification email...";

        protected override async System.Threading.Tasks.Task Work()
        {
            // Config files are meant to be immutable, so we retrieve mutable state from the DB
            // This means we need to check it here, instead of in the constructor, to assure the DB is accessed on the same thread.

            var lastExecutionSetting = await database.Settings.FindAsync("ThrivaNotificationEmail.LastExecution");

            DateTime lastExecution;

            if (lastExecutionSetting is null)
            {
                lastExecution = DateTime.MinValue;
            }
            else
            {
                lastExecution = DateTime.Parse(lastExecutionSetting.Value);
            }

            if(DateTime.Now <= lastExecution + frequency)
            {
                Logger.LogInformation($"Task skipped (it's been less than {frequency} since the last execution ({lastExecution}).");

                return;
            }

            var requests = await database.BloodKitRequests.Where(x => x.DateRequested > lastExecution).ToListAsync();

            if(!requests.Any())
            {
                Logger.LogInformation($"No blood kit requests since {lastExecution}");

                return;
            }

            Logger.LogDebug($"Sending Thriva notification");

            var body = new BUnitPageRenderer().RenderHtml<ThrivaNotification>(p => 
                p.Add(x=> x.RequestCount, requests.Count)
                .Add(x=> x.BaseUrl, websiteBaseUrl));

            try
            {
                RetryPolicy.Retry
                (
                    () => DispatchEmail("Digital NHS Health Check Blood Kit Request Notification", body, emailAddress),
                    new[] { typeof(EmailFailedException) },
                    TimeSpan.FromSeconds(1),
                    3,
                    1f
                );

                Logger.LogDebug($"Notification sent, updating settings DB.");

                if (lastExecutionSetting is null)
                {
                    lastExecutionSetting = new Setting
                    {
                        Id = "ThrivaNotificationEmail.LastExecution",
                        Value = DateTime.Now.ToString()
                    };

                    await database.AddAsync(lastExecutionSetting);
                }
                else
                {
                    lastExecutionSetting.Value = DateTime.Now.ToString();
                }

                await database.SaveChangesAsync();
            }
            catch (AggregateException ex)
            {
                Logger.LogError($"Error sending patient reminder email:", ex);

                NotifySupportByEmail("Error sending patient reminder emails", ex.ToString());
            }
        }

        void DispatchEmail(string subject, string body, string recipient)
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
                    Logger.LogWarning($"Email failed to send for Thriva notifications. Retrying...");
                    throw new EmailFailedException(recipient, subject, ex);
                }
            }

        }
    }
}
