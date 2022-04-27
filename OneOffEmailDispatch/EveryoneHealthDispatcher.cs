using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckCommon.Mail;
using DigitalHealthCheckEF;
using Microsoft.Extensions.DependencyInjection;
using ServiceComponents.Emails;

namespace OneOffEmailDispatch
{
    public class EveryoneHealthDispatcher
    {
        private readonly Database database;
        private readonly IMailNotificationEngine mailEngine;
        private readonly IEveryoneHealthReferralService everyoneHealthReferralService;
        private readonly IHealthCheckResultFactory resultFactory;
        private readonly IBodyMassIndexCalculator bodyMassIndexCalculator;
        private readonly string baseUrl;

        public EveryoneHealthDispatcher(Database database, IMailNotificationEngine mailEngine, IEveryoneHealthReferralService everyoneHealthReferralService)
        {
            this.database = database;
            this.mailEngine = mailEngine;
            this.everyoneHealthReferralService = everyoneHealthReferralService;
        }

        public async Task Run()
        {
            var ids = new []
            {
                Guid.Parse("5EB2C37E-78B7-44E3-BFAB-2C428D38B27A"),
                Guid.Parse("671B7EC0-BF12-4756-8596-3F81CFD7AB50"),
                Guid.Parse("90A09D60-8AE8-419B-A88D-42B368B1284C"),
                Guid.Parse("B6ADEBC6-2028-4ED7-ADE2-4365FC11C85E"),
                Guid.Parse("D85A3BB5-C1F5-48F9-B352-49EBA0FB8E37"),
                Guid.Parse("D93D269A-0364-42CE-9940-6C6BDA4EEE0E"),
                Guid.Parse("2E287EDD-37A8-4A90-9CE5-774AA14DA3A7"),
                Guid.Parse("03E99270-CF09-45B7-86AC-7D074D626589"),
                Guid.Parse("D9072896-840A-4165-86DF-9DD28E771EE7"),
                Guid.Parse("D1A9689B-8C5A-4D62-A762-C24093312F57"),
                Guid.Parse("71E32D52-A5B9-4A99-BAFC-E4FAF286D9DA")
            };

            Console.WriteLine($"There should be {ids.Count()} checks to send. Verify this is correct.");

            Console.WriteLine($"Loading health checks, this might take a while.");

            var checks = new List<HealthCheck>();

            foreach(var id in ids)
            {
                var check = await database.HealthChecks.FindAsync(id);
                if(check != null)
                {
                    checks.Add(check);
                }
            }

            Console.WriteLine($"{checks.Count} emails ready to send. Press enter to send them.");

            Console.ReadLine();

            var index = 1;

            foreach (var check in checks)
            {
                Console.WriteLine($"Sending email {index} of {checks.Count}.");

                await database.Entry(check).Collection(c => c.ChosenInterventions).LoadAsync();

                var everyoneHealthReferrals = everyoneHealthReferralService.GetEveryoneHealthReferrals(check).ToList();

                var body = new BUnitPageRenderer().RenderHtml<EveryoneHealth>(p =>
                    p.Add(x => x.Check, check)
                        .Add(x=> x.EveryoneHealthReferralInterventions, everyoneHealthReferrals), 
                    s=> s.AddSingleton<IEveryoneHealthReferralService>(everyoneHealthReferralService)
                );

                try
                {
                    RetryPolicy.Retry
                    (
                        () => DispatchEmail("Patient Referral", body, everyoneHealthReferralService.EveryoneHealthReferralEmail, check.Id),
                        new[] { typeof(EmailFailedException) },
                        TimeSpan.FromSeconds(1),
                        3,
                        1f
                    );

                    check.EveryoneHealthReferralSent = true;

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
