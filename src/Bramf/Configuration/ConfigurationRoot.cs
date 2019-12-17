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
    public class ConfigurationRoot
    {
        #region Members

        private IEnumerable<ConfigurationProvider> mProviders;
        private ConfigurationOptions mOptions;

        #endregion

        #region Constructor

        internal ConfigurationRoot(IEnumerable<ConfigurationProvider> providers, ConfigurationOptions options)
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
        public TConfig Get<TConfig>() where TConfig : class, new()
        {
            // Try get provider
            ConfigurationProvider provider = mProviders.FirstOrDefault(x => x.Type == typeof(TConfig));

            // Not found
            if (provider == null)
                throw new ArgumentException($"Provider of type '{typeof(TConfig).ToString()}' not found.");

            // Return instance
            return (TConfig)provider.Instance;
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

        private void SaveProvider(ConfigurationProvider provider)
        {
            // Write to file
            string serialized = JsonConvert.SerializeObject(provider.Instance, Formatting.Indented); // Serialize the instance
            byte[] encrypted = null;

            // Should encrypt?
            if (provider.Options.Encrypt)
                using (var crypto = new Crypto(@"wF~b_Q,SWzwd2+/k]x)XGd_'j<g&ygcJ&yMLeK77W~[@#jtcHd9?z86t$mK5-fCHF>us[d3:6XJYi[9^"))
                    encrypted = (byte[])crypto.EncryptString(serialized, EncryptationMode.Bytes);

            // Write to file
            if (encrypted == null) File.WriteAllText(provider.FilePath, serialized);
            else File.WriteAllBytes(provider.FilePath, encrypted);
        }

        #endregion
    }
}