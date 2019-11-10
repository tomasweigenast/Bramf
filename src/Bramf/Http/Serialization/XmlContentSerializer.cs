using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Bramf.Http.Serialization
{
    /// <summary>
    /// Serializes HTTP content using XML
    /// </summary>
    public class XmlContentSerializer : BaseContentSerializer
    {
        #region Constructor

        /// <summary>
        /// Creates a new XML content serializer
        /// </summary>
        public XmlContentSerializer() : base("text/xml") { }

        #endregion

        #region Methods

        /// <summary>
        /// Deserialize content
        /// </summary>
        public override T Deserialize<T>(string content)
        {
            var serializer = new XmlSerializer(typeof(T));
            T deserialized = default;
            using(var reader = new StringReader(content))
                deserialized = (T)serializer.Deserialize(reader);

            return deserialized;
        }

        /// <summary>
        /// Deserialize content
        /// </summary>
        public override object DeserializeAnonymous(string content)
        {
            var serializer = new XmlSerializer(typeof(object));
            object deserialized = null;
            using (var reader = new StringReader(content))
                deserialized = serializer.Deserialize(reader);

            return deserialized;
        }

        /// <summary>
        /// Serialize content
        /// </summary>
        public override string SerializeContent(object content)
        {
            var serializer = new XmlSerializer(content.GetType());
            StringBuilder stringBuilder = new StringBuilder();
            using(var writer = XmlWriter.Create(stringBuilder))
                serializer.Serialize(writer, content);

            return stringBuilder.ToString();
        }

        #endregion
    }
}
