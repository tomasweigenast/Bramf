using Bramf.Http.Authorization;
using Bramf.Http.Exceptions;
using Bramf.Validation;
using System;
using System.Timers;

namespace Bramf.Http
{
    /// <summary>
    /// Provides methods to re-use a http request storing cookies, authorization tokens, and so on.
    /// Useful to use with Dependency Injection
    /// </summary>
    public class ReusableHttpRequest : IReusableHttpRequest, IDisposable
    {
        #region Members

        private string mEndpoint;
        private IAuthorization mAuthorization;
        private Guid mImplementationId;
        private HttpRequestSettings mSettings;
        private (Timer timer, Func<string> action) mRefreshAuthorizationTimer;

        #endregion

        #region Properties

        /// <summary>
        /// The base url where all the requests will be made
        /// </summary>
        public string Endpoint => mEndpoint;

        /// <summary>
        /// The authorization method in use
        /// </summary>
        public IAuthorization AuthorizationProtocol => mAuthorization;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="ReusableHttpRequest"/>
        /// </summary>
        /// <param name="endpoint">The endpoint url where all the requests will be made to.</param>
        /// <param name="settings">The configuration for the requestor</param>
        public ReusableHttpRequest(string endpoint, HttpRequestSettings settings)
        {
            try
            {
                // Check the url and extract only the endpoint
                Uri url = new Uri(endpoint);
                mEndpoint = ExtractEndpoint(url);
            }
            catch (UriFormatException)
            {
                throw new InvalidUrlException(endpoint);
            }

            mImplementationId = Guid.NewGuid();
            mSettings = settings;
        }

        #endregion

        #region Methods

        #region Setup

        /// <summary>
        /// Changes the endpoint url of the reusable http request
        /// </summary>
        /// <param name="newEndpoint">The new url</param>
        public void ChangeEndpoint(string newEndpoint)
        {
            try
            {
                // Check the url and extract only the endpoint
                Uri url = new Uri(newEndpoint);
                mEndpoint = ExtractEndpoint(url);
            }
            catch (UriFormatException)
            {
                throw new InvalidUrlException(newEndpoint);
            }
        }

        /// <summary>
        /// Adds an authorization method to the request
        /// </summary>
        /// <param name="authorizationMethod">The authorization method to use</param>
        public ReusableHttpRequest AddAuthorization(IAuthorization authorizationMethod)
        {
            mAuthorization = authorizationMethod;

            return this;
        }

        /// <summary>
        /// Starts a timer that each specific time interval, will update the authorization value
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="getValueAction"></param>
        /// <returns></returns>
        public ReusableHttpRequest WithAuthorizationRefreshEach(TimeSpan interval, Func<string> getValueAction)
        {
            // Avoid bugs
            if (mAuthorization == null) throw new InvalidOperationException("Authorization method not implemented.");

            // Create a timer
            mRefreshAuthorizationTimer.action = getValueAction ?? throw new ArgumentNullException(nameof(getValueAction));
            mRefreshAuthorizationTimer.timer = new Timer(interval.TotalMilliseconds) { AutoReset = true };
            mRefreshAuthorizationTimer.timer.Elapsed += MRefreshAuthorizationTimer_Elapsed;
            mRefreshAuthorizationTimer.timer.Start();

            return this;
        }

        #endregion

        #region Send

        /// <summary>
        /// Sends a <see cref="HttpMethod.GET"/> request to the server
        /// </summary>
        /// <param name="resource">The resource to make the request to. Can be null to make the request to the endpoint url.</param>
        public IRequest SendGet(string resource)
        {
            // Avoid urls
            if (IsUrl(resource)) throw new InvalidOperationException("Resource cannot be an url.");
           
            return new Request(mEndpoint + resource, HttpMethod.GET, mSettings, mAuthorization);
        }

