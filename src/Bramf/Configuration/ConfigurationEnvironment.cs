using Bramf.Configuration.Abstractions;
using Bramf.Encryptation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bramf.Configuration
{
    /// <summary>
    /// Represents a configuration root
    /// </summary>
    public class ConfigurationEnvironment : IConfigurationEnvironment
    {
        #region Members

        private IEnumerable<ConfigurationProvider> mProviders;
        private ConfigurationOptions mOptions;

        #endregion

        #region Constructor

        internal ConfigurationEnvironment(IEnumerable<ConfigurationProvider> providers, ConfigurationOptions options)
        {
            mProviders = providers;
            mOptions = options;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a configuration provider instance
        /// </summary>
        /// <typeparam name="TConfig">The configuration provider type.</typeparam>
        public TConfig Get<TConfig>()
            where TConfig : class, new()
        {
            // Try get provider
            ConfigurationProvider provider = mProviders.FirstOrDefault(x => x.Type == typeof(TConfig));

            // Not found
            if (provider == null)
                throw new ArgumentException($"Provider of type '{typeof(TConfig).ToString()}' not found.");

            // Return instance
            return (TConfig)provider.GetInstance();
        }
        
        /// <summary>
        /// Edits a configuration without loading it to memory
        /// </summary>
        /// <typeparam name="TConfig">The configuration type to edit.</typeparam>
        /// <param name="editAction">The action used to edit.</param>
        public void BeginEdit<TConfig>(Action<TConfig> editAction)
            where TConfig : class, new()
        {
            // Try get provider
            ConfigurationProvider provider = mProviders.FirstOrDefault(x => x.Type == typeof(TConfig));

            // Not found
            if (provider == null)
                throw new ArgumentException($"Provider of type '{typeof(TConfig).ToString()}' not found.");

            // Get a provider instance
            TConfig initialValue = (TConfig)provider.GetInstance();

            // Edit
            editAction.Invoke(initialValue);

            // Save to file
            SaveProvider(provider, initialValue);
        }

        /// <summary>
        /// Saves all the providers to their specific file
        /// </summary>
        public void Save()
        {
            foreach (var provider in mProviders)
                SaveProvider(provider);
        }

        #endregion

        #region Helper Methods

        private void SaveProvider(ConfigurationProvider provider, object newValue = null)
        {
            // Ignore if there is no instnace
            if (provider.Instance == null && newValue == null)
                return;

            // Get value
            if (newValue == null)
                newValue = provider.Instance;

            // Serialize
            string serialized = JsonConvert.SerializeObject(newValue, Formatting.Indented); // Serialize the instance
            byte[] encrypted = null;

            // Should encrypt?
            if (provider.Options.Encrypt)
                encrypted = Crypto.EncryptString(serialized, @"wF~b_Q,SWzwd2+/k]x)XGd_'j<g&ygcJ&yMLeK77W~[@#jtcHd9?z86t$mK5-fCHF>us[d3:6XJYi[9^");

            // Write to file
            if (encrypted == null) File.WriteAllText(provider.FilePath, serialized);
            else File.WriteAllBytes(provider.FilePath, encrypted);
        }

        #endregion
    }
}