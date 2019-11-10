using System;
using System.Collections.Generic;

namespace Bramf.Helpers
{
    /// <summary>
    /// Expose methods to help with byte arrays
    /// </summary>
    public static class ByteArrayHelpers
    {
        /// <summary>
        /// Fills a byte array with a buffer of data
        /// </summary>
        /// <param name="source">The byte array to add the buffer to.</param>
        /// <param name="buffer">The buffer to add to the byte array</param>
        public static void Fill(this byte[] source, byte[] buffer)
        {
            // Avoid bugs
            if (buffer.Length <= 0) throw new InvalidOperationException("The buffer cannot be empty.");
            if (buffer.Length > source.Length) throw new ArgumentOutOfRangeException($"The buffer to add to the source byte array is greater.");

            // Copy data
            for (int i = 0; i < buffer.Length; i++)
                try
                {
                    source[i] = buffer[i];
                }
                catch
                {
                    throw new OverflowException("Buffer is greater than the source byte array.");
                }
        }

        /// <summary>
        /// Fills a byte array with buffers of data
        /// </summary>
        /// <param name="source">The byte array to add the buffer to.</param>
        /// <param name="buffers">An ienumerable collection of byte arrays</param>
        public static void Fill(this byte[] source, IEnumerable<byte[]> buffers)
        {
            // Avoid bugs
            if (buffers == null) throw new ArgumentNullException(nameof(buffers));

            // Foreach byte array
            int currentOffset = 0;
            foreach(byte[] byteArray in buffers)
            {
                // Copy the data
                try
                {
                    Array.Copy(byteArray, 0, source, currentOffset, byteArray.Length);
                    currentOffset = byteArray.Length;
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}