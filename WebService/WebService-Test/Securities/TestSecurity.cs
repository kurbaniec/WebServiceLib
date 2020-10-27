using System.Collections.Generic;
using WebService_Lib;
using WebService_Lib.Attributes;

namespace WebService_Test.Securities
{
    [Security]
    public class TestSecurity : ISecurity
    {
        public bool Authenticate(string username, string password)
        {
            return (username == "admin" && password == "admin") ? true : false;
        }

        public List<string> SecurePaths()
        {
            return new List<string> { "/secured" };
        }
    }
}