using System.Collections.Generic;

namespace WebService_Lib
{
    /// <summary>
    /// If you want to secure endpoints with WebService_Lib, make sure
    /// to make a Security class implementing <c>ISecurity</c> annotated
    /// with the <c>Security</c> attribute.
    /// </summary>
    public interface ISecurity
    {
        /// <summary>
        /// Is called on secured endpoints to determine if a request with an
        /// authorization header can access the resource.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>
        /// True, when the username-password combination is valid, else False
        /// </returns>
        public bool Authenticate(string username, string password);

        /// <summary>
        /// Used internally to determine which resources should be secured.
        /// </summary>
        /// <returns>
        /// List of strings that contain the paths to the resources that
        /// should be secured.
        /// </returns>
        public List<string> SecurePaths();
    }
}