using System.Collections.Generic;
using System.IO;
using System.Net.Mail;

namespace DigitalHealthCheckCommon.Mail
{
    /// <summary>
    /// Interface for email notification messages
    /// </summary>
    /// <remarks>Note that this instance uses a Fluent API.</remarks>
    public interface INotificationMessage
    {
        /// <summary>
        /// Creates a new instance of <see cref="MailMessage"/>.
        /// </summary>
        /// <returns>A new <see cref="MailMessage"/></returns>
        MailMessage Create();

        /// <summary>
        /// Adds sender information to the message.
        /// </summary>
        /// <param name="from">The address to send this message from.</param>
        /// <param name="displayName">The display name for the address.</param>
        /// <returns>The INotificationMessage instance, to allow further configuration.</returns>
        INotificationMessage From(string from, string displayName = null);

        /// <summary>
        /// Adds recipient information to the message.
        /// </summary>
        /// <param name="recipients">The recipients.</param>
        /// <returns>The INotificationMessage instance, to allow further configuration.</returns>
        INotificationMessage To(IEnumerable<string> recipients);

        /// <summary>
        /// Adds recipient information to the message.
        /// </summary>
        /// <param name="recipient">The recipient.</param>
        /// <returns>The INotificationMessage instance, to allow further configuration.</returns>
        INotificationMessage To(string recipient);

        /// <summary>
        /// Adds an attachment to the message.
        /// </summary>
        /// <param name="attachment">The attachment.</param>
        /// <returns>The INotificationMessage instance, to allow further configuration.</returns>
        INotificationMessage WithAttachment(FileInfo attachment);

        /// <summary>
        /// Adds an attachment to the message.
        /// </summary>
        /// <param name="filename">The filename of the attachment to add.</param>
        /// <returns>The INotificationMessage instance, to allow further configuration.</returns>
        INotificationMessage WithAttachment(string filename);

        /// <summary>
        /// Adds a HTML body to the message.
        /// </summary>
        /// <param name="bodyTemplate">The HTML body template.</param>
        /// <returns>The INotificationMessage instance, to allow further configuration.</returns>
        INotificationMessage WithHtmlBody(string bodyTemplate);

        /// <summary>
        /// Adds a HTML body to the message from a file.
        /// </summary>
        /// <param name="filename">The filename to load the HTML body from.</param>
        /// <returns>The INotificationMessage instance, to allow further configuration.</returns>
        INotificationMessage WithHtmlBodyFromFile(string filename);

        /// <summary>
        /// Adds a plain text body to the message.
        /// </summary>
        /// <param name="bodyTemplate">The plain text body template.</param>
        /// <returns>The INotificationMessage instance, to allow further configuration.</returns>
        INotificationMessage WithPlainTextBody(string bodyTemplate);

        /// <summary>
        /// Adds a plain text body to the message from a file.
        /// </summary>
        /// <param name="filename">The filename to load the plain text body from.</param>
        /// <returns>The INotificationMessage instance, to allow further configuration.</returns>
        INotificationMessage WithPlainTextBodyFromFile(string filename);

        /// <summary>
        /// Adds a subject to the message.
        /// </summary>
        /// <param name="subjectTemplate">The subject template.</param>
        /// <returns>The INotificationMessage instance, to allow further configuration.</returns>
        INotificationMessage WithSubject(string subjectTemplate);
    }
}