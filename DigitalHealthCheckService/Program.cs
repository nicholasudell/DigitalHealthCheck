using System;
using System.Configuration;
using DigitalHealthCheckCommon.Mail;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Topshelf;
using DigitalHealthCheckEF;
using Microsoft.EntityFrameworkCore;
using DigitalHealthCheckCommon;

namespace DigitalHealthCheckService
{

    public class Program
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

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{platform}.json", optional: true, reloadOnChange: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            Log.Information($"Starting NHS Digital Health Check Background Service with the {platform} configuration.");

            var serviceProvider = CreateServiceProvider(configuration);

            var returnCode = HostFactory.Run(x =>
            {
                x.Service<Service>(s =>
                {
                    s.ConstructUsing(name => serviceProvider.GetService<Service>());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsLocalSystem();
                x.StartAutomatically();

                x.EnableServiceRecovery(r =>
                {
                    r.RestartService(TimeSpan.FromMinutes(1));
                    r.RestartService(TimeSpan.FromMinutes(15));
                    r.RestartService(TimeSpan.FromMinutes(30));

                    r.SetResetPeriod(1);
                });

                x.OnException(ex =>
                {
                    Log.Error("An uncaught error occurred in the NHS Digital Health Check service.", ex);
                });

                x.SetDescription("This service processes background tasks for the NHS Digital Health Check.");
                x.SetDisplayName("NHS Digital Health Check Background Service");
                x.SetServiceName("NHSDigitalHealthCheck");
            });

            var exitCode = (int)Convert.ChangeType(returnCode, returnCode.GetTypeCode());

            Environment.ExitCode = exitCode;
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

            services.AddLogging(builder => builder.AddSerilog(dispose: true));

            services.AddDbContext<Database>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                });
            });

            services.AddTransient<Service>();

            services.AddTransient<PatientFirstReminderEmail>();
            services.AddTransient<PatientSecondReminderEmail>();
            services.AddTransient<PatientSecondSurveyEmail>();
            services.AddTransient<ThrivaNotificationEmail>();

            services.AddTransient<IEveryoneHealthReferralService>(x=> new EveryoneHealthReferralService("unused"));

            services.AddSingleton(configuration);

            return services.BuildServiceProvider();
        }
    }
}
