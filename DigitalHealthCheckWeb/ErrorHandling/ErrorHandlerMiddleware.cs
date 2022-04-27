using DigitalHealthCheckCommon;
using DigitalHealthCheckCommon.Mail;
using DigitalHealthCheckWeb.Model;
using DigitalHealthCheckWeb.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DigitalHealthCheckWeb.ErrorHandling
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IConfiguration configuration;
        private readonly IMailNotificationEngine mailEngine;
        private readonly ILogger logger;


        public ErrorHandlerMiddleware(RequestDelegate next, ILoggerFactory logger, IConfiguration configuration, IMailNotificationEngine mailEngine)
        {
            this.next = next;
            this.configuration = configuration;
            this.mailEngine = mailEngine;
            this.logger = logger.CreateLogger(typeof(ErrorHandlerMiddleware));

        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.ToString());

                try
                {
                    var recipient = configuration.GetValue<string>("SupportEmail");

                    var id = "unknown";

                    if(context.Request.Query.ContainsKey("id"))
                    {
                        id = context.Request.Query["id"];
                    }

                    var url = context.Request.GetDisplayUrl();

                    RetryPolicy.Retry(() =>
                    {
                        var mailMessage = mailEngine
                            .NewMessage()
                            .To(recipient)
                            .WithSubject("DIGITAL HEALTH CHECK ERROR")
                            .WithPlainTextBody($"A user ({id}) has encountered an error on the digital health check while navigating to {url}{Environment.NewLine}{Environment.NewLine}{ex.Message}{Environment.NewLine}{ex}");

                        using var message = mailMessage.Create();

                        try
                        {
                            mailEngine.SendMessage(message);
                        }
                        catch (Exception ex)
                        {
                            logger.LogWarning($"Error notification email failed to send. Retrying...");
                            throw new EmailFailedException(recipient, "DIGITAL HEALTH CHECK ERROR", ex);
                        }
                    }, new[] { typeof(EmailFailedException) }, TimeSpan.FromSeconds(1), 3, 1f);
                }
                catch(Exception ex2)
                {
                    logger.LogError($"Error notification email failed to send.",ex2);
                }

                throw;  
            }
        }
    }
}
