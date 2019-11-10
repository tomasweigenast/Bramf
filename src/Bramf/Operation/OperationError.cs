using System;

namespace Bramf.Operation
{
    /// <summary>
    /// Encapsulates an error from the <see cref="OperationResult"/>
    /// </summary>
    public class OperationError
    {
        #region Properties

        /// <summary>
        /// Gets or sets the code for this error
        /// </summary>
        public string Code { get; set; } = "UnknownError";

        /// <summary>
        /// Gets or sets the description for this error
        /// </summary>
        public string Description { get; set; } = "Unknown error.";

        /// <summary>
        /// Any exception to include in the error
        /// </summary>
        public Exception Exception { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the value of the current <see cref="OperationError"/> object to its equivalent string representation.
        /// </summary>
        /// <remarks>
        /// It returns the error this way if returnCode is true: "{Description}" [{Code}]. Otherwise, it will return only the description
        /// </remarks>
        public string ToString(bool returnCode = false)
        {
            if (returnCode)
                return $"\"{Description}\" [{Code}]";
            else
                return $"\"{Description}\"";
        }

        /// <summary>
        /// Implicit conversion of an <see cref="string"/> to an <see cref="OperationError"/>
        /// </summary>
        public static implicit operator OperationError(string errorDescription)
            => new OperationError { Description = errorDescription };

        #endregion
    }
}
