using DigitalHealthCheckCommon;
using DigitalHealthCheckCommon.Mail;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.ErrorHandling;
using DigitalHealthCheckWeb.Helpers;
using DigitalHealthCheckWeb.Model;
using DigitalHealthCheckWeb.Model.Risks;
using DigitalHealthCheckWeb.Pages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QMSUK.DigitalHealthCheck.Encryption;
using Serilog;
using System;

namespace DigitalHealthCheckWeb
{
    public static class StartupExtensions
    {
        public static bool IsDevelopmentOrTest(this IHostEnvironment hostEnvironment) =>
            hostEnvironment.IsEnvironment("Development") || hostEnvironment.IsEnvironment("Test");
    }

    public class Startup
    {
        const string AesHexKey = "<256-bit-hex-key>";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DatabaseConnection");

            services.AddRazorPages();
            services.AddSession(options => options.IdleTimeout = TimeSpan.FromHours(2));
            services.AddMemoryCache();
            services.AddServerSideBlazor();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = MicrosoftAccountDefaults.AuthenticationScheme;
            })
                .AddCookie()
                .AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = Configuration["Auth:ClientId"];
                microsoftOptions.ClientSecret = Configuration["Auth:ClientSecret"];
                microsoftOptions.AuthorizationEndpoint = Configuration["Auth:AuthEndpoint"];
                microsoftOptions.TokenEndpoint = Configuration["Auth:TokenEndpoint"];
            });

            services.AddDbContext<Database>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                });
            });

            services.AddTransient<ISqlCommandSender, SqlCommandSender>
            (
                x => new SqlCommandSender(connectionString)
            );

            services.AddTransient<IPostcodeLookupService, TownsendPostcodeLookupService>();
            services.AddTransient<IPostcodeNormaliser, PostcodeNormaliser>();
            services.AddTransient<IRiskScoreCalculator, QMSRiskCalculator>();
            services.AddTransient<IHealthCheckResultFactory, HealthCheckResultFactory>();
            services.AddTransient<IHealthPriorityRouter, HealthPriorityRouter>();
            services.AddTransient<IBodyMassIndexCalculator, BodyMassIndexCalculator>();
            services.AddTransient<IUrlBuilder, UrlBuilder>();
            services.AddTransient<IDecrypter, UrlOptimisedAesEncrypter>(x => new UrlOptimisedAesEncrypter(AesHexKey));
            services.AddTransient<IJsonDeserializer<Credentials>, NewtonsoftJsonSerializationWrapper<Credentials>>();
            services.AddTransient<ICredentialsDecrypter, HealthCheckCredentialsDecrypter>();
            services.AddTransient<IPageRenderer, PageRenderer>();
            services.AddTransient<IPageFlow, PageFlow>();
            services.AddTransient<IRiskScoreService, RiskScoreService>();
            services.AddTransient<IEveryoneHealthReferralService, EveryoneHealthReferralService>(x=> new EveryoneHealthReferralService(Configuration.GetValue<string>("EveryoneHealthReferralEmail")));

            services.AddHostedService<LongRunningService>();
            services.AddSingleton<BackgroundWorkerQueue>();

            var mail = Configuration.GetSection("Mail");

            services.AddTransient<IMailNotificationEngine, MailNotificationEngine>(x =>
                new MailNotificationEngine(
                    mail.GetValue<string>("Host"),
                    mail.GetValue<int>("Port"),
                    mail.GetValue<string>("From"),
                    mail.GetValue<string>("Display Name"),
                    mail.GetValue<bool>("UseSsl"))
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopmentOrTest())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>(loggerFactory);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
