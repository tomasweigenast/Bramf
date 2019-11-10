using System;

namespace Bramf.Http.Serialization
{
    /// <summary>
    /// Accepts any content type
    /// </summary>
    public class UniversalContentSerializer : BaseContentSerializer
    {
        /// <summary>
        /// Creates a new universal content serializer
        /// </summary>
        public UniversalContentSerializer() : base("*/*") { }

        public override T Deserialize<T>(string content)
             => throw new InvalidOperationException("Cannot deserialize using Universal.");

        public override object DeserializeAnonymous(string content)
            => throw new InvalidOperationException("Cannot deserialize using Universal.");

        public override string SerializeContent(object content)
            => throw new InvalidOperationException("Cannot deserialize using Universal.");
    }
}
