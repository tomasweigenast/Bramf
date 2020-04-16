using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace Bramf.Extensions
{
    /// <summary>
    /// Provides methods to help with <see cref="string"/>
    /// </summary>
    public static class StringExtensions
    {
        #region Constants

        static readonly string[] SIZE_SUFFIXES = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        static readonly Regex EMAIL_PATTERN_REGEX = new Regex(@"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$");

        #endregion

        #region Methods

        /// <summary>
        /// Takes a string array and checks for null/whitespace values
        /// </summary>
        /// <param name="values">The string values</param>
        /// <returns>Returns true if there is a null/whitespace values</returns>
        public static bool IsNullOrWhitespace(params string[] values)
        {
            bool error = false;

            // Foreach value
            foreach(var value in values)
            {
                if (string.IsNullOrWhiteSpace(value))
                    error = true;
            }

            // Return error
            return error;
        }

        /// <summary>
        /// Indicates whatever a string is null or contains white spaces
        /// </summary>
        /// <param name="value">The string</param>
        public static bool IsNullOrWhitespace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Indicates whatever a string is null or empty
        /// </summary>
        /// <param name="value">The string</param>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Takes plain text and encode it to Base64
        /// </summary>
        /// <param name="plainText">Plain text to encode</param>
        public static string ToBase64(this string plainText)
        {
            if (plainText.IsNullOrWhitespace())
                throw new ArgumentNullException(nameof(plainText));

            try
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
            }
            catch(Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Takes a byte array and encodes it using Base64
        /// </summary>
        /// <param name="bytes">The byte array to encode</param>
        public static string ToBase64(this byte[] bytes)
            => Convert.ToBase64String(bytes);

        /// <summary>
        /// Takes a Base64 encoded string and returns a byte array
        /// </summary>
        public static byte[] FromBase64AsByteArray(this string encoded)
            => Convert.FromBase64String(encoded);

        /// <summary>
        /// Takes a base64 encoded string and converts it to plain text
        /// </summary>
        /// <param name="base64Converted">Base64 encoded string</param>
        public static string FromBase64(this string base64Converted)
        {
            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(base64Converted));
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Takes a boolean a returns an string containing "Yes" or "No"
        /// depending boolean value.
        /// </summary>
        /// <param name="boolean"></param>
        /// <param name="culture">The culture to translate the text to.</param>
        public static string ToYesNo(this bool boolean, CultureInfo culture)
        {
            string text = string.Empty;

            if (culture.Name == "es-ES")
                if (boolean)
                    return "Si";
                else
                    return "No";
            else
                if (boolean)
                    return "Yes";
                else
                    return "No";
        }

        /// <summary>
        /// Unsecures a <see cref="SecureString"/> to plain text
        /// </summary>
        /// <param name="secureString">The secure string</param>
        public static string Unsecure(this SecureString secureString)
        {
            // Return empty string if there is no secure string
            if (secureString == null)
                return string.Empty;

            // Get a pointer for an unsecure string in memory
            var unmanagedString = IntPtr.Zero;

            try
            {
                // Unsecures the secure string
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);

                // Return secure string as plain text
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                // Clean up any memory allocation
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        /// <summary>
        /// Takes an <see cref="Int64"/> value and converts it to human-readable size adding suffix
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="decimalPlaces">The decimal places to add. By default is 1.</param>
        public static string SizeSuffix(this long value, int decimalPlaces = 1)
        {
            // If the value is less than 0, return negative
            if (value < 0) { return "-" + SizeSuffix(-value); }

            int i = 0;
            decimal dValue = value;
            while (Math.Round(dValue, decimalPlaces) >= 1000)
            {
                dValue /= 1024;
                i++;
            }

            // Return formatted string
            return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SIZE_SUFFIXES[i]);
        }

        /// <summary>
        /// Extracts a substring from a starting string to the end of it
        /// </summary>
        /// <param name="str">The source string</param>
        /// <param name="value">The value to search for and extract to end</param>
        public static string ExtractString(this string str, string value)
            => str.Substring(str.IndexOf(value) + value.Length);

        /// <summary>
        /// Extracts a substring from a starting string index to the specified end index
        /// </summary>
        /// <param name="str">The source string</param>
        /// <param name="value">The value to search for and extract to end</param>
        /// <param name="count">The count of characters to get to the end</param>
        public static string ExtractString(this string str, string value, int count)
            => str.Substring(str.IndexOf(value) + value.Length, count);

        /// <summary>
        /// Extracts a substring from a starting string index to the specified string index
        /// </summary>
        /// <param name="str">The source string</param>
        /// <param name="value">The value to search for and extract to end</param>
        /// <param name="finalValue">The another value to get the index an extract from it</param>
        public static string ExtractString(this string str, string value, string finalValue)
            => str.Substring(str.IndexOf(value) + value.Length, str.IndexOf(finalValue));

        /// <summary>
        /// Extracts a substring from the starting of a source string to the end of the specified value
        /// </summary>
        /// <param name="str">The source string</param>
        /// <param name="finalValue">The value to search for and extract to end</param>
        public static string ExtractStringFromStart(this string str, string finalValue)
            => str.Substring(0, str.IndexOf(finalValue));

        /// <summary>
        /// Returns true if an string contains numbers
        /// </summary>
        /// <param name="input">The input string</param>
        public static bool IsAlphaNumeric(this string input)
            => Regex.IsMatch(input, @"\d");

        /// <summary>
        /// Returns true if an string is a number or a sequence of numbers
        /// </summary>
        /// <param name="input">The input string</param>
        public static bool IsNumber(this string input)
            => Regex.IsMatch(input, @"^\d+$");

        /// <summary>
        /// Extracts all number occurrences in a string
        /// </summary>
        /// <param name="input">The input string</param>
        public static int[] ExtractNumbers(this string input)
        {
            // Split string and get numbers
            string[] stringSplit = Regex.Split(input, @"\D+");
            List<int> numbers = new List<int>();

            // Get only numbers and remove empty entries
            foreach (var str in stringSplit)
                if (!str.IsNullOrWhitespace() && str.IsNumber() && int.TryParse(str, out int number))
                    numbers.Add(number);

            // If there is no numbers, throw exception
            if (numbers.Count <= 0)
                throw new InvalidOperationException("Input string does not contain any number.");

            return numbers.ToArray();
        }

        /// <summary>
        /// Returns true if a string starts with a number
        /// </summary>
        /// <param name="input">The input string</param>
        public static bool StartWithNumber(this string input)
            => char.IsDigit(input.First());

        /// <summary>
        /// Checks if a string is a valid email address
        /// </summary>
        /// <param name="input">The input string to check</param>
        public static bool IsEmail(this string input)
            => (!string.IsNullOrWhiteSpace(input)) && EMAIL_PATTERN_REGEX.IsMatch(input);

        /// <summary>
        /// Truncates a string with a character ellipsis
        /// </summary>
        /// <param name="value">The string to truncate</param>
        /// <param name="maxChars">The max chars to show</param>
        public static string Truncate(this string value, int maxChars) 
            => value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";

        /// <summary>
        /// Makes a string first letter upper case
        /// </summary>
        /// <param name="value">The input string</param>
        /// <param name="containWhitespace">Indicates if the string contains other words separated by whitespaces that must be uppercased too</param>
        public static string Title(this string value, bool containWhitespace)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (containWhitespace)
            {
                string[] strs = value.Split(' ');
                if (strs.Length > 0)
                {
                    string[] finalString = new string[strs.Length];
                    for (int i = 0; i < strs.Length; i++)
                        finalString[i] = char.ToUpper(strs[i][0]) + strs[i].Substring(1);

                    return string.Join(" ", finalString);
                }
            }

            if (value.Length > 1)
                return char.ToUpper(value[0]) + value.Substring(1);

            return value.ToUpper();
        }

#endregion
    }
}