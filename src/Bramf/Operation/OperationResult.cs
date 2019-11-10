using System.Collections.Generic;
using System.Linq;

namespace Bramf.Operation
{
    /// <summary>
    /// Represents the result of an operation
    /// </summary>
    public class OperationResult
    {
        #region Private Members

        protected readonly List<OperationError> mErrors = new List<OperationError>();
        private static readonly OperationResult mSuccess = new OperationResult { Succeeded = true };

        #endregion

        #region Public Properties

        /// <summary>
        /// Indicates if the operation was successful or not.
        /// </summary>
        public bool Succeeded { get; protected set; }

        /// <summary>
        /// Contains all errors that ocurred during the identity operation
        /// </summary>
        public IEnumerable<OperationError> Errors => mErrors;

        #endregion

        #region Methods

        /// <summary>
        /// Returns an succesful operation
        /// </summary>
        public static OperationResult Success => mSuccess;

        /// <summary>
        /// Creates an <see cref="OperationResult"/> indicating a failed operation
        /// </summary>
        /// <param name="errors">An optional array of <see cref="OperationError"/> which caused the operation to fail.</param>
        public static OperationResult Failed(params OperationError[] errors)
        {
            var result = new OperationResult { Succeeded = false };
            if (errors != null)
                result.mErrors.AddRange(errors);

            return result;
        }

        /// <summary>
        /// Creates an <see cref="OperationError"/> indicating a failed operation. 
        /// Implicit conversion can be used, so you can write an string in the <paramref name="error"/>
        /// </summary>
        /// <param name="error">An optional <see cref="OperationError"/> which caused the operation to fail.</param>
        public static OperationResult Failed(OperationError error)
        {
            var result = new OperationResult { Succeeded = false };
            if (error != null)
                result.mErrors.Add(error);

            return result;
        }

        /// <summary>
        /// Implicit conversion of the current <see cref="OperationResult"/> object to its string representation
        /// </summary>
        /// <remarks>
        /// Returns a string with each error separated by a comma
        /// </remarks>
        public override string ToString()
            => mErrors
            .Select(x => x.Description)
            .Aggregate((a, b) => $"{a}, {b}");

        #endregion
    }

    /// <summary>
    /// Represents the result of an operation that returned an object
    /// </summary>
    public class OperationResult<T> : OperationResult
    {
        #region Public Properties

        /// <summary>
        /// The object returned
        /// </summary>
        public T Object { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns an succesful operation and returns an object
        /// </summary>
        /// <typeparam name="Object">The type of the object to be returned</typeparam>
        /// <param name="objectToReturn">The object to return</param>
        public static new OperationResult<Object> Success<Object>(Object objectToReturn)
            => new OperationResult<Object>
            {
                Succeeded = true,
                Object = objectToReturn
            };

        /// <summary>
        /// Creates an <see cref="OperationResult"/> indicating a failed operation
        /// </summary>
        /// <param name="errors">An optional array of <see cref="OperationError"/> which caused the operation to fail.</param>
        public static new OperationResult<T> Failed(params OperationError[] errors)
        {
            var result = new OperationResult<T> { Succeeded = false };
            if (errors != null)
                result.mErrors.AddRange(errors);

            return result;
        }

        /// <summary>
        /// Creates an <see cref="OperationError"/> indicating a failed operation. 
        /// Implicit conversion can be used, so you can write an string in the <paramref name="error"/>
        /// </summary>
        /// <param name="error">An optional <see cref="OperationError"/> which caused the operation to fail.</param>
        public static new OperationResult<T> Failed(OperationError error)
        {
            var result = new OperationResult<T> { Succeeded = false };
            if (error != null)
                result.mErrors.Add(error);

            return result;
        }

        #endregion
    }
}
