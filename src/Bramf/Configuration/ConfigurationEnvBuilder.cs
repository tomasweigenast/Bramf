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
            mAddProvider<TConfig>(new ConfigurationProviderOptions());

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

            // Create provider
            ConfigurationProvider provider = new ConfigurationProvider(filePath, configType, options);

            // Add provider
            mProviders.Add(provider);
        }

        #endregion
    }
}