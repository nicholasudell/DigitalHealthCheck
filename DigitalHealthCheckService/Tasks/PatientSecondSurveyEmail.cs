using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bunit;
using DigitalHealthCheckCommon.Mail;
using DigitalHealthCheckEF;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceComponents.Emails;
using Microsoft.EntityFrameworkCore;

namespace DigitalHealthCheckService
{
    public class PatientSecondSurveyEmail : PatientEmail<SecondSurvey>
    {
        readonly TimeSpan reminderGap;

        public PatientSecondSurveyEmail(ILogger logger, IConfiguration configuration, IMailNotificationEngine mailEngine, Database database) : base(logger, configuration, mailEngine, database)
        {
            var taskConfiguration = configuration.GetRequiredSection("PatientEmails");

            if (!TimeSpan.TryParse(taskConfiguration["SecondSurveyGap"], out reminderGap))
            {
                throw new InvalidOperationException("Cannot load configuration value for PatientEmails.SecondSurveyGap");
            }
        }

        public override string Header => "Patient Second Survey Email";

        protected override string Subject => "Digital NHS Health Check Survey Follow Up";

        protected override string WorkBegunMessage => "Sending patient second survey emails...";

        protected override async Task<IEnumerable<HealthCheck>> GetEligiblePatientsAsync() => 
            (await Database.HealthChecks
                .Where(x=> x.HealthCheckCompleted && x.EmailAddress != null && x.ReminderStatus == ReminderStatus.FirstReminder)
                .ToListAsync())
                .Where(x=> x.HealthCheckCompletedDate + reminderGap <= DateTime.Now)
                .ToList();

        protected override ComponentParameterCollectionBuilder<SecondSurvey> SetComponentParameters(HealthCheck check, ComponentParameterCollectionBuilder<SecondSurvey> parameterCollectionBuilder) => 
            parameterCollectionBuilder.Add(x => x.Check, check);
        protected override void UpdateCheckAfterEmail(HealthCheck check)
        {
            check.ReminderStatus = ReminderStatus.SecondSurvey;
        }
    }
}
