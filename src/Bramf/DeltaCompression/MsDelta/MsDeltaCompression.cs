using System;
using System.ComponentModel;

namespace Bramf.DeltaCompression.MsDelta
{
    /// <summary>
    /// Used to create DELTA compressions based on Native methods
    /// </summary>
    internal class MsDeltaCompression
    {
        /// <summary>
        /// Creates a delta from an existing binary file
        /// </summary>
        /// <param name="sourceFilePath">The path where the source file is located</param>
        /// <param name="newFilePath">The path where the new file is located, to compare them</param>
        /// <param name="deltaFilePath">The path where the delta path will be located</param>
        public void CreateDelta(string sourceFilePath, string newFilePath, string deltaFilePath)
        {
            const string sourceOptionsName = null;
            const string targetOptionsName = null;
            var globalOptions = new DeltaInput();
            var targetFileTime = IntPtr.Zero;

            // Create delta
            if (!NativeMethods.CreateDelta(
                FileTypeSet.Executables, CreateFlags.IgnoreFileSizeLimit, CreateFlags.None, sourceFilePath, newFilePath,
                sourceOptionsName, targetOptionsName, globalOptions, targetFileTime, HashAlgId.Crc32, deltaFilePath))
            {
                // Throw exception if something went wrong
                throw new Win32Exception();
            }
        }

        /// <summary>
        /// Applies delta to a binary file
        /// </summary>
        /// <param name="deltaFilePath">The path where the delta binary is located</param>
        /// <param name="destionationFilePath">The path where the binary file that will receive the delta is located</param>
        /// <param name="newFilePath">The destination path where the file with the delta applied will be created</param>
        public void ApplyDelta(string deltaFilePath, string destionationFilePath, string newFilePath)
        {
            // Apply delta
            if (!NativeMethods.ApplyDelta(ApplyFlags.AllowLegacy, destionationFilePath, deltaFilePath, newFilePath))
                throw new Win32Exception();
        }
    }
}