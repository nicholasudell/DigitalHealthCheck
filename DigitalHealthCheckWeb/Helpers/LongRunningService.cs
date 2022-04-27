using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DigitalHealthCheckWeb.Helpers
{
    public class LongRunningService : BackgroundService
    {
        private readonly BackgroundWorkerQueue queue;
        private readonly ILogger<LongRunningService> logger;
        private readonly IServiceProvider services;

        public LongRunningService(BackgroundWorkerQueue queue, ILogger<LongRunningService> logger, IServiceProvider services)
        {
            this.queue = queue;
            this.logger = logger;
            this.services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Background service starting.");

            using (var scope = services.CreateScope())
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        logger.LogInformation("Background worker waiting for work.");
                        var workItem = await queue.DequeueAsync(cancellationToken);
                        
                        logger.LogInformation("Background worker has work to process.");
                        var taskAndId = workItem(cancellationToken, scope.ServiceProvider);

                        try
                        {
                            await taskAndId.Item1;
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "Error while processing background work item, notifying queue.");

                            queue.RegisterError(taskAndId.Item2);
                        }

                        logger.LogInformation("Background worker has finished work.");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error while processing background work item");
                    }
                }
            }

            logger.LogWarning("Background service stopping.");
        }
    }
}

