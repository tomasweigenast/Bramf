namespace Bramf.Http
{
    /// <summary>
    /// A list of all allowed HTTP 1.1/2.0 request methods
    /// </summary>
    public enum HttpMethod
    {
        /// <summary>
        /// The GET method requests a representation of the specified resource. Requests using GET should only retrieve data.
        /// </summary>
        GET,

        /// <summary>
        /// The HEAD method asks for a response identical to that of a GET request, but without the response body.
        /// </summary>
        HEAD,

        /// <summary>
        /// The POST method is used to submit an entity to the specified resource, often causing a change in state or side effects on the server.
        /// </summary>
        POST,

        /// <summary>
        /// The PUT method replaces all current representations of the target resource with the request payload.
        /// </summary>
        PUT,

        /// <summary>
        /// The DELETE method deletes the specified resource.
        /// </summary>
        DELETE,

        /// <summary>
        /// The CONNECT method establishes a tunnel to the server identified by the target resource.
        /// </summary>
        CONNECT,

        /// <summary>
        /// The TRACE method performs a message loop-back test along the path to the target resource.
        /// </summary>
        TRACE,

        /// <summary>
        /// The PATCH method is used to apply partial modifications to a resource.
        /// </summary>
        PATCH,

        /// <summary>
        /// The OPTIONS method is used to describe the communication options for the target resource.
        /// </summary>
        OPTIONS
    }

    /// <summary>
    /// Helper methods to work with <see cref="HttpMethod"/>'s
    /// </summary>
    public static class HttpMethodHelpers
    {
        /// <summary>
        /// Indicates if a <see cref="HttpMethod"/> allows body content
        /// </summary>
        /// <param name="method">The method to check for.</param>
        public static bool AllowsBody(this HttpMethod method)
        {
            switch(method)
            {
                case HttpMethod.POST:
                    return true;

                case HttpMethod.PATCH:
                    return true;

                case HttpMethod.PUT:
                    return true;

                case HttpMethod.TRACE:
                    return true;

                default:
                    return false;
            }
        }
    }
}
