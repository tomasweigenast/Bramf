namespace Bramf.Http
{
    /// <summary>
    /// Represents a HTTP response status code
    /// </summary>
    public class HttpStatusCode
    {
        /// <summary>
        /// The status code
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// A description of the code
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Creates a new <see cref="HttpStatusCode"/>
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="description">The description.</param>
        public HttpStatusCode(int code, string description)
        {
            Code = code;
            Description = description;
        }

        /// <summary>
        /// Returns a easy way to read the status code and its description
        /// </summary>
        public override string ToString() => $"{Code} ({Description})";
    }
}
