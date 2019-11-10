using Bramf.Extensions;
using System.Collections.Generic;

namespace Bramf.Error
{
    /// <summary>
    /// Provides methods to validate <see cref="string"/>s
    /// </summary>
    public static class StringValidation
    {
        /// <summary>
        /// Validates strings
        /// </summary>
        /// <param name="strings">The strings to validate</param>
        public static StringValidationError[] Validate(params string[] strings)
        {
            // Create collection to store errors
            var errors = new List<StringValidationError>();

            // Foreach string...
            foreach(var str in strings)
            {
                // Check if the string is null or whitespace
                if(str.IsNullOrWhitespace())
                {
                    errors.Add(new StringValidationError
                    {
                        PropertyName = str.GetType().Name
                    });
                }
            }

            // Return all the errors
            return errors.ToArray();
        }
    }
}
