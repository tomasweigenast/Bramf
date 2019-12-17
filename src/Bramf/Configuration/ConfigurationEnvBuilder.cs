using Bramf.Encryptation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Bramf.Configuration
{
    /// <summary>
    /// Used to build <see cref="ConfigurationEnvironment"/>s
    /// </summary>
    public class ConfigurationEnvBuilder
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
        public ConfigurationEnvBuilder(string basePath)
        {
            mBasePath = basePath;
            mOptions = new ConfigurationOptions();
            mProviders = new List<ConfigurationProvider>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Configures the <see cref="ConfigurationEnvironment"/>
        /// </summary>
        /// <param name="configureAction">The configure action.</param>
        public ConfigurationEnvBuilder Configure(Action<ConfigurationOptions> configureAction)
        {
            configureAction.Invoke(mOptions);

            return this;
        }

        /// <summary>
        /// Adds a class as a provider
        /// </summary>
        /// <typeparam name="TConfig">The configuration type.</typeparam>
        public ConfigurationEnvBuilder AddProvider<TConfig>() where TConfig : class, new()
        {
            mAddProvider<TConfig>(null);

            return this;
        }

        /// <summary>
        /// Adds a class as a provider
        /// </summary>
        /// <typeparam name="TConfig">The configuration type.</typeparam>
        /// <param name="configure">The configure action.</param>
        public ConfigurationEnvBuilder AddProvider<TConfig>(Action<ConfigurationProviderOptions> configure) where TConfig : class, new()
        {
            // Get options
            ConfigurationProviderOptions options = new ConfigurationProviderOptions();
            configure.Invoke(options);

            // Add provider
            mAddProvider<TConfig>(options);

            return this;
        }

        /// <summary>
        /// Builds the configuration root
        /// </summary>
        public ConfigurationEnvironment Build()
            => new ConfigurationEnvironment(mProviders, mOptions);

        #endregion

        #region Helper Methods

        private void mAddProvider<TProvider>(ConfigurationProviderOptions options) where TProvider : class, new()
        {
            Type configType = typeof(TProvider);

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
            TProvider instance = null;

            // Read the file
            byte[] fileContent = File.ReadAllBytes(filePath);
            string deserialized = null;

            // Decrypt if its encrypted
            if (options.Encrypt)
                using (var crypto = new Crypto(@"wF~b_Q,SWzwd2+/k]x)XGd_'j<g&ygcJ&yMLeK77W~[@#jtcHd9?z86t$mK5-fCHF>us[d3:6XJYi[9^"))
                    deserialized = crypto.DecryptData(fileContent, EncryptationMode.Bytes);

            // Otherwise, just encode
            else
                deserialized = Encoding.UTF8.GetString(fileContent);

            // Deserialize it
            instance = JsonConvert.DeserializeObject<TProvider>(deserialized);

            // If there is no data, create a new instance
            instance = new TProvider();

            // Write to file
            string serialized = JsonConvert.SerializeObject(instance, Formatting.Indented); // Serialize the instance
            byte[] encrypted = null;

            // Should encrypt?
            if (options.Encrypt)
                using (var crypto = new Crypto(@"wF~b_Q,SWzwd2+/k]x)XGd_'j<g&ygcJ&yMLeK77W~[@#jtcHd9?z86t$mK5-fCHF>us[d3:6XJYi[9^"))
                    encrypted = (byte[])crypto.EncryptData(serialized, EncryptationMode.Bytes);

            // Write to file
            if (encrypted == null) File.WriteAllText(filePath, serialized);
            else File.WriteAllBytes(filePath, encrypted);

            // Create provider
            ConfigurationProvider provider = new ConfigurationProvider(filePath, configType, instance, options);

            // Add provider
            mProviders.Add(provider);
        }

        #endregion
    }
}