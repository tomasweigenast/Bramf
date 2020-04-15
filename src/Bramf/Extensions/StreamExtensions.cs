using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bramf.Extensions
{
    /// <summary>
    /// Extension methods to work with <see cref="Stream"/>
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Reads all lines from a <see cref="Stream"/>
        /// </summary>
        /// <param name="stream">The stream that contains the lines to read</param>
        public static IEnumerable<string> ReadAllLines(this Stream stream)
        {
            using var reader = new StreamReader(stream);
            string line;
            while ((line = reader.ReadLine()) != null)
                yield return line;
        }

        /// <summary>
        /// Read all lines from a <see cref="Stream"/> asynchronously
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        public static async IAsyncEnumerable<string> ReadAllLinesAsync(this Stream stream)
        {
            using var reader = new StreamReader(stream);
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
                yield return line;
        }

        /// <summary>
        /// Creates a <see cref="BinaryReader"/> from a <see cref="Stream"/>
        /// </summary>
        public static BinaryReader CreateReader(this Stream stream)
            => new BinaryReader(stream, Encoding.UTF8, true);

        /// <summary>
        /// Creates a <see cref="BinaryWriter"/> from a <see cref="Stream"/>
        /// </summary>
        public static BinaryWriter CreateWriter(this Stream stream)
            => new BinaryWriter(stream, Encoding.UTF8, true);

        /// <summary>
        /// Reads a <see cref="DateTimeOffset"/> from a buffer
        /// </summary>
        public static DateTimeOffset ReadDateTimeOffset(this BinaryReader reader)
            => new DateTimeOffset(reader.ReadInt64(), TimeSpan.Zero);

        /// <summary>
        /// Writes a <see cref="DateTimeOffset"/> as <see cref="Int64"/> to a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The <see cref="DateTimeOffset"/> value.</param>
        public static void Write(this BinaryWriter writer, DateTimeOffset value)
            => writer.Write(value.UtcTicks);
    }
}