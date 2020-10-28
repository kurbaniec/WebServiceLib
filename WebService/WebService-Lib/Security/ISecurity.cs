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
        /// <param name="token"></param>
        /// <returns>True, when the token is valid, else False</returns>
        public bool Authenticate(string token);

        /// <summary>
        /// Generate a token that can be used to access secured resources through
        /// the Authorization header.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public string GenerateToken(string username);

        /// <summary>
        /// Add a token to a persistent or temporary store, so that the
        /// Authenticate method can access it. A added token can be used
        /// to access secured resources through the Authorization header.
        /// </summary>
        /// <param name="token"></param>
        public void AddToken(string token);

        /// <summary>
        /// Revoke token, so that requests with it can not access secured resources
        /// anymore.
        /// </summary>
        /// <param name="token"></param>
        public void RevokeToken(string token);

        /// <summary>
        /// Used internally to determine which resources should be secured.
        /// </summary>
        /// <returns>
        /// List of strings that contain the paths to the resources that
        /// should be secured.
        /// </returns>
        public List<string> SecurePaths();

        /// <summary>
        /// Webservice_Lib's <c>AuthCheck</c> handler uses this method to
        /// Authenticate users with their full credentials.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>
        /// True, when the username-password combination is valid, else False
        /// </returns>
        public bool CheckCredentials(string username, string password);
    }
}