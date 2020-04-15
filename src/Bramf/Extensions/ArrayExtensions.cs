using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Bramf.Extensions
{
    /// <summary>
    /// Extension methods for arrays
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Append the given objects to the original source array
        /// </summary>
        /// <typeparam name="T">The type of array</typeparam>
        /// <param name="source">The original array of values</param>
        /// <param name="toAdd">The values to append to the source</param>
        /// <returns></returns>
        public static T[] Append<T>(this T[] source, params T[] toAdd)
            => source.Concat(toAdd).ToArray();

        /// <summary>
        /// Prepend the given objects to the original source array
        /// </summary>
        /// <typeparam name="T">The type of array</typeparam>
        /// <param name="source">The original array of values</param>
        /// <param name="toAdd">The values to prepend to the source</param>
        /// <returns></returns>
        public static T[] Prepend<T>(this T[] source, params T[] toAdd)
            => toAdd.Append(source);

        /// <summary>
        /// Combines two byte arrays
        /// </summary>
        /// <param name="left">The first byte[]</param>
        /// <param name="right">The second byte[]</param>
        public static byte[] CombineWith(this byte[] left, byte[] right)
        {
            byte[] output = new byte[left.Length + right.Length];
            Buffer.BlockCopy(left, 0, output, 0, left.Length);
            Buffer.BlockCopy(right, 0, output, left.Length, right.Length);

            return output;
        }

        /// <summary>
        /// Checks if two byte[] are equals or not
        /// </summary>
        [MethodImpl(MethodImplOptions.NoOptimization)]
        public static bool IsEqualsTo(this byte[] left, byte[] right)
        {
            if (ReferenceEquals(left, right))
                return true;

            if (left == null || right == null || left.Length != right.Length)
                return false;

            bool areSame = true;
            for (int i = 0; i < left.Length; i++)
                areSame &= (left[i] == right[i]);

            return areSame;
        }
    }
}
