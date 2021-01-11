using System;
using System.Runtime.InteropServices;

namespace Bramf.Encryptation
{
    /// <summary>
    /// Represents a password that is encrypted and deleted from memory when its no longer used 
    /// </summary>
    public class SecurePassword
    {
        #region Private Members

        /// <summary>
        /// The password stored as string
        /// </summary>
        private string mNormalPasswordString;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="normalPasswordString">The string containing the password. Its more secure if you enter the password right here 
        /// and not derive it from a variable.</param>
        public SecurePassword(string normalPasswordString)
        {
            mNormalPasswordString = normalPasswordString;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the password and deletes it
        /// </summary>
        public string GetPassword() => mNormalPasswordString;

        /// <summary>
        /// Cleans the memory
        /// </summary>
        public void Free()
        {
            // Frees pointers
            var passwordPointer = Marshal.StringToCoTaskMemUni(mNormalPasswordString);
            Marshal.ZeroFreeCoTaskMemUnicode(passwordPointer);
            Marshal.ZeroFreeGlobalAllocUnicode(passwordPointer);

            // Make null
            mNormalPasswordString = null;

            // Collect with garbage collector
            GC.Collect();
        }

        /// <summary>
        /// Generates a new secure password
        /// </summary>
        /// <param name="password">The string to use as password</param>
        public static SecurePassword Create(string password) => new SecurePassword(password);

        #endregion
    }
}
