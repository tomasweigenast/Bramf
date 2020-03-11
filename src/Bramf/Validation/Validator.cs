using System;

namespace Bramf.Validation
{
    /// <summary>
    /// Provides methods to validate variables
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Throws a <see cref="ArgumentNullException"/> if <paramref name="variable"/> is null
        /// </summary>
        public static void NotNull(object variable)
        {
            if (variable == null)
                throw new ArgumentNullException(nameof(variable), "Variable cannot be null.");
        }

        /// <summary>
        /// Throws a <see cref="ArgumentNullException"/> if any variable passed is null
        /// </summary>
        public static void NotNull(params object[] variables)
        {
            foreach(var variable in variables)
                NotNull(variable);
        }

        /// <summary>
        /// Returns true if variable is null, otherwise, false.
        /// </summary>
        public static bool IsNull(object variable)
            => variable == null;

        /// <summary>
        /// Returns true if any variable passed is null
        /// </summary>
        public static bool AnyNull(params object[] variables)
        {
            foreach (var variable in variables)
            {
                if (variable == null)
                    return true;

                continue;
            }

            return false;
        }
    }
}