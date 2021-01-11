using System;
using System.Security.Cryptography;

namespace Bramf.Crypto
{
    /// <summary>
    /// Provides methods to hash strings of data
    /// </summary>
    public static class Hashing
    {
        /// <summary>
        /// Hashes a text
        /// </summary>
        /// <param name="text">The text to hash.</param>
        public static string Hash(string text)
        {
            byte[] salt = new byte[16];
            using RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(text, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Verifies if a text matches its hash
        /// </summary>
        public static bool VerifyHash(string hash, string text)
        {
            byte[] hashBytes = Convert.FromBase64String(hash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Compute the hash on the plain text
            using var pbkdf2 = new Rfc2898DeriveBytes(text, salt, 100000);
            byte[] textHash = pbkdf2.GetBytes(20);

            // Compare results
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != textHash[i])
                    return false;

            return true;
        }
    }
}