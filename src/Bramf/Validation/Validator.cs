using System;

namespace Bramf.Validation
{
    /// <summary>
    /// Provides methods to validate variables
    /// </summary>
    public static class Validator
    {
        #region Null Validation 

        /// <summary>
        /// If the variable is null, it will throw a <see cref="ArgumentNullException"/>
        /// </summary>
        /// <param name="variable">The variable to check if its null</param>
        public static void Validate(object variable)
        {
            if (variable == null)
                throw new ArgumentNullException();
        }

        /// <summary>
        /// If the variable is null, it will throw a <see cref="ArgumentNullException"/>
        /// </summary>
        /// <param name="variable">The variable to check if its null</param>
        /// <param name="variableName">The name of the variable to check</param>
        public static void Validate(object variable, string variableName)
        {
            if (variable == null)
                throw new ArgumentNullException(variableName);
        }

        /// <summary>
        /// If the variable is null, it will throw a <see cref="ArgumentNullException"/>
        /// </summary>
        /// <param name="variable">The variable to check if its null</param>
        /// <param name="variableName">The name of the variable to check</param>
        /// <param name="errorMessage">An special error message to show</param>
        public static void Validate(object variable, string variableName, string errorMessage)
        {
            if (variable == null)
                throw new ArgumentNullException(variableName, errorMessage);
        }

        /// <summary>
        /// Validates multiples variables at once
        /// </summary>
        /// <param name="variables">An enumerable of variables</param>
        public static void ValidateAll(params object[] variables)
        {
            foreach (object variable in variables)
                Validate(variable);
        }

        /// <summary>
        /// Checks if an string is null or contains online whitespaces, if its, it will throw an <see cref="ArgumentNullException"/>
        /// </summary>
        /// <param name="str">The string to check</param>
        /// <param name="variableName">The name of the variable to check</param>
        /// <param name="errorMessage">An special error message to show</param>
        public static void ValidateString(string str, string variableName = null, string errorMessage = null)
        {
            if (str == null || string.IsNullOrWhiteSpace(str))
                throw new ArgumentNullException(variableName == null ? "string" : variableName, errorMessage == null ? "The string cannot be null or contain only whitespaces." : errorMessage);
        }

        /// <summary>
        /// Validates a list of strings if they are null or contains online whitespaces, if its, it will throw an <see cref="ArgumentNullException"/>
        /// </summary>
        /// <param name="strings">An enumerable of strings to check</param>
        public static void ValidateStrings(params string[] strings)
        {
            foreach (string str in strings)
                ValidateString(str);
        }

        #endregion

        #region Boolean Return

        /// <summary>
        /// Returns true if the input variable is null
        /// </summary>
        /// <param name="variable">The variable to check</param>
        public static bool IsNull(object variable)
        {
            if (variable == null)
                return true;

            return false;
        }

        /// <summary>
        /// Returns true if at least one of the variables is null
        /// </summary>
        /// <param name="variables">An enumerable of variables</param>
        public static bool IsNull(params object[] variables)
        {
            foreach (object variable in variables)
                if (variable == null)
                    return true;

            return false;
        }

        /// <summary>
        /// Returns true if the input string is null or contains only whitespaces
        /// </summary>
        /// <param name="str">The string to check</param>
        public static bool IsStringNull(string str)
        {
            if (str == null || string.IsNullOrWhiteSpace(str))
                return true;

            return false;
        }

        /// <summary>
        /// Returns true if at least one of the input strings is null or contains only whitespaces
        /// </summary>
        /// <param name="strings">A enumerable of strings to check</param>
        public static bool IsStringNull(params string[] strings)
        {
            foreach (string str in strings)
                if (str == null || string.IsNullOrWhiteSpace(str))
                    return true;

            return false;
        }

        #endregion
    }
}