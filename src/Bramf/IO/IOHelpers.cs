using Bramf.Extensions;
using System;
using System.IO;
using System.Linq;

namespace Bramf.IO
{
    /// <summary>
    /// Provides methods to help with paths, files and directories
    /// </summary>
    public static class IOHelpers
    {
        /// <summary>
        /// Returns true if a path is a file or false if its a directory.
        /// </summary>
        /// <param name="path">The path to the file or directory to check</param>
        public static bool IsFile(string path)
        {
            if (path.IsNullOrWhitespace())
                throw new ArgumentNullException(nameof(path));

            path = path.Trim();

            if (File.Exists(path)) return true;
            else if (Directory.Exists(path)) return false;
            else
            {
                if (new[] { "\\", "/" }.Any(x => path.EndsWith(x)))
                    return false;

                return !Path.GetExtension(path).IsNullOrWhitespace();
            }
        }
    }
}
