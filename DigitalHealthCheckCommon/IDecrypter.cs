using System.IO;

namespace QMSUK.DigitalHealthCheck.Encryption
{
    /// <summary>
    /// Interface for objects that decrypt data.
    /// </summary>
    public interface IDecrypter
    {
        /// <summary>
        /// Decrypts the specified string
        /// </summary>
        /// <param name="source">The string to decrypt.</param>
        /// <returns>The decrypted string.</returns>
        string Decrypt(string source);

        /// <summary>
        /// Decrypts the source file and outputs to the destination file.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        void Decrypt(FileInfo source, FileInfo destination);

        /// <summary>
        /// Decrypts the source stream and outputs to the destination stream.
        /// </summary>
        /// <param name="source">The source stream.</param>
        /// <param name="destination">The destination stream.</param>
        void Decrypt(Stream source, Stream destination);
    }
}