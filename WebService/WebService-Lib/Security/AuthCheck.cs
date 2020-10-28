namespace WebService_Lib
{
    public class AuthCheck
    {
        private ISecurity security;

        public AuthCheck(ISecurity security)
        {
            this.security = security;
        }

        public bool IsSecured(string path)
        {
            return this.security.SecurePaths().Contains(path);
        }

        public bool Authenticate(string username, string password)
        {
            return this.security.Authenticate(username, password);
        }
    }
}