namespace WebService_Lib
{
    /// <summary>
    /// Represents authentication user details.
    /// </summary>
    public class AuthDetails
    {
        private string username;
        private string token;
        public string Username => username;
        public string Token => token;

        public AuthDetails(string username, string token)
        {
            this.username = username;
            this.token = token;
        }
    }
}