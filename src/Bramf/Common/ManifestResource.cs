using Bramf.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Bramf.Common
{
    /// <summary>
    /// Provides methods to work with embbeded resources
    /// </summary>
    public static class ManifestResource
    {
        /// <summary>
        /// Returns true if a resource exists
        /// </summary>
        /// <param name="name">The name of the resource</param>
        public static bool ExistsResource(string name) => Assembly.GetEntryAssembly().GetManifestResourceNames().Any(x => x.EndsWith(name));

        /// <summary>
        /// Gets a resource
        /// </summary>
        /// <param name="name">The resource name to get</param>
        public static Stream GetResource(string name)
        {
            // Try to get resource
            var assembly = Assembly.GetEntryAssembly();
            var resourceName = assembly.GetManifestResourceNames().SingleOrDefault(x => x.EndsWith(name));

            // Resource not found
            if (resourceName.IsNullOrWhitespace()) throw new InvalidOperationException("Resource not found.");

            return assembly.GetManifestResourceStream(resourceName);
        }

        /// <summary>
        /// Gets a list of embbedded resources under a specific parent folder
        /// </summary>
        /// <param name="parent">The parent folder path.</param>
        public static IEnumerable<Stream> GetResources(string parent)
        {
            // Get all the resources name
            var assembly = Assembly.GetEntryAssembly();
            string[] resources = assembly.GetManifestResourceNames().Where(x => x.Contains(parent)).ToArray();
            var list = new List<Stream>();

            foreach (var resource in resources)
                list.Add(assembly.GetManifestResourceStream(resource));

            return list;
        }

        /// <summary>
        /// Copies a resource to a disk location
        /// </summary>
        /// <param name="resource">The resource to copy</param>
        /// <param name="path">The physical path to copy the file to</param>
        /// <param name="copyAlways">Indicates if the resource will be copied always, whatever the file exists or not</param>
        public static bool CopyResource(string resource, string path, bool copyAlways = true)
        {
            // Ignore if the file already exists
            if(!copyAlways)
                if (File.Exists(path))
                    return false;

            // Copy the resource
            using (var resourceStream = GetResource(resource))
            using (var fs = new FileStream(path, FileMode.Create))
                resourceStream.CopyTo(fs);

            return true;
        }

        /// <summary>
        /// Copies a resource to a disk location
        /// </summary>
        /// <param name="resource">The resource stream to copy</param>
        /// <param name="path">The physical path to copy the file to</param>
        /// <param name="copyAlways">Indicates if the resource will be copied always, whatever the file exists or not</param>
        public static bool CopyResource(Stream resource, string path, bool copyAlways = true)
        {
            // Ignore if the file already exists
            if (!copyAlways)
                if (File.Exists(path))
                    return false;

            // Copy the resource
            using (var fs = new FileStream(path, FileMode.Create))
                resource.CopyTo(fs);

            return true;
        }

        /// <summary>
        /// Returns the length of a resource
        /// </summary>
        /// <param name="resource">The resource</param>
        public static long GetResourceLength(string resource)
        {
            long length;
            using (var resourceStream = GetResource(resource))
                length = resourceStream.Length;

            return length;
        }

        /// <summary>
        /// Reads a resource and returns its content
        /// </summary>
        /// <param name="resource">The resource to read</param>
        public static byte[] ReadResource(string resource)
        {
            byte[] result = null;

            using (var resourceStream = GetResource(resource))
            using (var ms = new MemoryStream())
            {
                byte[] buffer = new byte[2048];
                int bytesRead;

                // Write
                while ((bytesRead = resourceStream.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, bytesRead);

                // Set result
                result = ms.ToArray();
            }

            return result;
        }
    }
}
