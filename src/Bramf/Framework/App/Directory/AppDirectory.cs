using Bramf.Extensions;
using System;
using System.IO;

namespace Bramf.App
{
    /// <summary>
    /// Represents a directory
    /// </summary>
    public class AppDirectory
    {
        #region Properties

        /// <summary>
        /// The path to the directory
        /// </summary>
        public string Path { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new directory
        /// </summary>
        /// <param name="path">The path to the directory</param>
        public AppDirectory(string path)
        {
            Path = path;

            if(Path.IsNullOrWhitespace())
                Path = null;
            else
                // Create the directory if no exist
                Directory.CreateDirectory(path);

        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a folder relative to the <see cref="Path"/>, if no exists, gets the path and return it
        /// </summary>
        /// <param name="folderName">The name of the folder</param>
        public string GetFolder(string folderName)
        {
            // Replace / for \
            folderName = folderName.Replace('/', '\\');

            FileInfo fi = new FileInfo(folderName);

            // If folderName is a file
            if (!fi.Attributes.HasFlag(FileAttributes.Directory))
                throw new ArgumentException("You cannot specify a file name.");

            // If folderName is a disk
            if(folderName[1] == ':')
                throw new ArgumentException("You cannot specify a disk name.");

            // Create the directory
            var dirInfo = Directory.CreateDirectory(System.IO.Path.Combine(Path, folderName));

            // Return the full path
            return dirInfo.FullName;
        }

        /// <summary>
        /// Gets the full path to a file. If it does not exist, a new file is created.
        /// </summary>
        /// <param name="fileName">The name of the file. Can contain a route: /user/files/file.txt</param>
        public string GetFile(string fileName)
        {
            // Replace paths if contains /
            if (fileName.Contains("/"))
                fileName = fileName.Replace('/', '\\');

            // Get path
            string filePath = System.IO.Path.Combine(Path, fileName);

            // Create directory if not exists
            string dirName = System.IO.Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dirName)) Directory.CreateDirectory(dirName);

            // If the file exists, return its full path
            if (File.Exists(filePath)) return filePath;

            // Create the file
            using (var fs = File.Create(filePath)) { }

            // Return the path
            return filePath;
        }

        /// <summary>
        /// Gets a <see cref="FileStream"/> to a file. If it does not exist, a new file is created.
        /// </summary>
        /// <param name="fileName">The name of the file. Can contain a route: /user/files/file.txt</param>
        /// <param name="fileAccess">The accessibility to the file</param>
        public FileStream GetFileStream(string fileName, FileAccess fileAccess = FileAccess.ReadWrite)
        {
            // Replace paths if contains /
            if (fileName.Contains("/"))
                fileName = fileName.Replace('/', '\\');

            // Get path
            string filePath = System.IO.Path.Combine(Path, fileName);

            // If the file exists, return its full path
            if (File.Exists(filePath)) new FileStream(filePath, FileMode.Open, fileAccess);

            // Create a new file and return the stream
            return new FileStream(filePath, FileMode.CreateNew, fileAccess);
        }

        /// <summary>
        /// Deletes a file
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        public bool DeleteFile(string filePath)
        {
            // Replace / for \
            filePath = filePath.Replace('/', '\\');

            try
            {
                // Delete the file
                File.Delete(filePath);
                return true;
            }
            catch
            {
                // Return false if something happens
                return false;
            }
        }

        /// <summary>
        /// Deletes a folder
        /// </summary>
        /// <param name="folderPath">The path to the folder</param>
        public bool DeleteFolder(string folderPath)
        {
            // Replace / for \
            folderPath = folderPath.Replace('/', '\\');

            FileInfo fi = new FileInfo(folderPath);

            // If folderName is a file
            if (!fi.Attributes.HasFlag(FileAttributes.Directory))
                throw new ArgumentException("You cannot specify a file name.");

            // If folderName is a disk
            if (folderPath[1] == ':')
                throw new ArgumentException("You cannot specify a disk name.");

            // Create the directory
            try
            {
                Directory.Delete(folderPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns directory path
        /// </summary>
        public override string ToString() => Path;

        #endregion

        #region Operators

        /// <summary>
        /// Implicit string operator
        /// </summary>
        public static implicit operator AppDirectory(string path) => new AppDirectory(path);

        /// <summary>
        /// Explicit appdir operator
        /// </summary>
        public static implicit operator string(AppDirectory appDir) => appDir.Path;

        #endregion
    }
}
