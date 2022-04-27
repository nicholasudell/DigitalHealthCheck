using System;
using System.Runtime.Serialization;

namespace DigitalHealthCheckCommon.Mail
{
    /// <summary>
    /// Exception type used when sending an email has failed for some reason.
    /// </summary>
    /// <seealso cref="Exception"/>
    public class EmailFailedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailFailedException"/> class.
        /// </summary>
        /// <param name="recipient">The recipient.</param>
        /// <param name="subject">The subject.</param>
        public EmailFailedException(string recipient, string subject) : this($"Unable to send email with subject {subject} to {recipient}.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailFailedException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public EmailFailedException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailFailedException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (Nothing
        /// in Visual Basic) if no inner exception is specified.
        /// </param>
        public EmailFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailFailedException"/> class.
        /// </summary>
        /// <param name="recipient">The recipient.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="innerException">The inner exception.</param>
        public EmailFailedException(string recipient, string subject, Exception innerException) : base($"Unable to send email with subject {subject} to {recipient}.", innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailFailedException"/> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the
        /// serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains
        /// contextual information about the source or destination.
        /// </param>
        protected EmailFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}