namespace Bramf.Http.Authorization
{
    /// <summary>
    /// A JWT Bearer token authorization
    /// </summary>
    public class BearerAuthorization : BaseAuthorization
    {
        /// <summary>
        /// Creates a new JWT Bearer token authorization
        /// </summary>
        /// <param name="bearerToken">The bearer token</param>
        public BearerAuthorization(string bearerToken) : base("Bearer", bearerToken) { }
    }
}
