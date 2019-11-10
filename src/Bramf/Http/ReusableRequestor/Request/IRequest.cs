using Bramf.Http.Serialization;
using System.Threading.Tasks;

namespace Bramf.Http
{
    /// <summary>
    /// Represents a request that is in building, waiting to be sent
    /// </summary>
    public interface IRequest
    {
        /// <summary>
        /// Adds a query parameter to the request
        /// </summary>
        /// <param name="key">The key for the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        IRequest AddQueryParameter(string key, string value);

        /// <summary>
        /// Adds an optional header to the request
        /// </summary>
        /// <param name="name">The name of the header</param>
        /// <param name="value">The value of the header</param>
        IRequest AddAdditionalHeader(string name, string value);

        /// <summary>
        /// Waits for the response
        /// </summary>
        Task<HttpResponse> WaitResponseAsync();

        /// <summary>
        /// Waits for the response and deserializes the content
        /// </summary>
        Task<HttpResponse<T>> WaitResponseAsync<T>();

        /// <summary>
        /// Waits for the response and deserializes the content without parsing <see cref="HttpResponse"/>
        /// </summary>
        Task<T> WaitAnonResponseAsync<T>();
    }

    /// <summary>
    /// Represents a request that can contain content in its body
    /// </summary>
    public interface IBodyRequest : IRequest
    {
        /// <summary>
        /// Adds content to the request body using JSON serialization
        /// </summary>
        /// <param name="content">The content to add</param>
        IBodyRequest AddContent(object content);

        /// <summary>
        /// Adds content to the request body
        /// </summary>
        /// <param name="content">The content to add</param>
        /// <param name="serializer">The serializer to use</param>
        IBodyRequest AddContent(object content, IHttpContentSerializer serializer);
    }
}
