namespace Bramf.Configuration
{
    /// <summary>
    /// The options for a <see cref="ConfigurationProvider"/>
    /// </summary>
    public class ConfigurationProviderOptions
    {
        /// <summary>
        /// Indicates if the file must be encrypted.
        /// Defaults to false.
        /// </summary>
        public bool Encrypt { get; set; }

        /// <summary>
        /// Indicates if the configuration instance must be loaded
        /// in to the memory after retrieve from file.
        /// Defaults to true
        /// </summary>
        public bool Load { get; set; } = true;
    }
}