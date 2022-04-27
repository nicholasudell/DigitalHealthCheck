using System;
using System.Linq;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckCommon.Mail;
using DigitalHealthCheckEF;
using Microsoft.EntityFrameworkCore;
using ServiceComponents.Emails;

namespace OneOffEmailDispatch
{
    public class SecondSurveyDispatcher
    {
        private readonly Database database;
        private readonly IMailNotificationEngine mailEngine;

        public SecondSurveyDispatcher(Database database, IMailNotificationEngine mailEngine)
        {
            this.database = database;
            this.mailEngine = mailEngine;
        }

        public async Task Run()
        {
            Console.WriteLine($"Loading health checks, this might take a while.");

            //Counting two follow ups completed as completed for this cohort.

            var patientsWithReminders = await database.HealthChecks
               .Include(c => c.BloodPressureFollowUp)
               .Include(c => c.BloodSugarFollowUp)
               .Include(c => c.CholesterolFollowUp)
               .Include(c => c.ImproveBloodPressureFollowUp)
               .Include(c => c.ImproveBloodSugarFollowUp)
               .Include(c => c.ImproveCholesterolFollowUp)
               .Include(c => c.DrinkLessFollowUp)
               .Include(c => c.HealthyWeightFollowUp)
               .Include(c => c.MentalWellbeingFollowUp)
               .Include(c => c.MoveMoreFollowUp)
               .Include(c => c.QuitSmokingFollowUp)
               .Where(x =>
                   x.EmailAddress != null &&
                   x.QRisk != null &&
                    (
                        (x.BloodPressureFollowUp != null && x.SecondHealthPriorityAfterResults == "bloodpressure") ||
                        (x.BloodSugarFollowUp != null && x.SecondHealthPriorityAfterResults == "bloodsugar") ||
                        (x.CholesterolFollowUp != null && x.SecondHealthPriorityAfterResults == "cholesterol") ||
                        (x.ImproveBloodPressureFollowUp != null && x.SecondHealthPriorityAfterResults == "improvebloodpressure") ||
                        (x.ImproveBloodSugarFollowUp != null && x.SecondHealthPriorityAfterResults == "improvebloodsugar") ||
                        (x.ImproveCholesterolFollowUp != null && x.SecondHealthPriorityAfterResults == "improvecholesterol") ||
                        (x.DrinkLessFollowUp != null && x.SecondHealthPriorityAfterResults == "alcohol") ||
                        (x.HealthyWeightFollowUp != null && x.SecondHealthPriorityAfterResults == "weight") ||
                        (x.MentalWellbeingFollowUp != null && x.SecondHealthPriorityAfterResults == "mental") ||
                        (x.MoveMoreFollowUp != null && x.SecondHealthPriorityAfterResults == "move") ||
                        (x.QuitSmokingFollowUp != null && x.SecondHealthPriorityAfterResults == "smoking")
                    ))
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

                var body = new BUnitPageRenderer().RenderHtml<SecondSurvey>(p =>
                    p.Add(x => x.Check, check));

                try
                {
                    RetryPolicy.Retry
                    (
                        () => DispatchEmail("Digital NHS Health Check Survey Follow Up", body, check.EmailAddress, check.Id),
                        new[] { typeof(EmailFailedException) },
                        TimeSpan.FromSeconds(1),
                        3,
                        1f
                    );
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
