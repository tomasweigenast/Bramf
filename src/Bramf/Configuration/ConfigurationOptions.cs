namespace Bramf.Configuration
{
    /// <summary>
    /// Options for a <see cref="ConfigurationRoot"/>
    /// </summary>
    public class ConfigurationOptions
    {
        /// <summary>
        /// The extension used in the files.
        /// Defaults to .bfc
        /// </summary>
        public string FileExtension { get; set; } = ".bfc";
    }
}