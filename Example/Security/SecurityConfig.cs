using System.Collections.Generic;
using WebService_Lib;
using WebService_Lib.Server;

namespace WebService.Security
{
    [WebService_Lib.Attributes.Security]
    public class SecurityConfig : ISecurity
    {
        public AuthDetails AuthDetails(string token)
        {
            var username = token.Substring(0, token.Length - 6);
            return new AuthDetails(token, username);
        }
        private readonly HashSet<string> tokens = new HashSet<string>();
        private readonly Dictionary<string, string> users = new Dictionary<string, string>();
        public bool Authenticate(string token) => tokens.Contains(token);
        public (bool, string) Register(string username, string password) 
        {
            if (users.ContainsKey(username)) return (false, "");
            users[username] = password;
            var token = GenerateToken(username);
            AddToken(token);
            return (true, token);
        }
        public string GenerateToken(string username) => username + "-token";
        public void AddToken(string token) => tokens.Add(token);
        public void RevokeToken(string token) => tokens.Remove(token);
        public Dictionary<Method, List<string>> SecurePaths() => new Dictionary<Method, List<string>>()
        {
            {Method.Delete, new List<string>(){"/secret"}},
            {Method.Get, new List<string>(){"/secret"}},
            {Method.Patch, new List<string>(){"/secret"}},
            {Method.Post, new List<string>(){"/secret"}},
            {Method.Put, new List<string>(){"/secret"}}
        };
        public bool CheckCredentials(string username, string password) 
            => users.ContainsKey(username) && users[username] == password;
    }
}