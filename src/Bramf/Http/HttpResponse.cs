using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Bramf.Http
{
    /// <summary>
    /// Contains information about a HTTP server response
    /// </summary>
    public class HttpResponse
    {
        #region Properties

        /// <summary>
        /// The response status code and description
        /// </summary>
        public HttpStatusCode Status { get; set; }
        
        /// <summary>
        /// The content length in bytes
        /// </summary>
        public long ContentLength { get; set; }

        /// <summary>
        /// The content type of the response
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The date and time extracted from the header
        /// </summary>
        public DateTimeOffset Date { get; set; }

        /// <summary>
        /// The charset encoding
        /// </summary>
        public Encoding Charset { get; set; }

        /// <summary>
        /// The headers of the response
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// The cookies of the response
        /// </summary>
        public CookieCollection Cookies { get; set; }

        /// <summary>
        /// The content in a raw view
        /// </summary>
        public string RawContent { get; set; }

        /// <summary>
        /// A boolean indicating if the request contains a 200 OK status code
        /// </summary>
        public bool IsSuccessful { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="HttpResponse"/>
        /// </summary>
        public HttpResponse()
        {
            Headers = new Dictionary<string, string>();
        }

        #endregion
    }

    /// <summary>
    /// Contains information about a HTTP server response
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize from the response body.</typeparam>
    public class HttpResponse<T> : HttpResponse
    {
        /// <summary>
        /// The body of the response
        /// </summary>
        public T Body { get; set; }
    }
}
