namespace WebService_Lib
{
    /// <summary>
    /// Represents authentication user details.
    /// </summary>
    public class AuthDetails
    {
        private readonly string username;
        private readonly string token;
        public string Username => username;
        public string Token => token;

        public AuthDetails(string username, string token)
        {
            this.username = username;
            this.token = token;
        }
    }
}