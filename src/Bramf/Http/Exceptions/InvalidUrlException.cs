using System;

namespace Bramf.Http.Exceptions
{
    /// <summary>
    /// Thrown when a <see cref="Uri"/> is mal formatted
    /// </summary>
    public class InvalidUrlException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public InvalidUrlException() : base("The specified url has a wrong format.") { }

        /// <summary>
        /// Default constructor
        /// </summary>
        public InvalidUrlException(string url) : base($"The specified uri has a wrong format.\nUrl: {url}") { }

        /// <summary>
        /// Default constructor
        /// </summary>
        public InvalidUrlException(string message, string url) : base(message + $"\nUrl: {url}") { }
    }
}
