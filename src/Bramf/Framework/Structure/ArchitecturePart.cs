using Bramf.App;
using System.Reflection;

namespace Bramf.Structure
{
    /// <summary>
    /// Represents a part of the full architecture of an application
    /// </summary>
    public class ArchitecturePart
    {
        #region Properties

        /// <summary>
        /// The version of the architecture part
        /// </summary>
        public SemanticVersion Version { get; set; }

        /// <summary>
        /// The name of the architecture part
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The assembly that represents the architecture part
        /// </summary>
        public Assembly Assembly { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new architecture part
        /// </summary>
        /// <param name="assembly">The representing assembly</param>
        /// <param name="name">The name of the architecture part</param>
        public ArchitecturePart(Assembly assembly, string name)
        {
            Assembly = assembly;
            Name = name;
            Version = new SemanticVersion(assembly.GetName().Version);
        }

        /// <summary>
        /// Creates a new architecture part
        /// </summary>
        /// <param name="assembly">The representing assembly.</param>
        /// <param name="name">The name of the architecture part.</param>
        /// <param name="version">The start version of the assembly</param>
        public ArchitecturePart(Assembly assembly, string name, SemanticVersion version)
        {
            Assembly = assembly;
            Name = name;
            Version = version;
        }

        #endregion
    }
}
