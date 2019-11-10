using Bramf.Http.Authorization;
using Bramf.Http.Exceptions;
using Bramf.Http.Serialization;
using Bramf.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bramf.Http
{
    /// <summary>
    /// Helps building Http requests
    /// </summary>
    public class HttpRequestBuilder
    {
        #region Internal Members

        internal Uri RequestUrl;
        internal HttpMethod HttpMethod;
        internal IAuthorization Authorization;
        internal HttpRequestSettings Settings;
        internal List<string> RequestParameters;
        internal Dictionary<string, string> CustomHeaders;
        internal (IHttpContentSerializer serializer, int contentLength, string content) Body;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="HttpRequestBuilder"/> with an specific endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint to make the request to</param>
        public HttpRequestBuilder(string endpoint)
        {
            try
            {
                RequestUrl = new Uri(endpoint);
            }
            catch
            {
                throw new InvalidUrlException(endpoint);
            }
            HttpMethod = HttpMethod.GET;
            Settings = new HttpRequestSettings();
        }

        /// <summary>
        /// Creates a new <see cref="HttpRequestBuilder"/> with an specific endpoint and request method
        /// </summary>
        /// <param name="endpoint">The endpoint to make the request to</param>
        /// <param name="httpMethod">The HTTP Method to use</param>
        public HttpRequestBuilder(string endpoint, HttpMethod httpMethod)
        {
            try
            {
                RequestUrl = new Uri(endpoint);
            }
            catch
            {
                throw new InvalidUrlException(endpoint);
            }

            HttpMethod = httpMethod;
            Settings = new HttpRequestSettings();
        }

        #endregion

        #region Methods

        #region Authorization

        /// <summary>
        /// Adds an authorization system
        /// </summary>
        /// <param name="authorization">The authorization system to use</param>
        public HttpRequestBuilder WithAuthorization(IAuthorization authorization)
        {
            // Ignore if null
            if (authorization == null) return this;

            // Prevent double authorization header
            if (Authorization != null)
                throw new InvalidOperationException("Authorization header cannot be added twice.");

            // Set header
            Authorization = authorization;
            return this;
        }

        #endregion

        #region Configure

        /// <summary>
        /// Set specific http request settings
        /// </summary>
        public HttpRequestBuilder WithConfiguration(Action<HttpRequestSettings> options)
        {
            // Get settings
            options.Invoke(Settings);

            return this;
        }

        /// <summary>
        /// Set specific http request settings
        /// </summary>
        public HttpRequestBuilder WithConfiguration(HttpRequestSettings options)
        {
            Validator.Validate(options, nameof(options));

            Settings = options;

            return this;
        }

        /// <summary>
        /// Adds a custom header
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <param name="value">The header value.</param>
        public HttpRequestBuilder WithCustomHeader(string name, string value)
        {
            // Create if null
            if (CustomHeaders == null) CustomHeaders = new Dictionary<string, string>();

            // Avoid bugs
            Validator.ValidateStrings(name, value);

            // Add header
            CustomHeaders.Add(name, value);

            return this;
        }

        /// <summary>
        /// Adds a list of custom headers
        /// </summary>
        /// <param name="headers">The key-value pair list to add</param>
        public HttpRequestBuilder WithCustomHeaders(IEnumerable<KeyValuePair<string, string>> headers)
        {
            foreach (var header in headers)
                return WithCustomHeader(header.Key, header.Value);

            return this;
        }

        #endregion

        #region Parameters

        /// <summary>
        /// Adds a single parameter to the request
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        public HttpRequestBuilder AddParameter(string key, string value)
        {
            // Create list
            if (RequestParameters == null) RequestParameters = new List<string>();

            // Avoid bugs
            Validator.ValidateStrings(key, value);

            // Add parameter
            RequestParameters.Add($"{key}={value}");

            return this;
        }

        /// <summary>
        /// Adds a list of parameters to the request
        /// </summary>
        /// <param name="parameters">The key-value pair list</param>
        public HttpRequestBuilder AddParameters(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            foreach (var parameter in parameters)
                return AddParameter(parameter.Key, parameter.Value);

            return this;
        }

        #endregion

        #region Content

        /// <summary>
        /// Adds a body to the request
        /// </summary>
        /// <param name="content">The content to add</param>
        /// <param name="serializer">The content serializer to use</param>
        public HttpRequestBuilder WithContent(object content, IHttpContentSerializer serializer)
        {
            // Avoid body if http method does not allows it
            if (!HttpMethod.AllowsBody()) throw new InvalidOperationException($"HTTP {HttpMethod} does not allow body."); 

            // If content is null, set length to 0 and continue
            if (content == null)
                Body.contentLength = 0;

            // Otherwise, serialize content
            else
            {
                // Prevent null serializer
                Validator.Validate(serializer, nameof(serializer));

                // Serialize content
                string serialized = serializer.SerializeContent(content);

                // Set content
                Body.serializer = serializer;
                Body.contentLength = Encoding.Unicode.GetBytes(serialized).Length;
                Body.content = serialized;
            }

            return this;
        }

        /// <summary>
        /// Adds a body to the request using JSON content encoding
        /// </summary>
        /// <param name="content">The content to add</param>
        public HttpRequestBuilder WithContent(object content)
        {
            // Avoid body if http method does not allows it
            if (!HttpMethod.AllowsBody()) throw new InvalidOperationException($"HTTP {HttpMethod} does not allow body.");

            // If content is null, set length to 0 and continue
            if (content == null)
                Body.contentLength = 0;

            // Otherwise, serialize content
            else
            {
                var serializer = new JsonContentSerializer();

                // Serialize content
                string serialized = serializer.SerializeContent(content);

                // Set content
                Body.serializer = serializer;
                Body.contentLength = Encoding.UTF8.GetBytes(serialized).Length;
                Body.content = serialized;
            }

            return this;
        }

        #endregion

        #endregion
    }
}