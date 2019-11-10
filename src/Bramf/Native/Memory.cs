using System;
using System.Runtime.InteropServices;

namespace Bramf.Native
{
    /// <summary>
    /// Handles methods natively from the OS
    /// </summary>
    public static class Memory
    {
        /// <summary>
        /// Used to fill a block of memory with zeros, cleaning the memory
        /// </summary>
        /// <param name="pointerDestination">A pointer to the starting address of the block of memory to fill with zeros.</param>
        /// <param name="lenght">The size of the block of memory to fill with zeros, in bytes</param>
        [DllImport("kernel32.dll", EntryPoint = "RtlZeroMemory")]
        public static extern bool ZeroMemory(IntPtr pointerDestination, int lenght);
    }
}
