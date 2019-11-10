namespace Bramf.DeltaCompression.CoreDelta
{
    /// <summary>
    /// Computes hashes
    /// </summary>
    internal class RollingHash
    {
        #region Private Members

        private ushort a;
        private ushort b;
        private ushort i;
        private byte[] z;
        static int ii = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public RollingHash()
        {
            a = 0;
            b = 0;
            i = 0;
            z = new byte[CoreDeltaCompression.NHASH];
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize the rolling hash using the first NHASH characters of z[]
        /// </summary>
        /// <param name="z">The origin array</param>
        /// <param name="pos">The reader position</param>
        public void Init(byte[] z, int pos)
        {
            ushort a = 0, b = 0, i, x;
            for (i = 0; i < CoreDeltaCompression.NHASH; i++)
            {
                x = z[pos + i];
                a = (ushort)((a + x) & 0xffff);
                b = (ushort)((b + (CoreDeltaCompression.NHASH - i) * x) & 0xffff);
                this.z[i] = (byte)x;
            }
            this.a = (ushort)(a & 0xffff);
            this.b = (ushort)(b & 0xffff);
            this.i = 0;
        }

        /// <summary>
        /// Advance the rolling hash by a single character "c"
        /// </summary>
        /// <param name="c">The byte to advance</param>
        public void Next(byte c)
        {
            ushort old = z[i];
            z[i] = c;
            i = (ushort)((i + 1) & (CoreDeltaCompression.NHASH - 1));
            a = (ushort)(a - old + c);
            b = (ushort)(b - CoreDeltaCompression.NHASH * old + a);
        }

        /// <summary>
        /// Return a 32-bit hash value
        /// </summary>
        public uint Value()
        {
            ii++;
            return ((uint)(a & 0xffff)) | (((uint)(b & 0xffff)) << 16);
        }

        /// <summary>
        /// Compute a hash on NHASH bytes.
        /// </summary>
        /// <remarks>
        /// This routine is intended to be equivalent to:
        /// <code>
        /// hash h;
        /// hash_init(&h, zInput);
        /// return hash_32bit(&h);
        /// </code>
        /// </remarks>
        /// <param name="z">The origin byte array</param>
        public static uint Once(byte[] z)
        {
            ushort a, b, i;
            a = b = z[0];
            for (i = 1; i < CoreDeltaCompression.NHASH; i++)
            {
                a += z[i];
                b += a;
            }
            return a | (((uint)b) << 16);
        }

        #endregion
    }
}
