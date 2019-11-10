using Bramf.Extensions;
using Bramf.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bramf
{
    /// <summary>
    /// Defines methods to get information about the current runtime
    /// </summary>
    public class Runtime
    {
        #region Members

        // The unique runtime instance. One per application
        private static Runtime mInstanceRuntime;

        #endregion

        #region Properties

        /// <summary>
        /// The assembly who is running Bramf
        /// </summary>
        public Assembly RunningAssembly { get; }

        /// <summary>
        /// A list of all parts of the application
        /// </summary>
        public IReadOnlyList<ArchitecturePart> Parts { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        internal Runtime(ArchitecturePart[] parts)
        {
            RunningAssembly = Assembly.GetEntryAssembly();

            // Skip is there is no parts
            if (parts == null)
                return;

            var partsD = parts.ToList();

            // Remove entry assembly is added
            if (partsD.Any(x => x.Assembly == RunningAssembly))
                partsD.Remove(partsD.First(x => x.Assembly == RunningAssembly));

            // Load
            Parts = partsD.AsReadOnly();
        }

        #endregion

        #region Methods

        #region Static

        /// <summary>
        /// Starts the creation of a new runtime
        /// </summary>
        internal static RuntimeBuilder CreateRuntime()
        {
            // Avoid duplicated runtime instances
            if (mInstanceRuntime != null) throw new InvalidOperationException("Already exists a runtime instance.");

            return new RuntimeBuilder();
        }

        /// <summary>
        /// Makes a check verification indicating if a new runtime can be created
        /// </summary>
        public static bool Verify() => mInstanceRuntime == null;

        #endregion

        /// <summary>
        /// Gets a part by its identifier
        /// </summary>
        /// <param name="name">The name of the identifier.</param>
        public ArchitecturePart GetPart(string name)
        {
            // Avoid bugs
            if (name.IsNullOrWhitespace()) throw new ArgumentNullException(nameof(name));
            if (Parts == null) throw new Exception("There are no loaded parts.");
            if (!Parts.Any(x => x.Name == name)) throw new DllNotFoundException($"Cannot find any part with the name {name}.");

            // Gets the part and return
            return Parts.FirstOrDefault(x => x.Name == name);
        }

        /// <summary>
        /// Gets a part by its assembly
        /// </summary>
        /// <param name="assembly">The assembly to figure out the architecture.</param>
        public ArchitecturePart GetPart(Assembly assembly)
        {
            // Avoid bugs
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            if (Parts == null) throw new Exception("Unknown exception.");
            if (!Parts.Any(x => x.Assembly == assembly)) throw new DllNotFoundException($"Cannot find any part with the assembly {assembly.GetName().Name}.");

            // Gets the part and return
            return Parts.FirstOrDefault(x => x.Assembly == assembly);
        }

        #endregion
    }

    /// <summary>
    /// Helps creating a <see cref="Runtime"/>
    /// </summary>
    internal class RuntimeBuilder
    {
        #region Members

        // A list of all assemblies to add to the runtime
        private Dictionary<string, Assembly> mAssemblies = new Dictionary<string, Assembly>();

        #endregion

        #region Methods

        /// <summary>
        /// Adds an assembly to the runtime
        /// </summary>
        /// <param name="assembly">The assembly to add.</param>
        /// <param name="identifier">An optional identifier name, otherwise, the name of the assembly will be used.</param>
        public RuntimeBuilder AddAssembly(Assembly assembly, string identifier = null)
        {
            // Avoid bugs
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            // Get assembly name
            if (identifier.IsNullOrWhitespace())
                identifier = assembly.GetName().Name;

            // Avoid duplicated assemblies
            if (mAssemblies.ContainsKey(identifier) || mAssemblies.ContainsValue(assembly))
                throw new InvalidOperationException($"{identifier} already added.");

            // Add assembly
            mAssemblies.Add(identifier, assembly);

            return this;
        }

        /// <summary>
        /// Builds the runtime
        /// </summary>
        public Runtime Build()
        {
            // Avoid duplicated runtime instances
            if (!Runtime.Verify()) throw new InvalidOperationException("Cannot create two runtime instances.");

            // Create runtime and return
            return new Runtime(null);
        }

        #endregion
    }
}