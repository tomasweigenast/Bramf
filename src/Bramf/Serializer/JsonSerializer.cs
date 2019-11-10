using Newtonsoft.Json;
using System;
using System.IO;

namespace Bramf.Serializer
{
    /// <summary>
    /// Provides methods to serialize objects using JSON format
    /// </summary>
    public static class JsonSerializer
    {
        /// <summary>
        /// Serializes an object and saves it to a file
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize</typeparam>
        /// <param name="objToSerialize">The object to serialize</param>
        /// <param name="path">The file path to write the object to</param>
        /// <param name="format">Formats the json to be indented</param>
        public static void SerializeToFile<T>(this T objToSerialize, string path, bool format = true)
        {
            // Serialize object
            var json = JsonConvert.SerializeObject(objToSerialize, format ? Formatting.Indented : Formatting.None);

            try
            {
                // Write to file
                File.WriteAllText(path, json);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deserializes an object from a file
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize</typeparam>
        /// <param name="path">The file path to read</param>
        public static T DeserializeFromFile<T>(string path)
        {
            try
            {
                // Create stream to read the file
                using (var reader = new StreamReader(path))
                {
                    // Read whole file
                    var text = reader.ReadToEnd();

                    // Check if the file has content
                    if (text.Length <= 0)
                        return default(T);

                    // Deserialize and return
                    return JsonConvert.DeserializeObject<T>(text);
                }
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
