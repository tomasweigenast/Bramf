using System;

namespace Bramf.Http.Serialization
{
    /// <summary>
    /// Treat request content as HTML
    /// </summary>
    public class HtmlContentSerializer : BaseContentSerializer
    {
        /// <summary>
        /// Creates a new html content serializer
        /// </summary>
        public HtmlContentSerializer() : base("text/html") { }

        public override T Deserialize<T>(string content)
            => throw new InvalidOperationException("Cannot deserialize using HTML.");

        public override object DeserializeAnonymous(string content)
            => throw new InvalidOperationException("Cannot deserialize using HTML.");

        public override string SerializeContent(object content)
             => throw new InvalidOperationException("Cannot serialize using HTML.");
    }
}
