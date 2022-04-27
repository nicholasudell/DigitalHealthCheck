using System;
using System.Collections.Generic;
using System.Reflection;
using System.Timers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DigitalHealthCheckService
{
    public class Service
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<Service> logger;

        public Service(IServiceProvider serviceProvider, ILogger<Service> logger, IConfiguration configuration)
        {
            this.serviceProvider = serviceProvider;

            this.logger = logger;

            logger.LogDebug("Starting work timer.");

            if(!int.TryParse(configuration["TimerInterval"], out var timerInterval))
            {
                throw new InvalidOperationException("Cannot load configuration value for TimerInterval");
            }

            timer = new Timer { Interval = timerInterval };
            timer.Elapsed += (sender, args) => RunTasks();
            timer.Enabled = true;

            logger.LogDebug("Work timer started successfully.");
        }

        bool inProgress;

        private void RunTasks()
        {
            lock (this)
            {
                if (inProgress)
                {
                    return;
                }

                inProgress = true;
            }

            logger.LogInformation($"Executing Digital Health Check background tasks, application version: {Assembly.GetExecutingAssembly().GetName().Version}");

            try
            {
                var taskNumber = 1;

                foreach (var task in Tasks)
                {
                    logger.LogDebug($"Task #{taskNumber} :: {task.Header}");

                    try
                    {
                        task.Process().Wait();
                    }
                    catch (Exception ex) 
                    {
                        logger.LogCritical("An error occurred when trying to run one of the schedule tasks", ex);
                    }

                    taskNumber++;
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical("An error occurred when trying to create one of the schedule tasks", ex);
            }
            finally
            {
                inProgress = false;
            }
        }

        private static Timer timer;

        IEnumerable<Task> Tasks
        {
            get
            {
                yield return serviceProvider.GetService<PatientFirstReminderEmail>();
                yield return serviceProvider.GetService<PatientSecondReminderEmail>();
                yield return serviceProvider.GetService<PatientSecondSurveyEmail>();
                yield return serviceProvider.GetService<ThrivaNotificationEmail>();
            }
        }

        public void Start() => timer.Start();

        public void Stop() => timer.Stop();
    }
}
