using System;
using System.Collections.Generic;
using WebService_Lib;
using WebService_Lib.Attributes;

namespace WebService_Test.Securities
{
    [Security]
    public class TestSecurity : ISecurity
    {
        private HashSet<string> tokens = new HashSet<string>();
        public bool Authenticate(string token)
        {
            return tokens.Contains(token);
        }

        public string GenerateToken(string username)
        {
            return username + "-token";
        }

        public void AddToken(string token)
        {
            tokens.Add(token);
        }

        public void RevokeToken(string token)
        {
            tokens.Remove(token);
        }

        public List<string> SecurePaths()
        {
            return new List<string> { "/secured" };
        }

        public bool CheckCredentials(string username, string password)
        {
            return (username == "admin" && password == "admin") ? true : false;
        }
    }
}