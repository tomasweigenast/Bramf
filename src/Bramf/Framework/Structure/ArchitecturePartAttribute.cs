using Bramf.App;
using System;

namespace Bramf.Structure
{
    /// <summary>
    /// Indicates that the applied class is the base for a architecture part
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ArchitecturePartAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// The name of the application part
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The version of the part
        /// </summary>
        public SemanticVersion Version { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="name">The name of the application part.</param>
        /// <param name="version">The version of the application part.</param>
        public ArchitecturePartAttribute(string name, string version)
        {
            Name = name;
            Version = new SemanticVersion(version);
        }

        #endregion
    }
}
