using System;

namespace Bramf.App
{
    /// <summary>
    /// Defines the behaviour of an application
    /// </summary>
    public interface IAppBehaviour
    {
        /// <summary>
        /// The version of the application
        /// </summary>
        SemanticVersion AppVersion { get; }

        /// <summary>
        /// The application runtime
        /// </summary>
        Runtime Runtime { get; set; }

        /// <summary>
        /// Copy needed files
        /// </summary>
        void CopyNeededFiles();
    }

    /// <summary>
    /// Defines the behaviour of an application and implements a storage
    /// </summary>
    public interface IAppBehaviour<out TStorage> : IAppBehaviour
        where TStorage : IAppStorage
    {
        /// <summary>
        /// The storage
        /// </summary>
        TStorage Storage { get; }
    }

    /// <summary>
    /// Defines a base app behaviour without storage
    /// </summary>
    public abstract class BaseAppBehaviour : IAppBehaviour
    {
        /// <summary>
        /// The version of the application
        /// </summary>
        public abstract SemanticVersion AppVersion { get; }

        /// <summary>
        /// The runtime of the application
        /// </summary>
        public Runtime Runtime { get; set; }

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public BaseAppBehaviour()
        {
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="runtime">The runtime instance</param>
        public BaseAppBehaviour(Runtime runtime)
        {
            Runtime = runtime;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Copies all required files at application startup
        /// </summary>
        public virtual void CopyNeededFiles() { }

        #endregion
    }

    /// <summary>
    /// A base application behaviour that implements the <see cref="IAppBehaviour"/>
    /// </summary>
    /// <typeparam name="TStorage">The type of storage to implement</typeparam>
    public abstract class BaseAppBehaviour<TStorage> : IAppBehaviour<TStorage>
        where TStorage : IAppStorage
    {
        #region Properties

        /// <summary>
        /// The version of the application
        /// </summary>
        public abstract SemanticVersion AppVersion { get; }

        /// <summary>
        /// The runtime of the application
        /// </summary>
        public Runtime Runtime { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public BaseAppBehaviour()
        {
            Storage = Activator.CreateInstance<TStorage>();
            Storage.CreateDirectories();
            //Runtime = runtime; TODO: Get runtime
        }

        #endregion

        #region Methods

        /// <summary>
        /// Copy needed files
        /// </summary>
        public virtual void CopyNeededFiles() { }

        #endregion

        #region Properties

        /// <summary>
        /// The access to the <see cref="BaseAppStorage"/>
        /// </summary>
        public TStorage Storage { get; }

        #endregion
    }
}
