using System;

namespace Bramf.DeltaCompression.MsDelta
{
    /// <remarks>
    ///     http://msdn.microsoft.com/en-us/library/bb417345.aspx#deltaflagtypeflags
    /// </remarks>
    internal enum ApplyFlags : long
    {
        /// <summary>Indicates no special handling.</summary>
        None = 0,

        /// <summary>Allow MSDelta to apply deltas created using PatchAPI.</summary>
        AllowLegacy = 1
    }

    /// <remarks>
    ///     http://msdn.microsoft.com/en-us/library/bb417345.aspx#deltaflagtypeflags
    /// </remarks>
    internal enum CreateFlags : long
    {
        /// <summary>Indicates no special handling.</summary>
        None = 0,

        /// <summary>Allow the source, target and delta files to exceed the default size limit.</summary>
        IgnoreFileSizeLimit = 1 << 17
    }

    /// <remarks>
    ///     http://msdn.microsoft.com/en-us/library/bb417345.aspx#filetypesets
    /// </remarks>
    [Flags]
    internal enum FileTypeSet : long
    {
        /// <summary>
        ///     File type set that includes I386, IA64 and AMD64 Portable Executable (PE) files. Others are treated as raw.
        /// </summary>
        Executables = 0x0FL
    }

    internal enum HashAlgId
    {
        /// <summary>No signature.</summary>
        None = 0,

        /// <summary>32-bit CRC defined in msdelta.dll.</summary>
        Crc32 = 32
    }
}