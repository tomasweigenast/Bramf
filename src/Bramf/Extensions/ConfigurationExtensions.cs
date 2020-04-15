using Microsoft.Extensions.Configuration;
using System;

namespace Bramf.Extensions
{
    /// <summary>
    /// Extension methods to work with Microsoft <see cref="IConfiguration"/>
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Gets a boolean from the configuration
        /// </summary>
        /// <param name="configuration">The configuration source.</param>
        /// <param name="key">The key.</param>
        public static bool GetBool(this IConfiguration configuration, string key)
            => Get<bool>(configuration, key);

        /// <summary>
        /// Gets a double from the configuration
        /// </summary>
        /// <param name="configuration">The configuration source.</param>
        /// <param name="key">The key.</param>
        public static double GetDouble(this IConfiguration configuration, string key)
            => Get<double>(configuration, key);

        /// <summary>
        /// Gets a DateTime from the configuration
        /// </summary>
        /// <param name="configuration">The configuration source.</param>
        /// <param name="key">The key.</param>
        public static DateTime GetDateTime(this IConfiguration configuration, string key)
        {
            string value = configuration[key];
            if (!DateTime.TryParse(value, out DateTime result))
                throw new FormatException("Invalid DateTime format.");

            return result;
        }

        /// <summary>
        /// Gets a float from the configuration
        /// </summary>
        /// <param name="configuration">The configuration source.</param>
        /// <param name="key">The key.</param>
        public static float GetFloat(this IConfiguration configuration, string key)
            => Get<float>(configuration, key);

        /// <summary>
        /// Gets a long from the configuration
        /// </summary>
        /// <param name="configuration">The configuration source.</param>
        /// <param name="key">The key.</param>
        public static long GetLong(this IConfiguration configuration, string key)
            => Get<long>(configuration, key);

        /// <summary>
        /// Gets an int from the configuration
        /// </summary>
        /// <param name="configuration">The configuration source.</param>
        /// <param name="key">The key.</param>
        public static int GetNumber(this IConfiguration configuration, string key)
            => Get<int>(configuration, key);

        /// <summary>
        /// Gets a <typeparamref name="T"/> from the configuration
        /// </summary>
        /// <typeparam name="T">The object type to parse.</typeparam>
        /// <param name="configuration">The configuration source.</param>
        /// <param name="key">The key.</param>
        private static T Get<T>(this IConfiguration configuration, string key)
        {
            string value = configuration[key];
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                throw new FormatException("Invalid value type.");
            }
        }
    }
}