using Bramf.Extensions;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.ComponentModel;
using System.IO;

namespace Bramf.Zip
{
    /// <summary>
    /// Contains methods to zip and unzip files and folders
    /// </summary>
    public static class ZipManager
    {
        /// <summary>
        /// Compress all the files of a folder
        /// </summary>
        /// <param name="inputPath">The directory path containing all files to be compressed</param>
        /// <param name="outputPath">The output file</param>
        /// <param name="compressLevel">The compression level. 9 is the highest value.</param>
        /// <param name="password">The password of the zip. Leaving it as null, means that will not be applied.</param>
        public static void ZipFolder(string inputPath, string outputPath, string password = "", int compressLevel = 3)
        {
            BackgroundWorker bWorker = new BackgroundWorker();
            bWorker.DoWork += (s, e) =>
            {
                int progress = 0;

                // If the compress level is greater than 9, or less than 0
                if (compressLevel > 9 || compressLevel < 0)
                    compressLevel = 3; // Set it to 3

                // If the input path is not a directory, throw exception
                if (File.GetAttributes(inputPath) != FileAttributes.Directory)
                    throw new Exception($"'{inputPath}' is not a directory.");

                // If input path no exists, throw exception
                if (!Directory.Exists(inputPath))
                    throw new DirectoryNotFoundException($"Directory {inputPath} not found.");

                // Set folder offset
                DirectoryInfo dirInfo = new DirectoryInfo(inputPath);
                int folderOffset = dirInfo.Name.Length + (dirInfo.Name.EndsWith("\\") ? 0 : 1);
                folderOffset = 0;

                // Report progress
                bWorker.ReportProgress(progress++);

                // Create directory info
                var files = Directory.GetFiles(inputPath);

                // Report progress
                bWorker.ReportProgress(progress++);

                // Create stream for the output file...
                using (var fileOutputStream = File.Open(outputPath, FileMode.OpenOrCreate))
                // Create zip output stream to compress
                using (var zipStream = new ZipOutputStream(fileOutputStream))
                {
                    // Configure zip stream
                    zipStream.SetLevel(compressLevel);
                    zipStream.Password = password;

                    // Foreach file in directory...
                    foreach (var fileName in files)
                    {
                        FileInfo fi = new FileInfo(fileName);

                        // Makes the name in zip based on the folder
                        string entryName = fi.Name.Substring(folderOffset);
                        entryName = ZipEntry.CleanName(entryName); // Clears the name

                        // Create new zip entry
                        ZipEntry newEntry = new ZipEntry(entryName)
                        {
                            DateTime = fi.LastWriteTime,
                            Size = fi.Length
                        };

                        // Add the entry to the stream
                        zipStream.PutNextEntry(newEntry);

                        // Zip the file in buffered chunks
                        // the "using" will close the stream even if an exception occurs
                        byte[] buffer = new byte[4096];
                        using (var streamReader = File.OpenRead(fileName))
                            StreamUtils.Copy(streamReader, zipStream, buffer);

                        // Close the entry
                        zipStream.CloseEntry();

                        // Report progress
                        bWorker.ReportProgress(progress++);
                    }
                }

                // Foreach directory inside path, zip them
                foreach (var folder in Directory.GetDirectories(inputPath))
                    ZipFolder(folder, outputPath);
            };
        }

        /// <summary>
        /// Decompress all the entries from a zip file
        /// </summary>
        /// <param name="zipFilePath">The path to the zip file to decompress</param>
        /// <param name="outputFolder">The output folder where the decompressed files will go</param>
        /// <param name="password">The password, if required, to decompress the zip file</param>
        public static void UnzipFolder(string zipFilePath, string outputFolder, string password = "")
        {
            // If the zip file does not exists, throw exception
            if (!File.Exists(zipFilePath))
                throw new FileNotFoundException("File not found.", zipFilePath);

            // If output folder does not exists, create it
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            // Create variable to store the zip file
            ZipFile zipFile = default(ZipFile);

            try
            {
                // Open read stream of the zip file
                using (FileStream fs = File.OpenRead(zipFilePath))
                {
                    // Create a zip file
                    zipFile = new ZipFile(fs);

                    // If there is a password, set to the zip file
                    if (!password.IsNullOrWhitespace())
                        zipFile.Password = password;

                    // Foreach entry in the zip file...
                    foreach(ZipEntry zipEntry in zipFile)
                    {
                        // If the entry is not a file, continue with the loop
                        if (!zipEntry.IsFile)
                            continue;

                        // Get entry name
                        string entryFileName = zipEntry.Name;

                        // Create a buffer of 4K to read the content of the entry
                        byte[] buffer = new byte[4096];

                        // Create a new stream
                        using (Stream zipStream = zipFile.GetInputStream(zipEntry))
                        {
                            // Manipulate output filename
                            string fullZipToPath = Path.Combine(outputFolder, entryFileName);
                            string directoryName = Path.GetDirectoryName(fullZipToPath);

                            // If the length of the directory name is greater than 0, create it
                            if (directoryName.Length > 0)
                                Directory.CreateDirectory(directoryName);

                            // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                            // of the file, but does not waste memory.
                            // The "using" will close the stream even if an exception occurs.
                            using (FileStream streamWriter = File.Create(fullZipToPath))
                                StreamUtils.Copy(zipStream, streamWriter, buffer);
                        }
                    }
                }
            }
            finally
            {
                // Close the zip file
                if(zipFile != null)
                {
                    zipFile.IsStreamOwner = true;
                    zipFile.Close();
                }
            }
        }
    }
}
