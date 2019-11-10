using Bramf.Extensions;
using System.IO;
using System.Reflection;

namespace Bramf.App
{
    /// <summary>
    /// Defines a base storage
    /// </summary>
    public interface IAppStorage
    {
        /// <summary>
        /// The installation directorty of the application
        /// </summary>
        AppDirectory InstallationDirectory { get; }

        /// <summary>
        /// The user files directory
        /// </summary>
        AppDirectory UserFiles { get; }

        /// <summary>
        /// The app files directory
        /// </summary>
        AppDirectory AppFiles { get; }

        /// <summary>
        /// Creates not found directories
        /// </summary>
        void CreateDirectories();
    }

    /// <summary>
    /// Represents the base class for an application storage
    /// </summary>
    public abstract class BaseAppStorage : IAppStorage
    {
        #region Properties

        /// <summary>
        /// The directory where the app was installed
        /// </summary>
        public AppDirectory InstallationDirectory => Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);

        /// <summary>
        /// The directory where the files of the user are stored
        /// </summary>
        public abstract AppDirectory UserFiles { get; }

        /// <summary>
        /// The directory where the files of the application are stored
        /// </summary>
        public abstract AppDirectory AppFiles { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new app storage
        /// </summary>
        public BaseAppStorage() { }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the directories if not found
        /// </summary>
        public virtual void CreateDirectories()
        {
            if(!UserFiles.Path.IsNullOrWhitespace()) Directory.CreateDirectory(UserFiles.Path);
            if(!AppFiles.Path.IsNullOrWhitespace()) Directory.CreateDirectory(AppFiles.Path);
        }

        #endregion
    }
}
