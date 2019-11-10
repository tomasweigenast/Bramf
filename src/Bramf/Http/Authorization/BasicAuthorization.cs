using Bramf.Extensions;
using System.Text;

namespace Bramf.Http.Authorization
{
    /// <summary>
    /// Basic authorization with username and password
    /// </summary>
    public class BasicAuthorization : BaseAuthorization
    {
        /// <summary>
        /// Creates a new Basic authorization header
        /// </summary>
        /// <param name="username">The username to use.</param>
        /// <param name="password">The password to use.</param>
        public BasicAuthorization(string username, string password) : base("Basic", Encoding.GetEncoding("ISO-8859-1").GetBytes($"{username}:{password}").ToBase64()) { }
    }
}
