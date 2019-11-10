using System.Collections.Generic;

namespace Bramf.DeltaCompression.CoreDelta
{
    /// <summary>
    /// Class used to write bytes to the delta
    /// </summary>
    internal class Writer
    {
        #region Private Members

        private List<byte> byteArray; // The array that contains the difference bytes
        static readonly uint[] zDigits = {  // The available chars
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D',
            'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R',
            'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '_', 'a', 'b', 'c', 'd', 'e',
            'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's',
            't', 'u', 'v', 'w', 'x', 'y', 'z', '~'
        };

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Writer()
        {
            // Init the list
            this.byteArray = new List<byte>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Puts a char to the array
        /// </summary>
        /// <param name="c"></param>
        public void PutChar(char c) => byteArray.Add((byte)c);

        /// <summary>
        /// Puts an <see cref="uint"/> to the array
        /// </summary>
        /// <param name="value">The <see cref="uint"/> value</param>
        public void PutInt(uint value)
        {
            int i, j;
            uint[] zBuf = new uint[20];

            if (value == 0)
            {
                PutChar('0');
                return;
            }

            for (i = 0; value > 0; i++, value >>= 6)
                zBuf[i] = zDigits[value & 0x3f];

            for (j = i - 1; j >= 0; j--)
                byteArray.Add((byte)zBuf[j]);
        }

        /// <summary>
        /// Puts an array in the <see cref="byteArray"/>
        /// </summary>
        /// <param name="a">The byte array</param>
        /// <param name="start">The start index</param>
        /// <param name="end">The end index</param>
        public void PutArray(byte[] a, int start, int end)
        {
            for (var i = start; i < end; i++)
                byteArray.Add(a[i]);
        }

        /// <summary>
        /// Returns the list as an byte array
        /// </summary>
        public byte[] ToArray()
            => byteArray.ToArray();

        #endregion
    }
}
