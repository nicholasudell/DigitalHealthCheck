using System;
using System.Linq;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckCommon.Mail;
using DigitalHealthCheckEF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServiceComponents.Emails;

namespace OneOffEmailDispatch
{
    public class GPReportDispatcher
    {
        private readonly Database database;
        private readonly IMailNotificationEngine mailEngine;
        private readonly IHealthCheckResultFactory resultFactory;
        private readonly IBodyMassIndexCalculator bodyMassIndexCalculator;

        public GPReportDispatcher(Database database, IMailNotificationEngine mailEngine, IHealthCheckResultFactory resultFactory, IBodyMassIndexCalculator bodyMassIndexCalculator)
        {
            this.database = database;
            this.mailEngine = mailEngine;
            this.resultFactory = resultFactory;
            this.bodyMassIndexCalculator = bodyMassIndexCalculator;
        }

        public async Task Run(string baseUrl)
        {
            Console.WriteLine($"Loading health checks, this might take a while.");

            //Counting two follow ups completed as completed for this cohort.

            var patientsWithReminders = await database.HealthChecks
               .Where(x =>
                   x.QRisk != null &&
                   !x.HealthCheckCompleted &&
                   !x.GPEmailSent &&
                   !x.ValidationOverwritten &&
                   x.GPEmail != null)
               .ToListAsync();

            var checks = patientsWithReminders
                .Where(x => x.CalculatedDate < new DateTime(2022,3,29))
                .ToList();

            Console.WriteLine($"{checks.Count} emails ready to send. Press enter to send them.");

            Console.ReadLine();

            var index = 1;

            foreach (var check in checks)
            {
                Console.WriteLine($"Sending email {index} of {checks.Count}.");

                var result = resultFactory.GetResult(check, false);

                var bmi = bodyMassIndexCalculator.CalculateBodyMassIndex(check.Height.Value, check.Weight.Value);

                var body = new BUnitPageRenderer().RenderHtml<GPReport>(p =>
                    p.Add(x => x.Check, check)
                        .Add(x=> x.Result, result)
                        .Add(x=> x.BaseUrl, baseUrl)
                , s=> s.AddSingleton<IBodyMassIndexCalculator>(bodyMassIndexCalculator)
                );

                try
                {
                    RetryPolicy.Retry
                    (
                        () => DispatchEmail("Digital NHS Health Check Results - GP REPORT", body, check.GPEmail, check.Id),
                        new[] { typeof(EmailFailedException) },
                        TimeSpan.FromSeconds(1),
                        3,
                        1f
                    );

                    check.GPEmailSent = true;

                    await database.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Email failed to send three times for id {check.Id}");
                }

                index++;
            }

            Console.WriteLine($"All emails processed.");
        }

        void DispatchEmail(string subject, string body, string recipient, Guid id)
        {
            var mailMessage = mailEngine
                .NewMessage()
                .To(recipient)
                .WithSubject(subject)
                .WithHtmlBody(body);

            using (var message = mailMessage.Create())
            {
                try
                {
                    mailEngine.SendMessage(message);
                    Console.WriteLine($"Email sent for id {id}.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Email failed to send for id {id}. Retrying...");
                    throw new EmailFailedException(recipient, subject, ex);
                }
            }

        }

    }
}
