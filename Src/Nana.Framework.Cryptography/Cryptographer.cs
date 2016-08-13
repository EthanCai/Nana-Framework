using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entlib = Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;

namespace Nana.Framework.Cryptography
{
    public class Cryptographer
    {
        /// <summary>
        /// Hash Provider
        /// </summary>
        private const string HASH_PROVIDER = "hashprovider";

        /// <summary>
        /// Symmetric Provider
        /// </summary>
        private const string SYMMETRIC_PROVIDER = "symprovider";

        /// <summary>
        /// Computes the hash value of plain text using the given hash provider instance
        /// </summary>
        /// <param name="plainText">The input for which to compute the hash.</param>
        /// <returns>The computed hash code.</returns>
        public static string Hash(string plainText)
        {

            return Entlib.Cryptographer.CreateHash(HASH_PROVIDER, plainText);
        }

        /// <summary>
        /// Hashes the compare.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashedText">The hashed text.</param>
        /// <returns></returns>
        public static bool CompareHash(string plainText, string hashedText)
        {
            return Entlib.Cryptographer.CompareHash(HASH_PROVIDER, plainText, hashedText);
        }

        /// <summary>
        /// Encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns>The resulting cipher text</returns>
        public static string Encrypt(string plainText)
        {
            return Entlib.Cryptographer.EncryptSymmetric(SYMMETRIC_PROVIDER, plainText);
        }

        /// <summary>
        /// Decrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns>The resulting cipher text</returns>
        public static string Decrypt(string plainText)
        {
            return Entlib.Cryptographer.DecryptSymmetric(SYMMETRIC_PROVIDER, plainText);
        }
    }
}
