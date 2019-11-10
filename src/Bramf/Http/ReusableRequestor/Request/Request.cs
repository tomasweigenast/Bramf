using Bramf.Http.Authorization;
using Bramf.Validation;
using System.Threading.Tasks;

namespace Bramf.Http
{
    /// <summary>
    /// Represents a request that is in building, waiting to be sent
    /// </summary>
    public class Request : IRequest
    {
        #region Protected Members

        /// <summary>
        /// The request builder
        /// </summary>
        protected HttpRequestBuilder mRequestBuilder;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new HTTP request to a url using a specific http method
        /// </summary>
        /// <param name="url">The url to make the request to.</param>
        /// <param name="method">The HTTP Method to use</param>
        /// <param name="configuration">The configuration of the request</param>
        /// <param name="authorization">The optional authorization protocol to use</param>
        public Request(string url, HttpMethod method, HttpRequestSettings configuration, IAuthorization authorization = null)
        {
            Validator.ValidateString(url, nameof(url));

            // Build the requestor
            mRequestBuilder = new HttpRequestBuilder(url, method)
                .WithAuthorization(authorization)
                .WithConfiguration(configuration);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds an optional header to the request
        /// </summary>
        /// <param name="name">The name of the header</param>
        /// <param name="value">The value of the header</param>
        public IRequest AddAdditionalHeader(string name, string value)
        {
            // Ignore if null
            if (Validator.IsNull(name, value))
                return this;

            // Add header
            mRequestBuilder.WithCustomHeader(name, value);

            return this;
        }

        /// <summary>
        /// Adds a query parameter to the request
        /// </summary>
        /// <param name="key">The key for the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        public IRequest AddQueryParameter(string key, string value)
        {
            // Ignore if null
            if (Validator.IsNull(key, value))
                return this;

            // Add parameter
            mRequestBuilder.AddParameter(key, value);

            return this;
        }

        /// <summary>
        /// Waits for the response
        /// </summary>
        public async Task<HttpResponse> WaitResponseAsync()
        {
            // Send it and get the response
            var response = await mRequestBuilder.SendAsync();

            // Return the response
            return response;
        }

        /// <summary>
        /// Waits for the response and deserializes the content
        /// </summary>
        public async Task<HttpResponse<T>> WaitResponseAsync<T>()
        {
            // Send it and get the response
            var response = await mRequestBuilder.SendAsync<T>();

            // Return the response
            return response;
        }

        /// <summary>
        /// Waits for the response and deserializes the content without parsing <see cref="HttpResponse"/>
        /// </summary>
        public async Task<T> WaitAnonResponseAsync<T>()
        {
            // Send it and get the response
            var response = await mRequestBuilder.RawSendAsync<T>();

            // Return the response
            return response;
        }

        #endregion
    }
}
