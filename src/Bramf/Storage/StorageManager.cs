using System;

namespace Bramf.Storage
{
    /// <summary>
    /// Manager that is used by the storage system to manage files
    /// </summary>
    public class StorageManager
    {
        #region Private Members

        /// <summary>
        /// Indicates if the manager is ready to use
        /// </summary>
        private static bool mReady;

        #endregion

        

        #region Methods

        /// <summary>
        /// Setups a new version of this manager
        /// </summary>
        public static StorageManager CreateInstance()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Builds the storage
        /// </summary>
        public static void Build()
        {

        }

        #endregion
    }
}
