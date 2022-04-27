using System;
using System.Net.Mail;

namespace DigitalHealthCheckCommon.Mail
{
    /// <summary>
    /// Interface for email notification engines.
    /// </summary>
    public interface IMailNotificationEngine
    {
        /// <summary>
        /// Gets a new <see cref="NotificationMessage"/> instance.
        /// </summary>
        /// <returns>A new <see cref="NotificationMessage"/> instance.</returns>
        INotificationMessage NewMessage();

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <returns>True if the message sent successfully; otherwise false.</returns>
        void SendMessage(MailMessage msg);

        /// <summary>
        /// Sends the message without waiting for the message to finish sending.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <returns>True if the message sent successfully; otherwise false.</returns>
        void SendMessageAsync(MailMessage msg);

        /// <summary>
        /// Sends the message asynchronous without waiting for the message to finish sending.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <param name="mailLogging">Callback for logging mail information.</param>
        /// <param name="loggingParameters">Parameters for the logger.</param>
        void SendMessageAsync(MailMessage msg, Action<string[]> mailLogging, string[] loggingParameters);
    }
}