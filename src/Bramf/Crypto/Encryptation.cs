using Bramf.Extensions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Bramf.Crypto
{
    /// <summary>
    /// Provides methods to encrypt and hash strings
    /// </summary>
    public static class Encryptation
    {
        #region Constants

        private const int KeyDerivationIterationCount = 10000;

        #endregion

        /// <summary>
        /// Encrypts an string using AES
        /// </summary>
        /// <param name="data">The string to encrypt.</param>
        /// <param name="password">The password to use.</param>
        public static byte[] EncryptString(string data, string password)
        {
            byte[] output = null;

            using(SymmetricAlgorithm encryptator = Aes.Create())
            using (KeyedHashAlgorithm signer = new HMACSHA256())
            {
                encryptator.KeySize = 256; // Key size of 256 bits

                // Get the data bytes
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                byte[] cipherText; // Create a buffer to store the cipher data
                byte[] salt = GetRandomSalt(); // Get random salt

                // Get key and IV
                encryptator.Key = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512, KeyDerivationIterationCount, encryptator.KeySize / 8);
                encryptator.IV = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512, KeyDerivationIterationCount, encryptator.BlockSize / 8);

                // Encrypt
                using (var ms = new MemoryStream())
                using(var crypto = new CryptoStream(ms, encryptator.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    // Write to the crypto stream
                    crypto.Write(plainText, 0, plainText.Length);
                    crypto.FlushFinalBlock();

                    // Get encrypted data with IV
                    cipherText = encryptator.IV.CombineWith(ms.ToArray());
                }

                // Sign the data so we can detect tampering
                byte[] signature = SignData(password, cipherText, salt, encryptator, signer);

                // Add the signature to the cipher text
                output = signature.CombineWith(cipherText);

                // Write salt to the output
                output = salt.CombineWith(output);

                // Clear everything up
                encryptator.Clear();
                signer.Clear();

                // Clear unused arrays
                Array.Clear(plainText, 0, plainText.Length);
                Array.Clear(salt, 0, salt.Length);
                Array.Clear(cipherText, 0, cipherText.Length);
                Array.Clear(signature, 0, signature.Length);
            }

            return output;
        }

        /// <summary>
        /// Decrypts an string using AES
        /// </summary>
        /// <param name="data">The data to decrypt.</param>
        /// <param name="password">The password to use.</param>
        public static string DecryptString(byte[] data, string password)
        {
            byte[] plainText;

            // Read the salt from the array
            byte[] salt = new byte[32];
            Buffer.BlockCopy(data, 0, salt, 0, salt.Length);

            // Create algorithms
            using (SymmetricAlgorithm encryptator = Aes.Create())
            using (KeyedHashAlgorithm signer = new HMACSHA256())
            {
                // Extract the signature
                byte[] signature = new byte[signer.HashSize / 8];
                Buffer.BlockCopy(data, 32, signature, 0, signer.HashSize / 8);

                // Grab the rest of the data
                int payloadLength = data.Length - 32 - signature.Length;
                byte[] payload = new byte[payloadLength];
                Buffer.BlockCopy(data, 32 + signature.Length, payload, 0, payloadLength);

                // Check the signature before anything else is done to detect tampering and avoid oracles
                byte[] computedSignature = SignData(password, payload, salt, encryptator, signer);
                if (!computedSignature.IsEqualsTo(signature))
                    throw new CryptographicException("Invalid signature.");

                // Clear the signer algorithm
                signer.Clear();

                // Extract IV
                int ivLength = encryptator.BlockSize / 8;
                byte[] iv = new byte[ivLength];
                byte[] cipherText = new byte[payload.Length - ivLength];

                // Extract the data
                Buffer.BlockCopy(payload, 0, iv, 0, ivLength);
                Buffer.BlockCopy(payload, ivLength, cipherText, 0, payload.Length - ivLength);

                // Set encryptation key and IV
                encryptator.Key = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512, KeyDerivationIterationCount, encryptator.KeySize / 8);
                encryptator.IV = iv;

                // Decrypt the data
                using(var ms = new MemoryStream())
                using (var crypto = new CryptoStream(ms, encryptator.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    // Write to the stream
                    crypto.Write(cipherText, 0, cipherText.Length);
                    crypto.FlushFinalBlock();

                    // Get plain text
                    plainText = ms.ToArray();
                }

                // Clear algorithms
                encryptator.Clear();
            }

            // Return the decrypted data
            return Encoding.UTF8.GetString(plainText);
        }

        #region Helper Methods

        /// <summary>
        /// Generates a random 32 bytes buffer of salt
        /// </summary>
        /// <returns></returns>
        private static byte[] GetRandomSalt()
        {
            byte[] buffer = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                rng.GetBytes(buffer);

            return buffer;
        }

        /// <summary>
        /// Signs data
        /// </summary>
        private static byte[] SignData(string password, byte[] data, byte[] salt, SymmetricAlgorithm encryptator, KeyedHashAlgorithm signer)
        {
            // Get key
            signer.Key = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512, KeyDerivationIterationCount, encryptator.KeySize / 8);

            // Compute hash
            byte[] signature = signer.ComputeHash(data);

            // Clear the algorithm
            signer.Clear();

            // Return data
            return signature;
        }

        #endregion
    }
}