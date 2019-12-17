namespace Bramf.Configuration.Abstractions
{
    /// <summary>
    /// Represents a configuration
    /// </summary>
    public interface IConfigurationRoot
    {
        /// <summary>
        /// Returns a configuration
        /// </summary>
        /// <typeparam name="TConfig">The configuration type.</typeparam>
        TConfig Get<TConfig>() where TConfig : class, new();

        /// <summary>
        /// Saves all the configurations
        /// </summary>
        void Save();
    }
}