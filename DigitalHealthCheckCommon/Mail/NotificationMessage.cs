using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;

namespace DigitalHealthCheckCommon.Mail
{
    /// <summary>
    /// A notification message for email notifications
    /// </summary>
    /// <seealso cref="INotificationMessage"/>
    public class NotificationMessage : INotificationMessage
    {
        readonly MailMessage containedMailMessage;
        string htmlBody;
        private bool isFromSet;
        private bool isSubjectSet;
        private bool isToSet;
        string plainTextBody;

        /// <summary>
        /// Gets or sets the token values.
        /// </summary>
        /// <value>The token values.</value>
        public IDictionary<string, string> TokenValues { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationMessage"/> class.
        /// </summary>
        public NotificationMessage() => containedMailMessage = new MailMessage();

        /// <summary>
        /// Creates a new instance of <see cref="MailMessage"/>.
        /// </summary>
        /// <returns>A new <see cref="MailMessage"/></returns>
        public MailMessage Create()
        {
            if (htmlBody != null && plainTextBody != null)
            {
                SetBodyFromPlainText();
                var htmlAlternative = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
                containedMailMessage.AlternateViews.Add(htmlAlternative);
            }
            else
            {
                if (htmlBody != null)
                {
                    SetBodyFromHtmlText();
                }
                else if (plainTextBody != null)
                {
                    SetBodyFromPlainText();
                }
            }

            return containedMailMessage;
        }

        /// <summary>
        /// Adds sender information to the message.
        /// </summary>
        /// <param name="from">The address to send this message from.</param>
        /// <param name="displayName">The display name for the address.</param>
        /// <returns>The INotificationMessage instance, to allow further configuration.</returns>
        /// <exception cref="InvalidOperationException">Sender has already been set</exception>
        public INotificationMessage From(string from, string displayName = null)
        {
            if (isFromSet)
            {
                throw new InvalidOperationException("Sender has already been set");
            }

            containedMailMessage.From = new MailAddress(from, displayName);
            isFromSet = true;
            return this;
        }

        /// <summary>
        /// Adds recipient information to the message.
        /// </summary>
        /// <param name="recipient">The recipient.</param>
        /// <returns>The INotificationMessage instance, to allow further configuration.</returns>
        public INotificationMessage To(string recipient) => To(new[] { recipient });

        /// <summary>
        /// Adds recipient information to the message.
        /// </summary>
        /// <param name="recipients">The recipients.</param>
        /// <returns>The INotificationMessage instance, to allow further configuration.</returns>
        /// <exception cref="InvalidOperationException">Recipient has already been set</exception>
        public INotificationMessage To(IEnumerable<string> recipients)
        {
            if (isToSet)
            {
                throw new InvalidOperationException("Recipient has already been set");
            }

            foreach (var email in recipients)
            {
                containedMailMessage.To.Add(new MailAddress(email));
            }

            isToSet = true;
            return this;
        }

        /// <summary>
        /// Adds an attachment to the message.
        /// </summary>
        /// <param name="attachment">The attachment.</param>
        /// <returns>The INotificationMessage instance, to allow further configuration.</returns>
        public INotificationMessage WithAttachment(FileInfo attachment) =>
            WithAttachment(attachment.FullName);

        /// <summary>
        /// Adds an attachment to the message.
        /// </summary>
        /// <param name="filename">The filename of the attachment to add.</param>
        /// <returns>The INotificationMessage instance, to allow further configuration.</returns>
        public INotificationMessage WithAttachment(string filename)
        {
            containedMailMessage.Attachments.Add(new Attachment(filename));

            return this;
        }

        /// <summary>
        /// Adds a HTML body to the message.
        /// </summary>
        /// <param name="body">The HTML body.</param>
        /// <returns>The INotificationMessage instance, to allow further configuration.</returns>
        /// <exception cref="InvalidOperationException">An HTML body already exists</exception>
        public INotificationMessage WithHtmlBody(string body)
        {
            if (htmlBody != null)
            {
                throw new InvalidOperationException("An HTML body already exists");
            }

            htmlBody = body;
            return this;
        }

        /// <summary>
        /// Adds a HTML body to the message from a file.
        /// </summary>
        /// <param name="filename">The filename to load the HTML body from.</param>
        /// <returns>The INotificationMessage instance, to allow further configuration.</returns>
        public INotificationMessage WithHtmlBodyFromFile(string filename) =>
            WithHtmlBody(File.ReadAllText(filename));

        /// <summary>
        /// Adds a plain text body to the message.
        /// </summary>
        /// <param name="body">The plain text body template.</param>
        /// <returns>The INotificationMessage instance, to allow further configuration.</returns>
        /// <exception cref="InvalidOperationException">A plaintext body already exists</exception>
        public INotificationMessage WithPlainTextBody(string body)
        {
            if (plainTextBody != null)
            {
                throw new InvalidOperationException("A plaintext body already exists");
            }

            plainTextBody = body;
            return this;
        }

        /// <summary>
        /// Adds a plain text body to the message from a file.
        /// </summary>
        /// <param name="filename">The filename to load the plain text body from.</param>
        /// <returns>The INotificationMessage instance, to allow further configuration.</returns>
        public INotificationMessage WithPlainTextBodyFromFile(string filename) =>
            WithPlainTextBody(File.ReadAllText(filename));

        /// <summary>
        /// Adds a subject to the message.
        /// </summary>
        /// <param name="subjectTemplate">The subject template.</param>
        /// <returns>The INotificationMessage instance, to allow further configuration.</returns>
        /// <exception cref="InvalidOperationException">Subject has already been set</exception>
        public INotificationMessage WithSubject(string subject)
        {
            if (isSubjectSet)
            {
                throw new InvalidOperationException("Subject has already been set");
            }

            containedMailMessage.Subject = subject;
            isSubjectSet = true;

            return this;
        }

        void SetBodyFromHtmlText()
        {
            containedMailMessage.Body = htmlBody;
            containedMailMessage.IsBodyHtml = true;
        }

        void SetBodyFromPlainText()
        {
            containedMailMessage.Body = plainTextBody;
            containedMailMessage.IsBodyHtml = false;
        }
    }
}