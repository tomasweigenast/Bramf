using Newtonsoft.Json;

namespace Bramf.Http.Serialization
{
    /// <summary>
    /// Uses JSON to deserialize content
    /// </summary>
    public class JsonContentSerializer : BaseContentSerializer
    {
        #region Constructors

        /// <summary>
        /// Creates a new JSON content serializer
        /// </summary>
        public JsonContentSerializer() : base("application/json") { }

        #endregion

        #region Methods

        /// <summary>
        /// Deserializes content
        /// </summary>
        public override T Deserialize<T>(string content)
            => JsonConvert.DeserializeObject<T>(content);

        /// <summary>
        /// Deserializes content
        /// </summary>
        public override object DeserializeAnonymous(string content)
            => JsonConvert.DeserializeObject(content);

        /// <summary>
        /// Serializes content
        /// </summary>
        /// <param name="content">The content to serialize</param>
        public override string SerializeContent(object content)
            => JsonConvert.SerializeObject(content);

        #endregion
    }
}
