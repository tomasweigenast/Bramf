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
    }
}