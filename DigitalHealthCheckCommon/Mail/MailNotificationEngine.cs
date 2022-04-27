using System;
using System.Net.Mail;
using System.Threading;

namespace DigitalHealthCheckCommon.Mail
{
    /// <summary>
    /// Email notification engine
    /// </summary>
    /// <seealso cref="IMailNotificationEngine"/>
    public class MailNotificationEngine : IMailNotificationEngine
    {
        private readonly string displayName;
        private readonly string from;
        private readonly string host;
        private readonly int port;
        private readonly bool useSsl;
        private NotificationMessage message;

        /// <summary>
        /// Initializes a new instance of the <see cref="MailNotificationEngine"/> class.
        /// </summary>
        /// <param name="host">The host of the mail server.</param>
        /// <param name="port">The port the mail server operates on.</param>
        /// <param name="from">The email address to put in the email's from field.</param>
        /// <param name="displayName">The display name for the email's from field.</param>
        /// <param name="useSsl">if set to <c>true</c>, use SSL when communicating with the mail server.</param>
        /// <exception cref="ArgumentException">'{nameof(host)}' cannot be null or empty. - host</exception>
        public MailNotificationEngine(string host, int port, string from, string displayName = null, bool useSsl = true)
        {
            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentException($"'{nameof(host)}' cannot be null or empty.", nameof(host));
            }

            this.host = host;
            this.port = port;
            this.from = from;
            this.displayName = displayName;
            this.useSsl = useSsl;
        }

        /// <summary>
        /// Gets a new <see cref="INotificationMessage"/> instance.
        /// </summary>
        /// <remarks>
        /// This does not actually create a new <see cref="INotificationMessage"/> and instead uses
        /// the one created when this instance was created.
        /// </remarks>
        /// <returns>A "new" <see cref="INotificationMessage"/> instance.</returns>
        public INotificationMessage NewMessage()
        {
            message = new NotificationMessage();
            message.From(from, displayName);
            return message;
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <returns>True if the message sent successfully; otherwise false.</returns>
        /// <exception cref="ArgumentException">Invalid mail message...</exception>
        public void SendMessage(MailMessage msg)
        {
            if (!IsValidMessage(msg))
            {
                throw new ArgumentException("Invalid mail message...");
            }

            SendEmailMessage(msg);
        }

        /// <summary>
        /// Sends the message without waiting for the message to finish sending.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <param name="mailLogging">Callback for logging mail information.</param>
        /// <param name="loggingParameters">Parameters for the logger.</param>
        /// <exception cref="ArgumentException">Invalid mail message...</exception>
        public void SendMessageAsync(MailMessage msg, Action<string[]> mailLogging, string[] loggingParameters)
        {
            if (!IsValidMessage(msg))
            {
                throw new ArgumentException("Invalid mail message...");
            }

            ThreadPool.QueueUserWorkItem(s =>
            {
                SendEmailMessage(msg);

                mailLogging(loggingParameters);
            });
        }

        /// <summary>
        /// Sends the message without waiting for the message to finish sending.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <exception cref="ArgumentException">Invalid mail message...</exception>
        public void SendMessageAsync(MailMessage msg)
        {
            if (!IsValidMessage(msg))
            {
                throw new ArgumentException("Invalid mail message...");
            }

            ThreadPool.QueueUserWorkItem(s => SendEmailMessage(msg));
        }

        private static bool IsValidMessage(MailMessage msg)
        {
            var isValid = msg.To.Count > 0 &&
                           msg.From != null &&
                           !string.IsNullOrEmpty(msg.Body);

            return isValid;
        }

        private void SendEmailMessage(MailMessage message)
        {
            // NOTE: SMTP Client is now deprecated https://github.com/dotnet/platform-compat/blob/master/docs/DE0005.md
            // It's recommended to switch to MailKit

            var smtpClient = new SmtpClient(host, port)
            {
                EnableSsl = useSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            smtpClient.Send(message);
        }
    }
}