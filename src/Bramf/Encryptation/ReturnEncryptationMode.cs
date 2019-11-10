namespace Bramf.Encryptation
{
    /// <summary>
    /// The mode to return and get the data encrypted
    /// </summary>
    public enum EncryptationMode
    {
        /// <summary>
        /// The encrypted data is encoded using Base64 
        /// </summary>
        Base64,

        /// <summary>
        /// The encrypted data is encoded using unicode
        /// </summary>
        Unicode,

        /// <summary>
        /// Represents the encrypted bytes
        /// </summary>
        Bytes
    }
}
