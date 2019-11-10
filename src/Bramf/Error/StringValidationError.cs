namespace Bramf.Error
{
    /// <summary>
    /// Model class used to validate a set of <see cref="string"/> 
    /// </summary>
    public class StringValidationError
    {
        /// <summary>
        /// The name of the property
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// The error message to display
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
