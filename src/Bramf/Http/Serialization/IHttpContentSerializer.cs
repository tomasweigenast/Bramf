using Bramf.Extensions;
using System;

namespace Bramf.Http.Serialization
{
    /// <summary>
    /// Base HTTP content serializer
    /// </summary>
    public interface IHttpContentSerializer
    {
        /// <summary>
        /// The serializer mime type
        /// </summary>
        string MimeType { get; }

        /// <summary>
        /// Deserializes the content
        /// </summary>
        /// <param name="content">The content to deserialize</param>
        object DeserializeAnonymous(string content);

        /// <summary>
        /// Deserializes the content
        /// </summary>
        /// <typeparam name="T">The type of content to deserialize</typeparam>
        /// <param name="content">The string content to deserialize</param>
        T Deserialize<T>(string content);

        /// <summary>
        /// Serializes content
        /// </summary>
        /// <param name="content">The content to serialize</param>
        string SerializeContent(object content);
    }

    /// <summary>
    /// A class that should be implemented by each content serializer
    /// </summary>
    public abstract class BaseContentSerializer : IHttpContentSerializer
    {
        /// <summary>
        /// The serializer mime type
        /// </summary>
        public string MimeType { get; }

        /// <summary>
        /// Creates a new content serializer
        /// </summary>
        /// <param name="mimeType">The mime type of the serializer</param>
        public BaseContentSerializer(string mimeType)
        {
            // Avoid nulls
            if (mimeType.IsNullOrWhitespace()) throw new ArgumentNullException(nameof(mimeType));

            MimeType = mimeType;
        }

        /// <summary>
        /// Deserializes the content
        /// </summary>
        /// <param name="content">The content to deserialize</param>
        public abstract T Deserialize<T>(string content);

        /// <summary>
        /// Deserializes the content
        /// </summary>
        /// <typeparam name="T">The type of content to deserialize</typeparam>
        /// <param name="content">The string content to deserialize</param>
        public abstract object DeserializeAnonymous(string content);

        /// <summary>
        /// Serializes content
        /// </summary>
        /// <param name="content">The content to serialize</param>
        public abstract string SerializeContent(object content);
    }
}
