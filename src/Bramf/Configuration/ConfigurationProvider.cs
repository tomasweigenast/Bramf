using System;

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
        /// The provider instance
        /// </summary>
        public object Instance { get; set; }

        /// <summary>
        /// An unique id for the provider
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// The file path to the provider
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// The options for the provider
        /// </summary>
        public ConfigurationProviderOptions Options { get; }

        /// <summary>
        /// Creates a new <see cref="ConfigurationProvider"/>
        /// </summary>
        /// <param name="filePath">The file path to the provider</param>
        /// <param name="instance">The provider instance</param>
        /// <param name="type">The provider configuration type</param>
        /// <param name="options">The options for the provider.</param>
        public ConfigurationProvider(string filePath, Type type, object instance, ConfigurationProviderOptions options)
        {
            Id = Guid.NewGuid().ToString();
            Type = type;
            FilePath = filePath;
            Instance = instance;
            Options = options ?? new ConfigurationProviderOptions();
        }
    }
}