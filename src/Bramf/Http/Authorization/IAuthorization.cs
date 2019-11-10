using Bramf.Extensions;
using Bramf.Validation;
using System;

namespace Bramf.Http.Authorization
{
    /// <summary>
    /// Inteface to help building <see cref="BaseAuthorization"/>
    /// </summary>
    public interface IAuthorization
    {
        /// <summary>
        /// The authorization type
        /// </summary>
        string Type { get; set; }

        /// <summary>
        /// The value for the authorization
        /// </summary>
        string Value { get; set; }

        /// <summary>
        /// Refresh the value
        /// </summary>
        /// <param name="newValue">The new value to set</param>
        void RefreshValue(string newValue);
    }

    /// <summary>
    /// Basic authorization class that each authorization type should implement
    /// </summary>
    public abstract class BaseAuthorization : IAuthorization
    {
        #region Properties

        /// <summary>
        /// The authorization type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The value for the authorization
        /// </summary>
        public string Value { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BaseAuthorization(string type, string value)
        {
            // Avoid bugs
            if (type.IsNullOrWhitespace()) throw new ArgumentNullException(nameof(type));
            if (value.IsNullOrWhitespace()) throw new ArgumentNullException(nameof(value));

            // Set values
            Type = type;
            Value = value;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gives format to the authorization header
        /// </summary>
        public override string ToString()
            => $"{Type} {Value}";

        /// <summary>
        /// Refresh the value
        /// </summary>
        /// <param name="newValue">The new value to set</param>
        public virtual void RefreshValue(string newValue)
        {
            Validator.ValidateString(newValue, nameof(newValue));

            // Ignore if its the same
            if (Value == newValue)
                return;

            // Set the new value
            Value = newValue;
        }

        #endregion
    }
}