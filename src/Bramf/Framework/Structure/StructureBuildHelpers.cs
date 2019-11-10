using Bramf.App;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bramf.Structure
{
    /// <summary>
    /// Helpers methods to inject and create better structured apps
    /// </summary>
    public static class StructureBuildHelpers
    {
        /// <summary>
        /// Setups an application with a custom behaviour
        /// </summary>
        /// <typeparam name="TApp">The type of behaviour</typeparam>
        public static FrameworkConstruction ConstructBehaviour<TApp>(this FrameworkConstruction construction, Action<StructureBuilder> builder = null)
            where TApp : IAppBehaviour, new()
        {
            var app = new TApp(); // Create instance of TApp
            app.CopyNeededFiles(); // Copy needed files

            // Add structure if builder is not null
            Runtime runtime = default;
            if(builder != null)
            {
                // Invoke structure builder
                StructureBuilder sb = new StructureBuilder();
                builder.Invoke(sb);

                // Create runtime
                if (!Runtime.Verify()) throw new InvalidOperationException("Runtime already initialized."); // Avoid duplicated runtime
                runtime = new Runtime(sb.iParts.ToArray());
            }

            // Create without part runtime
            if (!Runtime.Verify()) throw new InvalidOperationException("Runtime already initialized."); // Avoid duplicated runtime
            runtime = new Runtime(null);

            app.Runtime = runtime; // Set runtime

            // Inject
            construction.Services.AddSingleton(typeof(BaseAppBehaviour), app);

            return construction;
        }

        /// <summary>
        /// Setups an application with a custom behaviour
        /// </summary>
        /// <typeparam name="TApp">The type of behaviour</typeparam>
        /// <typeparam name="TStorage">The storage type</typeparam>
        public static FrameworkConstruction ConstructBehaviour<TApp, TStorage>(this FrameworkConstruction construction, Action<StructureBuilder> builder = null)
            where TApp : IAppBehaviour<TStorage>, new()
            where TStorage : IAppStorage
        {
            // Application startup
            var app = new TApp(); // Create instance of TApp
            app.Storage.CreateDirectories(); // Create storage directories
            app.CopyNeededFiles(); // Copy needed files

            Runtime runtime;

            if(builder != null)
            {
                // Invoke structure builder
                StructureBuilder sb = new StructureBuilder();
                builder.Invoke(sb);

                // Create runtime
                if (!Runtime.Verify()) throw new InvalidOperationException("Runtime already initialized."); // Avoid duplicated runtime
                runtime = new Runtime(sb.iParts.ToArray());
            }
            else
            {
                // Create without part runtime
                if (!Runtime.Verify()) throw new InvalidOperationException("Runtime already initialized."); // Avoid duplicated runtime
                runtime = new Runtime(null);
            }

            app.Runtime = runtime; // Set runtime

            // Inject services
            construction.Services.AddSingleton(typeof(IAppBehaviour<IAppStorage>), app);

            return construction;
        }
    }

    /// <summary>
    /// Helps creating app structures
    /// </summary>
    public class StructureBuilder
    {
        #region Internal properties

        internal List<ArchitecturePart> iParts = new List<ArchitecturePart>();

        #endregion

        #region Methods

        /// <summary>
        /// Adds assemblies to the parts
        /// </summary>
        /// <param name="assemblies">The assemblies to add.</param>
        public StructureBuilder WithAssemblies(params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                // Get part
                ArchitecturePart part = ArchitectureManager.GetArchitecturePartFromFile(assembly);

                // If part is null, skip
                if (part == null)
                    continue;

                // Add part
                iParts.Add(part);
            }

            return this;
        }

        #endregion
    }
}