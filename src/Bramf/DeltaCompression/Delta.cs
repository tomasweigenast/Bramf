using Bramf.DeltaCompression.CoreDelta;
using Bramf.DeltaCompression.MsDelta;
using System;
using System.ComponentModel;
using System.IO;

namespace Bramf.DeltaCompression
{
    /// <summary>
    /// Static class used to create delta files
    /// </summary>
    public static class Delta
    {
        /// <summary>
        /// Creates a delta file from a source file. Returns true if the delta buffer could be created.
        /// </summary>
        /// <param name="sourceFile">The path where the source file is located</param>
        /// <param name="newVersionFile">The path where the target file is located</param>
        /// <param name="outputDeltaFile">The path where the delta file is going to be saved</param>
        /// <param name="deltaCompressionType">The delta compression used to compare</param>
        public static bool Create(string sourceFile, string newVersionFile, string outputDeltaFile, DeltaCompressionType deltaCompressionType = DeltaCompressionType.Core)
        {
            // File checks
            if (!File.Exists(sourceFile)) throw new FileNotFoundException("File not found.", sourceFile);
            if (!File.Exists(newVersionFile)) throw new FileNotFoundException("File not found.", newVersionFile);
            Directory.CreateDirectory(Path.GetDirectoryName(outputDeltaFile)); // Create output directory if no exists

            // Use native methods
            if (deltaCompressionType == DeltaCompressionType.Native)
            {
                // Create output delta file if not found
                if (!File.Exists(outputDeltaFile)) using (var fs = File.Create(outputDeltaFile)) { }

                // Create msDelta
                var msDelta = new MsDeltaCompression();

                try
                {
                    // Create delta
                    msDelta.CreateDelta(sourceFile, newVersionFile, outputDeltaFile);
                    return true;
                }
                catch(Win32Exception)
                {
                    return false;
                }
            }

            // Use core methods
            else
            {
                // Get bytes
                byte[] sourceFileBytes = File.ReadAllBytes(sourceFile);
                byte[] newVersionFileBytes = File.ReadAllBytes(sourceFile);
                byte[] deltaBuffer = default; // Create array to store the delta

                // Create delta
                deltaBuffer = CoreDeltaCompression.Create(sourceFileBytes, newVersionFileBytes);

                // If the delta buffer is 0, return false
                if (deltaBuffer.Length <= 0)
                    return false;

                // Create stream to write
                using (var fs = new FileStream(outputDeltaFile, FileMode.OpenOrCreate, FileAccess.Write))
                using (var binaryWriter = new BinaryWriter(fs))
                    binaryWriter.Write(deltaBuffer); // Write the buffer

                // Return success
                return true;
            }
        }

        /// <summary>
        /// Applies a delta byte array to a file
        /// </summary>
        /// <param name="sourceFile">The path where the source file is located</param>
        /// <param name="deltaFile">The path where the delta file is located</param>
        /// <param name="finalFile">The path where the file with delta applied is going to be saved</param>
        /// <param name="deltaCompressionType">The delta compression used to compare</param>
        public static bool Apply(string sourceFile, string deltaFile, string finalFile, DeltaCompressionType deltaCompressionType = DeltaCompressionType.Core)
        {
            // File checks
            if (!File.Exists(sourceFile)) throw new FileNotFoundException("Source file not found.", sourceFile);
            if (!File.Exists(deltaFile)) throw new FileNotFoundException("Delta file not found.", deltaFile);
            Directory.CreateDirectory(Path.GetDirectoryName(finalFile)); // Create directory if not found

            // Use native methods
            if(deltaCompressionType == DeltaCompressionType.Native)
            {
                // Create final file if not found
                if (!File.Exists(finalFile)) using (var fs = File.Create(finalFile)) { }

                // Create ms delta instance
                var msDelta = new MsDeltaCompression();

                try
                {
                    // Apply delta
                    msDelta.ApplyDelta(deltaFile, sourceFile, finalFile);

                    return true;
                }
                catch(Win32Exception)
                {
                    return false;
                }

            }
            // Use core methods
            else
            {
                // Read files
                byte[] sourceFileBytes = File.ReadAllBytes(sourceFile);
                byte[] deltaBytes = File.ReadAllBytes(deltaFile);
                byte[] finalBytesBuffer = default; // Create buffer to store the final bytes

                // Apply delta
                finalBytesBuffer = CoreDeltaCompression.Apply(sourceFileBytes, deltaBytes);

                try
                {
                    // Create stream to write to the file
                    using (var fs = new FileStream(finalFile, FileMode.OpenOrCreate, FileAccess.Write))
                    using (var binaryWriter = new BinaryWriter(fs))
                        binaryWriter.Write(finalBytesBuffer); // Write the buffer
                }
                catch(Exception)
                {
                    return false;
                }

                return true;
            }
        }
    }
}
