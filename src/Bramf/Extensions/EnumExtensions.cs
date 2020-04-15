using System;
using System.ComponentModel;
using System.Reflection;

namespace Bramf.Extensions
{
    /// <summary>
    /// A set of methods to help with Enumerators
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Converts a <see cref="string"/> to specific enum
        /// </summary>
        /// <typeparam name="T">The type of enum to convert to</typeparam>
        /// <param name="value">The string value to converter</param>
        /// <param name="ignoreCase">If true, the case will be ignored</param>
        /// <returns>Returns the desired enum</returns>
        public static T ToEnum<T>(this string value, bool ignoreCase = true)
        {
            T result;

            // Convert
            try
            {
                result = (T)Enum.Parse(typeof(T), value, ignoreCase);
            }
            catch(Exception)
            {
                throw;
            }

            // Return
            return result;
        }

        /// <summary>
        /// Converts a <see cref="string"/> to specific enum
        /// </summary>
        /// <typeparam name="T">The type of enum to convert to</typeparam>
        /// <param name="value">The string value to converter</param>
        /// <param name="defaultValue">A default value to set if the conversion fail</param>
        /// <param name="ignoreCase">If true, the case will be ignored</param>
        /// <returns>Returns the desired enum</returns>
        public static T ToEnum<T>(this string value, T defaultValue, bool ignoreCase = true)
            where T : struct
        {
            // If the string is null, return default value
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            // Convert and return
            return Enum.TryParse<T>(value, ignoreCase, out T result) ? result : defaultValue;
        }

        /// <summary>
        /// Gets the string in the <see cref="DescriptionAttribute"/> of an an enumeration
        /// </summary>
        /// <typeparam name="T">The type of enum to get the attribute</typeparam>
        /// <param name="enumerationValue">The enumeration value to get its description</param>
        /// <returns>Returns the description string</returns>
        public static string GetDescription<T>(this T enumerationValue)
            where T : struct
        {
            // Get the type of the enumeration value
            var type = enumerationValue.GetType();

            // If the parameter is not an enum
            if (!type.IsEnum)
                throw new ArgumentException("EnumerationValue must be of Enum type.", nameof(enumerationValue));

            // Tries to find a DescriptionAttribute
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if(memberInfo != null && memberInfo.Length > 0)
            {
                // Get attributes of type DescriptionAttribute
                object[] attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if(attributes != null && attributes.Length > 0)
                    // Return the description value
                    return ((DescriptionAttribute)attributes[0]).Description;
            }

            // If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();
        }
    }
}
