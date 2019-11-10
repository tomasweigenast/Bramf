using Bramf.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bramf.Http.Serialization
{
    /// <summary>
    /// Serialize key-value pairs
    /// </summary>
    public class FormUrlEncodedSerializer : BaseContentSerializer
    {
        /// <summary>
        /// Creates a new key-value pair serializer
        /// </summary>
        public FormUrlEncodedSerializer() : base("application/x-www-form-urlencoded") { }

        /// <summary>
        /// Deserializes content
        /// </summary>
        public override T Deserialize<T>(string content)
            => throw new InvalidOperationException("URL Form Encoding cannot deserialize data.");

        /// <summary>
        /// Deserializes content
        /// </summary>
        public override object DeserializeAnonymous(string content)
            => throw new InvalidOperationException("URL Form Encoding cannot deserialize data.");

        /// <summary>
        /// Serializes content
        /// </summary>
        public override string SerializeContent(object content)
        {
            if (content is IEnumerable<KeyValuePair<string, string>> keyValuePair)
            {
                StringBuilder builder = new StringBuilder();
                foreach(var pair in keyValuePair)
                {
                    if (builder.Length > 0)
                        builder.Append('&');

                    builder.Append(Encode(pair.Key));
                    builder.Append('=');
                    builder.Append(Encode(pair.Value));
                }

                return builder.ToString();
            }
            else
                throw new ArgumentException($"The content must be a KeyValuePair collection. It is: {content.GetType()}");
        }

        private static string Encode(string data)
        {
            if (data.IsNullOrWhitespace())
                return string.Empty;

            return Uri.EscapeDataString(data).Replace("%20", "+");
        }
    }
}
