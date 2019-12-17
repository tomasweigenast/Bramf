using Bramf.Encryptation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bramf.Configuration
{
    /// <summary>
    /// Used to build <see cref="ConfigurationRoot"/>s
    /// </summary>
    public class ConfigurationBuilder
    {
        #region Members

        private string mBasePath;
        private IList<ConfigurationProvider> mProviders;
        private ConfigurationOptions mOptions;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="basePath">The base path where the configuration will save the implementations.</param>
        public ConfigurationBuilder(string basePath)
        {
            mBasePath = basePath;
            mOptions = new ConfigurationOptions();
            mProviders = new List<ConfigurationProvider>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Configures the <see cref="ConfigurationRoot"/>
        /// </summary>
        /// <param name="configureAction">The configure action.</param>
        public ConfigurationBuilder Configure(Action<ConfigurationOptions> configureAction)
        {
            configureAction.Invoke(mOptions);

            return this;
        }

        /// <summary>
        /// Adds a class as a provider
        /// </summary>
        /// <typeparam name="TConfig">The configuration type.</typeparam>
        public ConfigurationBuilder AddProvider<TConfig>() where TConfig : class, new()
        {
            Type configType = typeof(TConfig);

            // Try to get attribute
            ConfigAttribute attribute = (ConfigAttribute)configType.GetCustomAttributes(typeof(ConfigAttribute), false).FirstOrDefault();

            // Not found
            if (attribute == null)
                throw new ArgumentException($"The type '{configType.ToString()}' is not a valid configuration.");

            // Check if the provider is not added yet
            if (mProviders.Any(x => x.Type == configType))
                throw new Exception($"Configuration provider of type '{configType.ToString()}' already added.");

            // Get provider file path
            string filePath = Path.Combine(mBasePath, $"{attribute.Name ?? configType.Name}{mOptions.FileExtension}");
            
            // Get optinos
            var options = new ConfigurationProviderOptions();
            TConfig instance = null;

            // Check if it contains data
            using (var fs = new FileStream(filePath, FileMode.OpenOrCreate))
            using(var reader = new StreamReader(fs))
            {
                // If the file contains data, deserialize it
                if (fs.Length != 0)
                {
                    // Read the file
                    string content = reader.ReadToEnd();

                    // Decrypt if its encrypted
                    if (options.Encrypt)
                        using (var crypto = new Crypto(@"wF~b_Q,SWzwd2+/k]x)XGd_'j<g&ygcJ&yMLeK77W~[@#jtcHd9?z86t$mK5-fCHF>us[d3:6XJYi[9^"))
                            content = crypto.DecryptString(content, EncryptationMode.Bytes);

                    // Deserialize it
                    instance = JsonConvert.DeserializeObject<TConfig>(content);
                }
            }

            // If there is no data, create a new instance
            instance = new TConfig();

            // Write to file
            string serialized = JsonConvert.SerializeObject(instance, Formatting.Indented); // Serialize the instance
            byte[] encrypted = null;

            // Should encrypt?
            if (options.Encrypt)
                using (var crypto = new Crypto(@"wF~b_Q,SWzwd2+/k]x)XGd_'j<g&ygcJ&yMLeK77W~[@#jtcHd9?z86t$mK5-fCHF>us[d3:6XJYi[9^"))
                    encrypted = (byte[])crypto.EncryptString(serialized, EncryptationMode.Bytes);

            // Write to file
            if (encrypted == null) File.WriteAllText(filePath, serialized);
            else File.WriteAllBytes(filePath, encrypted);

            // Create provider
            ConfigurationProvider provider = new ConfigurationProvider(filePath, configType, instance, options);

            // Add provider
            mProviders.Add(provider);

            return this;
        }

        /// <summary>
        /// Adds a class as a provider
        /// </summary>
        /// <typeparam name="TConfig">The configuration type.</typeparam>
        /// <param name="configure">The configure action.</param>
        public ConfigurationBuilder AddProvider<TConfig>(Action<ConfigurationProviderOptions> configure) where TConfig : class, new()
        {
            Type configType = typeof(TConfig);

            // Try to get attribute
            ConfigAttribute attribute = (ConfigAttribute)configType.GetCustomAttributes(typeof(ConfigAttribute), false).FirstOrDefault();

            // Not found
            if (attribute == null)
                throw new ArgumentException($"The type '{configType.ToString()}' is not a valid configuration.");

            // Check if the provider is not added yet
            if (mProviders.Any(x => x.Type == configType))
                throw new Exception($"Configuration provider of type '{configType.ToString()}' already added.");

            // Get provider file path
            string filePath = Path.Combine(mBasePath, $"{attribute.Name ?? configType.Name}{mOptions.FileExtension}");

            // Get options
            var options = new ConfigurationProviderOptions();
            configure.Invoke(options);
            TConfig instance = null;

            // Check if it contains data
            using (var fs = new FileStream(filePath, FileMode.OpenOrCreate))
            using (var reader = new StreamReader(fs))
            {
                // If the file contains data, deserialize it
                if (fs.Length != 0)
                {
                    // Read the file
                    string content = reader.ReadToEnd();

                    // Decrypt if its encrypted
                    if (options.Encrypt)
                        using (var crypto = new Crypto(@"wF~b_Q,SWzwd2+/k]x)XGd_'j<g&ygcJ&yMLeK77W~[@#jtcHd9?z86t$mK5-fCHF>us[d3:6XJYi[9^"))
                            content = crypto.DecryptString(content, EncryptationMode.Bytes);

                    // Deserialize it
                    instance = JsonConvert.DeserializeObject<TConfig>(content);
                }
            }

            // If there is no data, create a new instance
            instance = new TConfig();

            // Write to file
            string serialized = JsonConvert.SerializeObject(instance, Formatting.Indented); // Serialize the instance
            byte[] encrypted = null;

            // Should encrypt?
            if (options.Encrypt)
                using (var crypto = new Crypto(@"wF~b_Q,SWzwd2+/k]x)XGd_'j<g&ygcJ&yMLeK77W~[@#jtcHd9?z86t$mK5-fCHF>us[d3:6XJYi[9^"))
                    encrypted = (byte[])crypto.EncryptString(serialized, EncryptationMode.Bytes);

            // Write to file
            if (encrypted == null) File.WriteAllText(filePath, serialized);
            else File.WriteAllBytes(filePath, encrypted);

            // Create provider
            ConfigurationProvider provider = new ConfigurationProvider(filePath, configType, instance, options);

            // Add provider
            mProviders.Add(provider);

            return this;
        }

        /// <summary>
        /// Builds the configuration root
        /// </summary>
        public ConfigurationRoot Build()
            => new ConfigurationRoot(mProviders, mOptions);

        #endregion
    }
}