using Bramf.Http.Authorization;
using Bramf.Http.Serialization;
using System;

namespace Bramf.Http
{
    /// <summary>
    /// Represents a request that can contain content in its body
    /// </summary>
    public class BodyRequest : Request, IBodyRequest
    {
        #region Constructor

        /// <summary>
        /// Creates a new HTTP request to a url using a specific http method
        /// </summary>
        /// <param name="url">The url to make the request to.</param>
        /// <param name="method">The HTTP Method to use</param>
        /// <param name="configuration">The configuration of the request</param>
        /// <param name="authorization">The optional authorization protocol to use</param>
        public BodyRequest(string url, HttpMethod method, HttpRequestSettings configuration, IAuthorization authorization = null)
            : base(url, method, configuration, authorization)
        {
            if (!method.AllowsBody()) throw new InvalidOperationException("The specified method does not allows body content.");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds content to the request body using JSON serialization
        /// </summary>
        /// <param name="content">The content to add</param>
        public IBodyRequest AddContent(object content)
        {
            mRequestBuilder.WithContent(content);

            return this;
        }

        /// <summary>
        /// Adds content to the request body
        /// </summary>
        /// <param name="content">The content to add</param>
        /// <param name="serializer">The serializer to use</param>
        public IBodyRequest AddContent(object content, IHttpContentSerializer serializer)
        {
            mRequestBuilder.WithContent(content, serializer);

            return this;
        }

        #endregion
    }
}
