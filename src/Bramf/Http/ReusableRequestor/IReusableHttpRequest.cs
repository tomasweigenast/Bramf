namespace Bramf.Http
{
    /// <summary>
    /// Defines the base class of a HTTP request that can be reusable
    /// </summary>
    public interface IReusableHttpRequest
    {
        /// <summary>
        /// Sends a <see cref="HttpMethod.GET"/> request to a specific resource
        /// </summary>
        /// <param name="resource">The resource to make the request to</param>
        IRequest SendGet(string resource);

        /// <summary>
        /// Sends a <see cref="HttpMethod.POST"/> request to a specific resource
        /// </summary>
        /// <param name="resource">The resource to make the request to</param>
        IBodyRequest SendPost(string resource);

        /// <summary>
        /// Sends a <see cref="HttpMethod.PUT"/> request to a specific resource
        /// </summary>
        /// <param name="resource">The resource to make the request to</param>
        IBodyRequest SendPut(string resource);

        /// <summary>
        /// Sends a <see cref="HttpMethod.PATCH"/> request to a specific resource
        /// </summary>
        /// <param name="resource">The resource to make the request to</param>
        IBodyRequest SendPatch(string resource);

        /// <summary>
        /// Sends a <see cref="HttpMethod.DELETE"/> request to a specific resource
        /// </summary>
        /// <param name="resource">The resource to make the request to</param>
        IRequest SendDelete(string resource);

        /// <summary>
        /// Sends a request using a specific <see cref="HttpMethod"/>
        /// </summary>
        /// <param name="resource">The resource to make the request to</param>
        /// <param name="method">The Http method to use</param>
        IRequest Send(string resource, HttpMethod method);
    }
}
