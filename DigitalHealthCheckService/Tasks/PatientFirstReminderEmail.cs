using System;
using System.Collections.Generic;
using System.Linq;
using Bunit;
using DigitalHealthCheckCommon;
using DigitalHealthCheckCommon.Mail;
using DigitalHealthCheckEF;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceComponents.Emails;

namespace DigitalHealthCheckService
{

    public class PatientFirstReminderEmail : PatientReminderEmail<PatientFirstReminder>
    {
        readonly IEveryoneHealthReferralService everyoneHealthReferralService;
        readonly TimeSpan reminderGap;

        public PatientFirstReminderEmail(
            IEveryoneHealthReferralService everyoneHealthReferralService,
            ILogger<PatientFirstReminderEmail> logger,
            IConfiguration configuration,
            IMailNotificationEngine mailEngine,
            Database database)
            : base(logger, configuration, mailEngine, database)
        {
            this.everyoneHealthReferralService = everyoneHealthReferralService;

            var taskConfiguration = configuration.GetRequiredSection("PatientEmails");

            if (!TimeSpan.TryParse(taskConfiguration["FirstReminderGap"], out reminderGap))
            {
                throw new InvalidOperationException("Cannot load configuration value for PatientEmails.FirstReminderGap");
            }
        }

        public override string Header => "Patient First Reminder Emails";

        protected override string Subject => "Your Digital NHS Health Check Reminders";

        protected override string WorkBegunMessage => "Sending patient first reminder emails...";

        protected override async System.Threading.Tasks.Task<IEnumerable<HealthCheck>> GetEligiblePatientsAsync() => 
            await GetEligiblePatientsAsync(ReminderStatus.Unsent, reminderGap);
        protected override ComponentParameterCollectionBuilder<PatientFirstReminder> SetComponentParameters(HealthCheck check, ComponentParameterCollectionBuilder<PatientFirstReminder> parameterCollectionBuilder)
        {
            var reminders = GetReminders(check);

            return parameterCollectionBuilder
                    .Add(x => x.Check, check)
                    .Add(x => x.Reminders, reminders)
                    .Add(x=> x.BaseUrl, WebsiteBaseUrl)
                    .Add(x=> x.EveryoneHealthReferralInterventions, everyoneHealthReferralService.GetEveryoneHealthReferrals(check).ToList());
        }

        protected override void UpdateCheckAfterEmail(HealthCheck check) 
            => UpdateCheckAfterEmail(check, ReminderStatus.FirstReminder);
    }
}
