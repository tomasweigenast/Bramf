using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Bramf.Serializer
{
    /// <summary>
    /// Provides methods to serialize objects using JSON format
    /// </summary>
    public static class BinarySerializer
    {
        /// <summary>
        /// Serializes an object
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize</typeparam>
        /// <param name="objToSerialize">Object to serialize</param>
        public static byte[] Serialize<T>(T objToSerialize)
        {
            // Create memory stream to set the bytes
            using (var memoryStream = new MemoryStream())
            {
                // Create the binary formatter
                var bFormatter = new BinaryFormatter();

                // Serialize the content
                bFormatter.Serialize(memoryStream, objToSerialize);

                // Return the content
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Serializes an object
        /// </summary>
        /// <param name="buffer">The byte array that contains the bytes to deserialize</param>
        public static T Deserialize<T>(byte[] buffer)
        {
            // Create memory stream to set the bytes
            using (var memoryStream = new MemoryStream(buffer))
            {
                // Create the binary formatter
                var bFormatter = new BinaryFormatter();

                // Deserialize the content
                var obj = bFormatter.Deserialize(memoryStream);

                // Return the content
                return (T)obj;
            }
        }

        /// <summary>
        /// Serializes an object and saves it to a file
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize</typeparam>
        /// <param name="objToSerialize">Object to serialize</param>
        /// <param name="path">The path to the file where the data will be saved</param>
        public static void SerializeToFile<T>(T objToSerialize, string path)
        {
            // Create file stream of the file
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                // Create the binary formatter
                var bFormatter = new BinaryFormatter();

                // Serialize the content
                bFormatter.Serialize(fileStream, objToSerialize);
            }
        }

        /// <summary>
        /// Deserializes an object saved in a file
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize</typeparam>
        /// <param name="path">The path to the file where the data will be saved</param>
        public static T DeserializeFromFile<T>(string path)
        {
            // Create file stream of the file
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                // Create the binary formatter
                var bFormatter = new BinaryFormatter();

                // Serialize the content
                var obj = (T)bFormatter.Deserialize(fileStream);

                return obj;
            }
        }
    }
}
