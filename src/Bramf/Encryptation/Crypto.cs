using Bramf.Extensions;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Bramf.Encryptation
{
    /// <summary>
    /// Defines a class that is used to encrypt and decrypt files and data
    /// </summary>
    public class Crypto : IDisposable
    {
        #region Private Members

        /// <summary>
        /// The algorithm to use while encryptation
        /// </summary>
        private CryptoAlgorithm mEncryptationAlgorithm;

        /// <summary>
        /// A gc handle to pin the password
        /// </summary>
        private GCHandle gcHandle;

        #endregion

        #region Public Properties

        /// <summary>
        /// The encoding used to encrypt and decrypt strings
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="encryptationAlgorithm">The algorithm to use in the encryptation</param>
        /// <param name="password">The password to use in the encryptation</param>
        public Crypto(string password, CryptoAlgorithm encryptationAlgorithm = CryptoAlgorithm.AES)
        {
            mEncryptationAlgorithm = encryptationAlgorithm;

            // Pin the password
            gcHandle = GCHandle.Alloc(password, GCHandleType.Pinned);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Encrypts a file using the selected algorithm
        /// </summary>
        /// <param name="inputFile">The path to the file to encrypt</param>
        /// <param name="outputPath">The path to the file without extension (will be created) where it will be encrypted.</param>
        /// <param name="deleteInputFile">If true, the file to encrypt will be deleted after encryptation.</param>
        public void EncryptFile(string inputFile, string outputPath, bool deleteInputFile = false)
        {
            // If the file no exists, throw exception
            if (!File.Exists(inputFile))
                throw new FileNotFoundException("File not found.", inputFile);

            // Generate a random salt
            byte[] salt = GenerateRandomSalt();

            // If the outputh does not end with .crypto
            if (!outputPath.EndsWith(".crypto"))
                outputPath += ".crypto"; // Add the extension

            // Create a file and get its stream
            using (var fs = new FileStream(outputPath, FileMode.Create))
            {
                // Set the symmetric encryptation algorithm
                using (var aes = new RijndaelManaged())
                {
                    // Configure the key
                    aes.KeySize = 256; // Set key size
                    aes.BlockSize = 128; // Set block size
                    aes.Padding = PaddingMode.PKCS7; // Set the padding mode

                    // Set the key
                    var key = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes((string)gcHandle.Target), salt, 50000); // Create a key generator
                    aes.Key = key.GetBytes(aes.KeySize / 8); // Set the key
                    aes.IV = key.GetBytes(aes.BlockSize / 8); // Set the initialization vector

                    // Se the operation mode while encryptation
                    aes.Mode = CipherMode.CFB;

                    // Write the salt to the beginning of the file, so in this case can be random every time
                    fs.Write(salt, 0, salt.Length);

                    // Create a cryptographic stream to write the content to a file
                    using (CryptoStream cryptoStream = new CryptoStream(fs, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    using(var inputFileStream = new FileStream(inputFile, FileMode.Open)) // Create an stream opening the file to encrypt
                    {
                        // Create a buffer of 1 mb to read the file
                        byte[] buffer = new byte[1048576];
                        int read;

                        try
                        {
                            // While there is something to read
                            while((read = inputFileStream.Read(buffer, 0, buffer.Length)) > 0)
                                cryptoStream.Write(buffer, 0, read); // Write it to the crypto stream
                        }
                        catch
                        {
                            throw;
                        }
                    }
                }
            }

            // Delete the file
            if (deleteInputFile) File.Delete(inputFile);
        }

        /// <summary>
        /// Decrypts a file using the selected algorithm
        /// </summary>
        /// <param name="inputFile">The path to the file to decrypt.</param>
        /// <param name="outputPath">The path to a file that will be created where the decrypted file will be saved</param>
        /// <param name="deleteEncryptedFile">TIf true, the file to decrypt will be deleted after decryptation.</param>
        public void DecryptFile(string inputFile, string outputPath, bool deleteEncryptedFile = true)
        {
            // Create an array to store the salt
            byte[] salt = new byte[32];

            // If the output path ends with .crypto
            if (outputPath.EndsWith(".crypto"))
                outputPath.Replace(".crypto", ""); // Remove it

            // Create an stream to read the encrypted file
            using (FileStream encryptedFs = new FileStream(inputFile, FileMode.Open))
            {
                // Read the salt
                encryptedFs.Read(salt, 0, salt.Length);

                // Create a new AES algorithm instance
                using (var aes = new RijndaelManaged())
                {
                    // Configure AES
                    aes.KeySize = 256;
                    aes.BlockSize = 128;

                    // Create the key
                    var key = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes((string)gcHandle.Target), salt, 50000);
                    aes.Key = key.GetBytes(aes.KeySize / 8); // Get the key
                    aes.IV = key.GetBytes(aes.BlockSize / 8); // Get the iteration vector
                    aes.Padding = PaddingMode.PKCS7; // Set the padding
                    aes.Mode = CipherMode.CFB; // Set the symmetric algorithm mode
                    // Create an stream to decrypt the data
                    using (var cryptoStream = new CryptoStream(encryptedFs, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    using (var fileOutput = new FileStream(outputPath, FileMode.Create)) // Create output file to decrypt the data
                    {
                        // Create a buffer of 1mb to read the file
                        byte[] buffer = new byte[1048576];
                        int read;

                        try
                        {
                            // While there is something to read...
                            while ((read = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                                fileOutput.Write(buffer, 0, read); // Write to the output file
                        }
                        catch
                        {
                            throw;
                        }
                    }
                }
            }

            // Delete the file
            if (deleteEncryptedFile) File.Delete(inputFile);
        }

        /// <summary>
        /// Encrypts a string using the selected algorithm
        /// </summary>
        /// <param name="plainText">The text to encrypt</param>
        /// <param name="returnEncryptationMode">The way the encrypted string is returned</param>
        public object EncryptString(string plainText, EncryptationMode returnEncryptationMode = EncryptationMode.Base64)
        {
            // If plainText is empty, throw exception
            if (plainText.IsNullOrWhitespace())
                throw new ArgumentNullException("plainText");

            // Variable to store the data
            byte[] encryptedBytes = new byte[1];

            // Create a symmetric encryptor / decryptor
            using (var aes = new RijndaelManaged())
            using (var memoryStream = new MemoryStream())
            {
                // Configure the key
                aes.KeySize = 256; // Set key size
                aes.BlockSize = 128; // Set block size
                aes.Padding = PaddingMode.PKCS7; // Set the padding mode

                // Generate a random salt
                byte[] salt = GenerateRandomSalt();

                // Set the key
                var key = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes((string)gcHandle.Target), salt, 50000); // Create a key generator
                aes.Key = key.GetBytes(aes.KeySize / 8); // Set the key
                aes.IV = key.GetBytes(aes.BlockSize / 8); // Set the initialization vector

                // Se the operation mode while encryptation
                aes.Mode = CipherMode.CBC;

                // Write the salt to the beginning of the file, so in this case can be random every time
                memoryStream.Write(salt, 0, salt.Length);

                // Create a cryptographic stream to write the content to a file
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (StreamWriter writer = new StreamWriter(cryptoStream)) // Create stream writer to write the text to the encrypter
                    writer.Write(plainText); // Write the plain text to the stream

                // Get the encrypted bytes
                encryptedBytes = memoryStream.ToArray();
            }

            // Return the data
            switch (returnEncryptationMode) // Switch between all encryptatin modes
            {
                case EncryptationMode.Base64:
                    return Convert.ToBase64String(encryptedBytes); // Convert the bytes to a base64 string and return

                case EncryptationMode.Unicode:
                    return Encoding.Unicode.GetString(encryptedBytes); // Convert the bytes to a string and return

                case EncryptationMode.Bytes:
                    return encryptedBytes; // Return the bytes

                default:
                    return default(byte[]); // If something happen, return an empty byte array
            }
        }

        /// <summary>
        /// Decrypts a data source using the selected algorithm
        /// </summary>
        /// <param name="cipherData">The data to decrypt</param>
        /// <param name="encryptationMode">The way the encrypted data is provided</param>
        public string DecryptString(object cipherData, EncryptationMode encryptationMode)
        {
            // If there is no cipher data, throw exception
            if (cipherData == null)
                throw new ArgumentNullException("cipherData");

            // Variable to store the decrypted string
            string decryptedString = string.Empty;
            byte[] encryptedDataBytes = new byte[1];

            // Get the bytes of the encrypted data
            switch(encryptationMode)
            {
                // Encrypted data is encoded with base64
                case EncryptationMode.Base64: 
                    if (cipherData.GetType() != typeof(string)) // If the cipher data type is not string, throw exception
                        throw new ArgumentException("The cipher data type provided is not the same as the provided in 'encryptationMode' argument", "cipherData");

                    // Convert the string to byte array 
                    encryptedDataBytes = Convert.FromBase64String((string)cipherData);
                    break;

                // Encrypted data is encoded with unicode
                case EncryptationMode.Unicode:
                    if (cipherData.GetType() != typeof(string)) // If the cipher data type is not string, throw exception
                        throw new ArgumentException("The cipher data type provided is not the same as the provided in 'encryptationMode' argument", "cipherData");

                    // Convert the string to byte array 
                    encryptedDataBytes = Encoding.Unicode.GetBytes((string)cipherData);
                    break;

                // Encrypted data is the bytes
                case EncryptationMode.Bytes:
                    if (cipherData.GetType() != typeof(byte[])) // If the cipher data type is not a byte array, throw exception
                        throw new ArgumentException("The cipher data type provided is not the same as the provided in 'encryptationMode' argument", "cipherData");

                    // Convert the string to byte array 
                    encryptedDataBytes = (byte[])cipherData;
                    break;
            }

            // Create an array to store the salt
            byte[] salt = new byte[32];

            // Create an stream to read the encrypted file
            using (MemoryStream encryptedMs = new MemoryStream(encryptedDataBytes))
            {
                // Read the salt
                encryptedMs.Read(salt, 0, salt.Length);

                // Create a new AES algorithm instance
                using (var aes = new RijndaelManaged())
                {
                    // Configure AES
                    aes.KeySize = 256;
                    aes.BlockSize = 128;

                    // Create the key
                    var key = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes((string)gcHandle.Target), salt, 50000);
                    aes.Key = key.GetBytes(aes.KeySize / 8); // Get the key
                    aes.IV = key.GetBytes(aes.BlockSize / 8); // Get the iteration vector
                    aes.Padding = PaddingMode.PKCS7; // Set the padding
                    aes.Mode = CipherMode.CBC; // Set the symmetric algorithm mode

                    // Create an stream to decrypt the data
                    using (var cryptoStream = new CryptoStream(encryptedMs, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    using (var streamReader = new StreamReader(cryptoStream)) // Create output file to decrypt the data
                        decryptedString = streamReader.ReadToEnd();// Read the decrypted string
                }
            }

            // Return the decrypted string
            return decryptedString;
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Generates a random salt of 32 bytes
        /// </summary>
        /// <returns></returns>
        private static byte[] GenerateRandomSalt()
        {
            // Variable to store the salt
            byte[] data = new byte[32];

            // Create a RNG generator
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                for (int i = 0; i < 10; i++) // While i is less than 10
                    rng.GetBytes(data); // Generate bytes

            // Return the salt
            return data;
        }

        /// <summary>
        /// Reads a byte array from an stream
        /// </summary>
        /// <param name="stream">The stream to read its bytes</param>
        private static byte[] ReadByteArray(Stream stream)
        {
            // Read the length
            byte[] rawLength = new byte[sizeof(int)]; // Buffer to store the length
            if (stream.Read(rawLength, 0, rawLength.Length) != rawLength.Length) // Try to length
                throw new Exception("Stream did not contain propertly formatted byte array."); // If something happen, throw exception

            // Read the content
            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)]; // Buffer to store the content
            if (stream.Read(buffer, 0, buffer.Length) != buffer.Length) // Try to read the content
                throw new Exception("Did not read the byte array properly."); // If something happen, throw exception

            // Return the buffer
            return buffer;
        }

        #endregion

        #region Dispose methods

        /// <summary>
        /// Dispose the object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                // Free the memory address where the password is allocated
                Bramf.Native.Memory.ZeroMemory(gcHandle.AddrOfPinnedObject(), ((string)gcHandle.Target).Length * 2);
                gcHandle.Free(); // Release the GCHandle
            }
        }

        #endregion
    }
}
