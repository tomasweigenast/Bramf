using Bramf.Serializer;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Bramf.Structure
{
    /// <summary>
    /// Helps with architectures
    /// </summary>
    public static class ArchitectureManager
    {
        // The file name of the library data
        private const string FileName = "app.part";

        /// <summary>
        /// Gets the startup class of an assembly
        /// </summary>
        /// <param name="assembly">The assembly to look for startups</param>
        internal static (Type, ArchitecturePart) GetArchitecturePart(Assembly assembly)
        {
            // Get all types with architecture part attribute implemented
            var types = assembly.GetTypes().Where(x => x.GetCustomAttribute(typeof(ArchitecturePartAttribute), false) != null).ToArray();

            // Avoid bugs
            if (types.Length > 1) throw new InvalidOperationException("There are two or more startup classes.");
            else if (types.Length <= 0) return (null, null); // Return null if there is no

            // Get startup attribute
            var startupAttribute = types[0].GetCustomAttribute<ArchitecturePartAttribute>(false);

            // Return null if not found
            if (startupAttribute == null) return (null, null);

            return (types[0], new ArchitecturePart(assembly, startupAttribute.Name, startupAttribute.Version));
        }

        /// <summary>
        /// Gets the startup class
        /// </summary>
        internal static ArchitecturePart GetArchitecturePartFromFile(Assembly assembly)
        {
            // Try to get file info
            var file = assembly.GetManifestResourceNames().FirstOrDefault(x => x.EndsWith(FileName));

            // If null, return null
            if (file == null) return null;

            // Read file xml
            string xmlContent = "";
            using(var stream = assembly.GetManifestResourceStream(file))
            using(var reader = new StreamReader(stream))
                xmlContent = reader.ReadToEnd();

            // Parse values
            var keys = xmlContent.Replace("\r", "").Split('\n');

            // Avoid invalid files
            if (keys.Length <= 0 || keys.Length > 3) throw new InvalidOperationException("Invalid app.part file.");
            if(keys[0] != "!libpart") throw new InvalidOperationException("Invalid app.part file.");

            try
            {
                // Get values
                string name = keys[1].Remove(0, 6);
                string version = keys[2].Remove(0, 9);

                // Create part and return
                return new ArchitecturePart(assembly, name, version);
            }
            catch
            {
                throw new InvalidOperationException("Invalid app.part file.");
            }

            return null;
        }
    }
}
