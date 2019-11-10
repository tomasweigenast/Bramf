using Bramf.Extensions;
using Bramf.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Bramf.Security
{
    /// <summary>
    /// Exposes methods to generate safe-numbers
    /// </summary>
    public static class IdGenerator
    {
        /// <summary>
        /// Generates a new safe random id
        /// </summary>
        /// <param name="bytes">The amount of bytes to use</param>
        public static string Generate(int bytes = 16)
        {
            byte[] buffer = new byte[bytes];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(buffer);

            return Convert.ToBase64String(buffer, Base64FormattingOptions.None).Replace("=", "").Replace("+", "").Replace("/", "");
        }

        /// <summary>
        /// Generates a random Id implementing custom arguments that can be or not decoded later
        /// </summary>
        /// <param name="arguments">The arguments to include in the Id</param>
        /// <param name="allowDecode">If true, it will allows you to decode the id and get back the arguments</param>
        public static string Generate(ICollection<string> arguments, bool allowDecode = false)
        {
            // Make sure we have arguments
            if (arguments == null || arguments.Count <= 0) throw new InvalidOperationException("If you won't use arguments, use Generate(int) method instead.");

            // Create buffers
            byte[] buffer = new byte[48];
            byte[] guid = new byte[16];
            byte[] randomNumbers = new byte[32];

            // Generate a random buffer of bytes
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(randomNumbers);

            // Generate a GUID 
            guid.Fill(Guid.NewGuid().ToByteArray());

            // Fill buffer
            buffer.Fill(new Collection<byte[]> { randomNumbers, guid });

            // Encode arguments
            string encodedArguments = JsonConvert.SerializeObject(arguments).ToBase64(!allowDecode);

            // Build the id to return
            StringBuilder id = new StringBuilder(); // FORMAT: Guid.Arguments.Numbers.AllowDecode
            id.Append(guid.ToBase64(!allowDecode)); // Add GUID
            id.Append('.');
            id.Append(encodedArguments); // Add arguments 
            id.Append('.');
            id.Append(randomNumbers.ToBase64(true)); // Add the random numbers
            id.Append('.');
            id.Append(allowDecode ? "1" : "0");

            // Return the id
            return id.ToString();
        }

        /// <summary>
        /// Generates a random Id implementing a single custom argument that can be or not decoded later
        /// </summary>
        /// <param name="argument">The argument to include in the id</param>
        /// <param name="allowDecode">If true, it will allows you to decode the id and get back the argument</param>
        public static string Generate(string argument, bool allowDecode = false)
        {
            // Make sure we have arguments
            if (argument.IsNullOrWhitespace()) throw new InvalidOperationException("If you won't use arguments, use Generate(int) method instead.");

            // Create buffers
            byte[] buffer = new byte[48];
            byte[] guid = new byte[16];
            byte[] randomNumbers = new byte[32];

            // Generate a random buffer of bytes
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(randomNumbers);

            // Generate a GUID 
            guid.Fill(Guid.NewGuid().ToByteArray());

            // Fill buffer
            buffer.Fill(new Collection<byte[]> { randomNumbers, guid });

            // Build the id to return
            StringBuilder id = new StringBuilder(); // FORMAT: Guid.Argument.Numbers.AllowDecode
            id.Append(guid.ToBase64(!allowDecode)); // Add GUID
            id.Append('.');
            id.Append(JsonConvert.SerializeObject(new[] { argument }).ToBase64(!allowDecode)); // Add the argument
            id.Append('.');
            id.Append(randomNumbers.ToBase64(true)); // Add the random numbers
            id.Append('.');
            id.Append(allowDecode ? "1" : "0");

            // Return the id
            return id.ToString();
        }

        /// <summary>
        /// Decodes an Id if it is allowed to
        /// </summary>
        /// <param name="id">The id to decode</param>
        public static string[] DecodeId(string id)
        {
            // Avoid bugs
            if (id.IsNullOrWhitespace()) throw new ArgumentNullException(nameof(id));

            // Split the id
            string[] parts = id.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            // Invalid id
            if (parts.Length <= 0 || parts.Length > 4) throw new InvalidOperationException("Invalid id format.");

            // Get if it can be decoded
            bool canDecode = parts[3] == "1";

            // Throw exception if cannot decode
            if (!canDecode) throw new InvalidOperationException("The id provided is not allowed to be decoded");

            List<string> idParts = new List<string>();

            // Get parts
            string guid = parts[0];
            var arguments = (JArray)JsonConvert.DeserializeObject(parts[1].FromBase64());

            // Store
            idParts.Add(new Guid(guid.FromBase64AsByteArray()).ToString());
            foreach(JToken argument in arguments)
                idParts.Add(argument.ToString());

            return idParts.ToArray();
        }
    }
}