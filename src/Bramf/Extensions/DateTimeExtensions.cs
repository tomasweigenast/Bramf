using System;
using System.Globalization;

namespace Bramf.Extensions
{
    /// <summary>
    /// Provides methods to help with <see cref="DateTime"/>
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Takes a <see cref="DateTime"/> and parses it returning its timestamp
        /// </summary>
        public static string ToTimestamp(this DateTime dt)
            => dt.ToString("yyyyMMddHHmmssfff");

        /// <summary>
        /// Takes a format <see cref="string"/> and parses it to a <see cref="DateTime"/>
        /// </summary>
        public static DateTime FromTimestamp(this string str)
        {
            bool result = DateTime.TryParseExact(str, "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt);

            if (!result) throw new InvalidOperationException("The input string is not a valid timestamp format.");

            return dt;
        }
    }
}