        /// <summary>
        /// Sends a <see cref="HttpMethod.DELETE"/> request to the server
        /// </summary>
        /// <param name="resource">The resource to make the request to. Can be null to make the request to the endpoint url.</param>
        public IRequest SendDelete(string resource)
        {
            // Avoid urls
            if (IsUrl(resource)) throw new InvalidOperationException("Resource cannot be an url.");

            return new Request(mEndpoint + resource, HttpMethod.DELETE, mSettings, mAuthorization);
        }

        /// <summary>
        /// Sends a <see cref="HttpMethod.POST"/> request to the server
        /// </summary>
        /// <param name="resource">The resource to make the request to. Can be null to make the request to the endpoint url.</param>
        public IBodyRequest SendPost(string resource)
        {
            // Avoid urls
            if (IsUrl(resource)) throw new InvalidOperationException("Resource cannot be an url.");

            return new BodyRequest(mEndpoint + resource, HttpMethod.POST, mSettings, mAuthorization);
        }

        /// <summary>
        /// Sends a <see cref="HttpMethod.PUT"/> request to the server
        /// </summary>
        /// <param name="resource">The resource to make the request to. Can be null to make the request to the endpoint url.</param>
        public IBodyRequest SendPut(string resource)
        {
            // Avoid urls
            if (IsUrl(resource)) throw new InvalidOperationException("Resource cannot be an url.");

            return new BodyRequest(mEndpoint + resource, HttpMethod.PUT, mSettings, mAuthorization);
        }

        /// <summary>
        /// Sends a <see cref="HttpMethod.PATCH"/> request to the server
        /// </summary>
        /// <param name="resource">The resource to make the request to. Can be null to make the request to the endpoint url.</param>
        public IBodyRequest SendPatch(string resource)
        {
            // Avoid urls
            if (IsUrl(resource)) throw new InvalidOperationException("Resource cannot be an url.");

            return new BodyRequest(mEndpoint + resource, HttpMethod.PATCH, mSettings, mAuthorization);
        }

        /// <summary>
        /// Sends a request to a server using a specific http method
        /// </summary>
        /// <param name="resource">The resource to make the request to. Can be null to make the request to the endpoint url.</param>
        /// <param name="method">The HTTP method to use</param>
        public IRequest Send(string resource, HttpMethod method)
        {
            // Avoid urls
            if (IsUrl(resource)) throw new InvalidOperationException("Resource cannot be an url.");

            return new Request(mEndpoint + resource, method, mSettings, mAuthorization);
        }

        #endregion

        #endregion

        #region Disposing methods

        /// <summary>
        /// Disposes this instance
        /// </summary>
        public void Dispose()
        {
            // Set null values
            mEndpoint = null;
            mAuthorization = null;

            // Stop timer if started
            if (mRefreshAuthorizationTimer.timer != null)
            {
                mRefreshAuthorizationTimer.timer.Stop(); // Stop timer
                mRefreshAuthorizationTimer.timer.Elapsed -= MRefreshAuthorizationTimer_Elapsed; // Remove event

                // Set to null
                mRefreshAuthorizationTimer.timer = null;
                mRefreshAuthorizationTimer.action = null;
            }

            GC.SuppressFinalize(this);
        }

        #endregion

        #region Private Helper Methods

        // Fired when timer ticks
        void MRefreshAuthorizationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Get and update authorization value
            mAuthorization.RefreshValue(mRefreshAuthorizationTimer.action.Invoke());
        }

        // Extracts the endpoint from a url
        string ExtractEndpoint(Uri url)
        {
            // Validate if null
            Validator.Validate(url, nameof(url));

            return $"{url.Scheme}{Uri.SchemeDelimiter}{url.Authority}";
        }

        // Indicates if a string is an url
        bool IsUrl(string input)
        {
            if (input.StartsWith("http://"))
                return true;

            if (input.StartsWith("https://"))
                return true;

            if (input.StartsWith("www."))
                return true;

            return false;
        }

        #endregion
    }
}