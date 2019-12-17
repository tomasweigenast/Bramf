using Bramf.Encryptation;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace Bramf.Configuration
{
    /// <summary>
    /// Represents a provider that serves a configuration object
    /// </summary>
    public class ConfigurationProvider
    {
        /// <summary>
        /// The provider configuration type
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// An unique id for the provider
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// The file path to the provider
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// The provider instnace
        /// </summary>
        public object Instance { get; set; }

        /// <summary>
        /// The options for the provider
        /// </summary>
        public ConfigurationProviderOptions Options { get; }

        /// <summary>
        /// Creates a new <see cref="ConfigurationProvider"/>
        /// </summary>
        /// <param name="filePath">The file path to the provider</param>
        /// <param name="type">The provider configuration type</param>
        /// <param name="options">The options for the provider.</param>
        public ConfigurationProvider(string filePath, Type type, ConfigurationProviderOptions options)
        {
            Id = Guid.NewGuid().ToString();
            Type = type;
            FilePath = filePath;
            Options = options ?? new ConfigurationProviderOptions();

            GetDefaults();
        }

        /// <summary>
        /// Returns the configuration provider instance from the file
        /// </summary>
        public object GetInstance()
        {
            object instance;

            // Create the file if it does not exists
            using (var fs = new FileStream(FilePath, FileMode.OpenOrCreate)) { }

            // Read the file
            byte[] fileContent = File.ReadAllBytes(FilePath);
            string deserialized = null;

            // If there is data to deserialize
            if (fileContent.Length > 0)
            {
                // Decrypt if its encrypted
                if (Options.Encrypt)
                    deserialized = Crypto.DecryptString(fileContent, "wF~b_Q,SWzwd2+/k]x)XGd_'j<g&ygcJ&yMLeK77W~[@#jtcHd9?z86t$mK5-fCHF>us[d3:6XJYi[9^");

                // Otherwise, just encode
                else
                    deserialized = Encoding.UTF8.GetString(fileContent);

                // Deserialize it
                instance = JsonConvert.DeserializeObject(deserialized, Type);
            }
            else
            {
                // If there is no data, create a new instance
                instance = Activator.CreateInstance(Type);

                // Write to file
                string serialized = JsonConvert.SerializeObject(instance, Formatting.Indented); // Serialize the instance
                byte[] encrypted = null;

                // Should encrypt?
                if (Options.Encrypt)
                    encrypted = Crypto.EncryptString(serialized, "wF~b_Q,SWzwd2+/k]x)XGd_'j<g&ygcJ&yMLeK77W~[@#jtcHd9?z86t$mK5-fCHF>us[d3:6XJYi[9^");

                // Write to file
                if (encrypted == null) File.WriteAllText(FilePath, serialized);
                else File.WriteAllBytes(FilePath, encrypted);
            }

            // Load to memory?
            if (Options.Load)
                Instance = instance;

            return instance;
        }

        /// <summary>
        /// Gets a default instance an save to file
        /// </summary>
        private void GetDefaults()
        { 
            if(!File.Exists(FilePath))
            {
                // Create default instance
                object instance = Activator.CreateInstance(Type);

                // Write to file
                using (var fs = new FileStream(FilePath, FileMode.Create))
                {
                    // Should encrypt?
                    if (Options.Encrypt)
                    {
                        byte[] encrypted = Crypto.EncryptString(JsonConvert.SerializeObject(instance, Formatting.Indented), "wF~b_Q,SWzwd2+/k]x)XGd_'j<g&ygcJ&yMLeK77W~[@#jtcHd9?z86t$mK5-fCHF>us[d3:6XJYi[9^");
                        fs.Write(encrypted, 0, encrypted.Length);
                    }
                    else
                    {
                        using (StreamWriter writer = new StreamWriter(fs))
                            writer.Write(JsonConvert.SerializeObject(instance, Formatting.Indented));
                    }
                }
            }
        }
    }
}