using System;
using DigitalHealthCheckCommon;
using DigitalHealthCheckCommon.Mail;
using DigitalHealthCheckEF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OneOffEmailDispatch
{

    internal class Program
    {
        static void Main(string[] args)
        {
            var platform = "Development";

#if RELEASE
            platform = "Production";
#endif

#if TEST
            platform = "Test";
#endif

#if DEMO
            platform = "Demo";
#endif

            Console.WriteLine($"Running with the {platform} configuration. Close the program if this is incorrect! Press enter to continue.");

            Console.ReadLine();

            Console.WriteLine($"Loading configuration.");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{platform}.json", optional: true, reloadOnChange: true)
                .Build();

            var serviceProvider = CreateServiceProvider(configuration);

            var baseUrl = configuration["baseUrl"];

            var gPReportDispatcher = serviceProvider.GetService<GPReportDispatcher>();

            Console.WriteLine($"Running GP Report Dispatcher.");

            gPReportDispatcher.Run(baseUrl).Wait();

            Console.WriteLine($"Done press enter to continue.");

            Console.ReadLine();

            var everyoneHealthDispatcher = serviceProvider.GetService<EveryoneHealthDispatcher>();

            Console.WriteLine($"Running Everyone Health Dispatcher.");

            everyoneHealthDispatcher.Run().Wait();

            Console.WriteLine($"All done, press enter to exit.");

            Console.ReadLine();
        }

        private static ServiceProvider CreateServiceProvider(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConnection");

            //setup our DI
            var services = new ServiceCollection();

            var mail = configuration.GetSection("Mail");

            services.AddTransient<IMailNotificationEngine, MailNotificationEngine>(x =>
                new MailNotificationEngine(
                    mail["Host"],
                    int.Parse(mail["Port"]),
                    mail["From"],
                    mail["Display Name"],
                    bool.Parse(mail["UseSsl"]))
            );

            services.AddDbContext<Database>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                });
            });

            services.AddTransient<IHealthCheckResultFactory,HealthCheckResultFactory>();
            services.AddTransient<IBodyMassIndexCalculator,BodyMassIndexCalculator>();
            services.AddTransient<IEveryoneHealthReferralService, EveryoneHealthReferralService>(x=> new EveryoneHealthReferralService(configuration["EveryoneHealthReferralEmail"]));

            services.AddTransient<GPReportDispatcher>();
            services.AddTransient<EveryoneHealthDispatcher>();

            services.AddSingleton(configuration);

            return services.BuildServiceProvider();
        }
    }
}
