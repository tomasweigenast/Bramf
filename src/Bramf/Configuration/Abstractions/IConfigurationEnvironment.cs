using System;

namespace Bramf.Configuration.Abstractions
{
    /// <summary>
    /// Represents a configuration
    /// </summary>
    public interface IConfigurationEnvironment
    {
        /// <summary>
        /// Returns a configuration
        /// </summary>
        /// <typeparam name="TConfig">The configuration type.</typeparam>
        TConfig Get<TConfig>() where TConfig : class, new();

        /// <summary>
        /// Edits a configuration without loading it to memory
        /// </summary>
        /// <typeparam name="TConfig">The configuration type to edit.</typeparam>
        /// <param name="editAction">The action used to edit.</param>
        void BeginEdit<TConfig>(Action<TConfig> editAction) where TConfig : class, new();

        /// <summary>
        /// Saves all the configurations
        /// </summary>
        void Save();
    }
}