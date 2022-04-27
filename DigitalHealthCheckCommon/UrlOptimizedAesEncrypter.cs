using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using QMSUK.DigitalHealthCheck.Encryption;

namespace DigitalHealthCheckCommon
{
    /// <summary>
    /// Encryption tool using AES 256 bit encryption. 
    /// This version uses an encoding that is optimal for primarily latin charsets but supports all unicode chars.
    /// </summary>
    /// <seealso cref="IEncrypter"/>
    /// <seealso cref="IDecrypter"/>
    public class UrlOptimisedAesEncrypter : IEncrypter, IDecrypter
    {
        static readonly Encoding TextEncoding = Encoding.UTF8;
        private readonly byte[] key;

        /// <summary>
        /// Initializes a new instance of the <see cref="AesEncrypter"/> class.
        /// </summary>
        /// <param name="hexKey">The hexadecimal key.</param>
        /// <exception cref="ArgumentException">
        /// Argument cannot be null or the empty string. - hexKey
        /// </exception>
        public UrlOptimisedAesEncrypter(string hexKey)
        {
            if (string.IsNullOrEmpty(hexKey))
            {
                throw new ArgumentException("Argument cannot be null or the empty string.", nameof(hexKey));
            }

            key = ConvertHexToByteArray(hexKey);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AesEncrypter"/> class.
        /// </summary>
        /// <param name="key">The hexadecimal key.</param>
        /// <exception cref="ArgumentNullException">key</exception>
        public UrlOptimisedAesEncrypter(byte[] key) => this.key = key ?? throw new ArgumentNullException(nameof(key));

        /// <summary>
        /// Converts the byte array to a hexadecimal string.
        /// </summary>
        /// <param name="bytes">The byte array to convert.</param>
        /// <returns>
        /// A string where each character is the hexadecimal equivalent of a byte-pair from the
        /// input array.
        /// </returns>
        public static string ConvertByteArrayToHexString(byte[] bytes)
        {
            var hex = new StringBuilder(bytes.Length * 2);

            foreach (var @byte in bytes)
            {
                hex.AppendFormat("{0:x2}", @byte);
            }

            return hex.ToString();
        }

        /// <summary>
        /// Converts a hexadecimal string to a byte array.
        /// </summary>
        /// <param name="hex">The input string in hexadecimal format.</param>
        /// <returns>
        /// An array of bytes, twice the length of the input string. Each input string character has
        /// been converted to two bytes in hexadecimal format.
        /// </returns>
        public static byte[] ConvertHexStringToByteArray(string hex)
        {
            var length = hex.Length;
            var bytes = new byte[length / 2];

            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }

        /// <summary>
        /// Converts a string into a stream.
        /// </summary>
        /// <remarks>
        /// Note that the resulting stream's index will be at the end of the stream. Set the
        /// position to 0 to be able to read the text data again.
        /// </remarks>
        /// <param name="text">The text to convert into a stream.</param>
        /// <returns>A stream containing the provided text.</returns>
        public MemoryStream CreateStreamFromText(string text) => new MemoryStream(TextEncoding.GetBytes(text));

        /// <summary>
        /// Creates a string from the contents of a stream.
        /// </summary>
        /// <param name="stream">The stream to convert to text.</param>
        /// <returns>The binary data from the stream, converted to text.</returns>
        public string CreateTextFromStream(MemoryStream stream)
        {
            var bytes = stream.ToArray();
            return TextEncoding.GetString(bytes);
        }

        /// <summary>
        /// Decrypts the specified string
        /// </summary>
        /// <param name="source">The string to decrypt.</param>
        /// <returns>The decrypted string.</returns>
        public string Decrypt(string source)
        {
            using (var unprocessed = new MemoryStream(ConvertHexStringToByteArray(source)))
            {
                using (var processed = new MemoryStream())
                {
                    Decrypt(unprocessed, processed);

                    processed.Position = 0;

                    return CreateTextFromStream(processed);
                }
            }
        }

        /// <summary>
        /// Decrypts the source file and outputs to the destination file.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <exception cref="ArgumentNullException">source or destination</exception>
        /// <exception cref="FileNotFoundException">The specified file does not exist.</exception>
        public void Decrypt(FileInfo source, FileInfo destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (!source.Exists)
            {
                throw new FileNotFoundException("The specified file does not exist.", source.FullName);
            }

            using (var inputStream = File.OpenRead(source.FullName))
            {
                using (var outputStream = File.Create(destination.FullName))
                {
                    Decrypt(inputStream, outputStream);
                }
            }
        }

        /// <summary>
        /// Decrypts the source stream and outputs to the destination stream.
        /// </summary>
        /// <param name="source">The source stream.</param>
        /// <param name="destination">The destination stream.</param>
        /// <exception cref="ArgumentNullException">source or destination</exception>
        public void Decrypt(Stream source, Stream destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            using (var cryptor = Aes.Create())
            {
                cryptor.Key = key;

                var ivBytes = new byte[16];

                source.Read(ivBytes, 0, 16);

                cryptor.IV = ivBytes;

                var decryptor = cryptor.CreateDecryptor();

                using (var cryptoStream = new CryptoStream(source, decryptor, CryptoStreamMode.Read))
                {
                    ForceCryptoStreamToLeaveUnderlyingStreamOpen(cryptoStream);
                    cryptoStream.CopyTo(destination);
                }
            }
        }

        /// <summary>
        /// Encrypts the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>Encrypted source.</returns>
        public string Encrypt(string source)
        {
            using (var unprocessed = CreateStreamFromText(source))
            {
                using (var processed = new MemoryStream())
                {
                    Encrypt(unprocessed, processed);

                    processed.Position = 0;

                    return ConvertByteArrayToHexString(processed.ToArray());
                }
            }
        }

        /// <summary>
        /// Encrypts the file at the source and stores it in the destination file.
        /// </summary>
        /// <param name="source">The source file.</param>
        /// <param name="destination">The destination file.</param>
        /// <exception cref="ArgumentNullException">source or destination</exception>
        /// <exception cref="FileNotFoundException">The specified file does not exist.</exception>
        public void Encrypt(FileInfo source, FileInfo destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (!source.Exists)
            {
                throw new FileNotFoundException("The specified file does not exist.", source.FullName);
            }

            using (var inputStream = File.OpenRead(source.FullName))
            {
                using (var outputStream = File.Create(destination.FullName))
                {
                    Encrypt(inputStream, outputStream);
                }
            }
        }

        /// <summary>
        /// Encrypts the source stream, storing it in the destination stream.
        /// </summary>
        /// <param name="source">The source stream.</param>
        /// <param name="destination">The destination stream.</param>
        /// <exception cref="ArgumentNullException">source or destination</exception>
        public void Encrypt(Stream source, Stream destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            using (var cryptor = Aes.Create())
            {
                cryptor.Key = key;

                destination.Write(cryptor.IV, 0, cryptor.IV.Length);

                var encryptor = cryptor.CreateEncryptor();

                using (var cryptoStream = new CryptoStream(destination, encryptor, CryptoStreamMode.Write))
                {
                    ForceCryptoStreamToLeaveUnderlyingStreamOpen(cryptoStream);
                    source.CopyTo(cryptoStream);
                }
            }
        }

        static byte[] ConvertHexToByteArray(string hex)
        {
            if (hex.Length % 2 != 0)
            {
                throw new FormatException("Argument is not a valid hex string.");
            }

            var result = new byte[hex.Length / 2];

            for (var i = 0; i < hex.Length; i += 2)
            {
                result[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return result;
        }

        void ForceCryptoStreamToLeaveUnderlyingStreamOpen(CryptoStream cryptoStream)
        {
            var leaveOpen = cryptoStream.GetType().GetField("_leaveOpen", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            leaveOpen.SetValue(cryptoStream, true);
        }
    }
}
