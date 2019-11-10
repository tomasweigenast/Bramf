namespace Bramf.DeltaCompression
{
    /// <summary>
    /// The types of delta compressions that can be made
    /// </summary>
    public enum DeltaCompressionType
    {
        /// <summary>
        /// Uses the native delta compression. Special for large file sizes
        /// </summary>
        Native,

        /// <summary>
        /// Uses a re-made delta compression. Special for smaller files
        /// </summary>
        Core
    }
}
