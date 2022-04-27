using System.IO;

namespace QMSUK.DigitalHealthCheck.Encryption
{
    /// <summary>
    /// Interface for objects that encrypt data.
    /// </summary>
    public interface IEncrypter
    {
        /// <summary>
        /// Encrypts the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>Encrypted source.</returns>
        string Encrypt(string source);

        /// <summary>
        /// Encrypts the file at the source and stores it in the destination file.
        /// </summary>
        /// <param name="source">The source file.</param>
        /// <param name="destination">The destination file.</param>
        void Encrypt(FileInfo source, FileInfo destination);

        /// <summary>
        /// Encrypts the source stream, storing it in the destination stream.
        /// </summary>
        /// <param name="source">The source stream.</param>
        /// <param name="destination">The destination stream.</param>
        void Encrypt(Stream source, Stream destination);
    }
}