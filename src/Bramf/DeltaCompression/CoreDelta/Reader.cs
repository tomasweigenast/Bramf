using System;

namespace Bramf.DeltaCompression.CoreDelta
{
    /// <summary>
    /// Class used to read the delta values
    /// </summary>
    internal class Reader
    {
        #region Private Members

        static readonly int[] zValue = {
            -1, -1, -1, -1, -1, -1, -1, -1,   -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1,   -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1,   -1, -1, -1, -1, -1, -1, -1, -1,
            0,  1,  2,  3,  4,  5,  6,  7,    8,  9, -1, -1, -1, -1, -1, -1,
            -1, 10, 11, 12, 13, 14, 15, 16,   17, 18, 19, 20, 21, 22, 23, 24,
            25, 26, 27, 28, 29, 30, 31, 32,   33, 34, 35, -1, -1, -1, -1, 36,
            -1, 37, 38, 39, 40, 41, 42, 43,   44, 45, 46, 47, 48, 49, 50, 51,
            52, 53, 54, 55, 56, 57, 58, 59,   60, 61, 62, -1, -1, -1, 63, -1
        };

        #endregion

        #region Public Properties

        /// <summary>
        /// The byte array containing the delta
        /// </summary>
        public byte[] ByteArray { get; set; }

        /// <summary>
        /// The current position of the reader
        /// </summary>
        public uint CurrentPosition { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="array">Byte array to read</param>
        public Reader(byte[] array)
        {
            ByteArray = array;
            CurrentPosition = 0;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Indicates if there are bytes to read
        /// </summary>
        public bool HaveBytes() => CurrentPosition < ByteArray.Length;

        /// <summary>
        /// Returns a single byte from the current position
        /// </summary>
        public byte GetByte()
        {
            byte b = ByteArray[CurrentPosition]; // Get the byte at the current position
            CurrentPosition++; // Increment one position

            // Out of bounds
            if (CurrentPosition > ByteArray.Length)
                throw new IndexOutOfRangeException("Out of bounds");

            // Return the byte
            return b;
        }

        /// <summary>
        /// Returns a single char from the current position
        /// </summary>
        public char GetChar() => (char)GetByte();

        /// <summary>
        /// Read bytes from *pz and convert them into a positive integer. When
		/// finished, leave *pz pointing to the first character past the end of
        /// the integer.The *pLen parameter holds the length of the string
        /// in *pz and is decremented once for each character in the integer.
        /// </summary>
        public uint GetInt()
        {
            uint v = 0;
            int c;
            while (HaveBytes() && (c = zValue[0x7f & GetByte()]) >= 0)
                v = (uint)((((Int32)v) << 6) + c);

            CurrentPosition--;
            return v;
        }

        #endregion
    }
}
