using Bramf.Http.Serialization;
using System;
using System.Text;

namespace Bramf.Http
{
    /// <summary>
    /// Properties to configure a <see cref="HttpRequest"/>
    /// </summary>
    public class HttpRequestSettings
    {
        /// <summary>
        /// The request timeout
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Indicates if an auto-redirect must be allowed
        /// </summary>
        public bool AllowAutoRedirect { get; set; } = true;

        /// <summary>
        /// The user agent to use
        /// </summary>
        public string UserAgent { get; set; } = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        /// <summary>
        /// The data type accepted
        /// </summary>
        public IHttpContentSerializer AcceptType { get; set; } = new JsonContentSerializer();

        /// <summary>
        /// The encoding accepted
        /// </summary>
        public string AcceptEncoding { get; set; } = "gzip,deflate";

        /// <summary>
        /// The charset accepted
        /// </summary>
        public Encoding Charset { get; set; } = Encoding.UTF8;
    }
}
