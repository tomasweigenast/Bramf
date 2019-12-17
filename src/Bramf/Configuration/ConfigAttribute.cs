using System;

namespace Bramf.Configuration
{
    /// <summary>
    /// Attribute that every class that will be act as a provider must implement
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigAttribute : Attribute
    {
        /// <summary>
        /// An optional name for the configuration.
        /// If its no set, type name will be used.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes the <see cref="ConfigAttribute"/>
        /// </summary>
        public ConfigAttribute()
        {

        }
    }
}