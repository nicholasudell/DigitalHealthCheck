using System;
using DigitalHealthCheckCommon.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DigitalHealthCheckService
{
    /// <summary>
    /// Base class for tasks that the NHS Digital Health Check background service runs.
    /// </summary>
    public abstract class Task
    {
        private readonly string serviceName;

        /// <summary>
        /// Occurs when the task has completed with errors.
        /// </summary>
        public event EventHandler Failed;

        /// <summary>
        /// Occurs when has not met the criteria to run.
        /// </summary>
        public event EventHandler NotRun;

        /// <summary>
        /// Occurs when the task has completed without errors.
        /// </summary>
        public event EventHandler Success;

        /// <summary>
        /// Gets a value indicating whether this task can run or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this task can run; otherwise, <c>false</c>.
        /// </value>
        public abstract bool CanProcess { get; }

        /// <summary>
        /// Gets the header name for this task for logging purposes.
        /// </summary>
        /// <value>
        /// The name of the task used to generate a log header.
        /// </value>
        public abstract string Header { get; }

        /// <summary>
        /// Gets the log message used when this task cannot run.
        /// </summary>
        /// <remarks>
        /// Use this to explain why the task cannot run.
        /// </remarks>
        /// <value>
        /// The log message when this task cannot run.
        /// </value>
        protected abstract string CannotWorkMessage { get; }

        /// <summary>
        /// Gets the configuration for the service.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        protected IConfiguration Configuration { get; }
        protected IMailNotificationEngine MailEngine { get; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        protected ILogger Logger { get; }

        /// <summary>
        /// Gets the log message posted when work begins.
        /// </summary>
        /// <value>
        /// The log message posted when work begins.
        /// </value>
        protected abstract string WorkBegunMessage { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Task"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="pctName">Name of the borough this task is running for.</param>
        protected Task(ILogger logger, IConfiguration configuration, IMailNotificationEngine mailEngine)
        {
            Logger = logger;
            Configuration = configuration;
            MailEngine = mailEngine;
            serviceName = $"NHS Digital Health Check Background Service({Environment.MachineName})";
        }

        /// <summary>
        /// Notifies a support contact by email.
        /// </summary>
        /// <param name="subject">The email subject.</param>
        /// <param name="message">The email body.</param>
        public void NotifySupportByEmail(string subject, string message)
        {
            var mailMessage =
                MailEngine
                    .NewMessage()
                    .To(Configuration["SupportEmail"])
                    .WithSubject($"{serviceName}:> {subject}")
                    .WithPlainTextBody(message)
                    .Create();

            MailEngine.SendMessage(mailMessage);
        }

        /// <summary>
        /// Processes this task.
        /// </summary>
        public async System.Threading.Tasks.Task Process()
        {
            if (CanProcess)
            {
                Logger.LogInformation(WorkBegunMessage);

                await ExecuteWithErrorNotification(Work, Header);
            }
            else
            {
                Logger.LogDebug(CannotWorkMessage);

                NotRun?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// The work this task performs.
        /// </summary>
        protected abstract System.Threading.Tasks.Task Work();

        private async System.Threading.Tasks.Task ExecuteWithErrorNotification(Func<System.Threading.Tasks.Task> action, string taskName)
        {
            try
            {
                await action();

                Success?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred while trying to execute task {taskName}", ex);

                NotifySupportByEmail($"EXCEPTION :: {taskName}", ex.ToString());

                Failed?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
