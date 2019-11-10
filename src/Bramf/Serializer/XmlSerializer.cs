using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Bramf.Serializer
{
    // compile with: -doc:Bitture.Bramf.xml
    /// <summary>
    /// Provides methods to serialize objects using XML format
    /// </summary>
    public static class XmlSerializer
    {
        /// <summary>
        /// Serializes an object
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize</typeparam>
        /// <param name="objToSerialize">Object to serialize</param>
        public static string Serialize<T>(this T objToSerialize)
        {
            try
            {
                // Create serializer
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

                // Create stream to store the serialized content
                using (var stream = new MemoryStream())
                using (var xmlWriter = XmlWriter.Create(stream))
                {
                    // Serialize
                    serializer.Serialize(xmlWriter, objToSerialize);

                    // Return serialized
                    return xmlWriter.ToString();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Deserializes an object
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize</typeparam>
        /// <param name="content">The string to deserialize</param>
        public static T Deserialize<T>(string content)
        {
            try
            {
                // Create serializer
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

                // Create stream to store the deserialized content
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
                {
                    // Deserialize
                    var obj = (T)serializer.Deserialize(stream);

                    // Return deserialized
                    return obj;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Serializes an object and saves it to fil
        /// <para></para>
        /// </summary>
        /// <typeparam name="T">The type of object to write to file</typeparam>
        /// <param name="path">The file path to write the object to</param>
        /// <param name="objToSerialize">The object to serialize</param>
        public static void SerializeToFile<T>(string path, T objToSerialize)
        {
            try
            {
                // Create serializer
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

                // Create writer
                using (var xmlWriter = XmlWriter.Create(path))
                {
                    // Serialize and write to file
                    serializer.Serialize(xmlWriter, objToSerialize);
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Deserializes an object reading it from a file
        /// </summary>
        /// <typeparam name="T">The type of object to read from the file.</typeparam>
        /// <param name="path">The file path to read.</param>
        public static T DeserializeFromFile<T>(string path)
        {
            try
            {
                // Create serializer
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

                // Read from file
                using (var reader = new StreamReader(path))
                {
                    // Read, deserialize and return
                    return (T)serializer.Deserialize(reader);
                }
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
