using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MTCG.Components.DataManagement.DB;
using MTCG.Components.DataManagement.Schemas;
using WebService_Lib;
using WebService_Lib.Attributes;
using WebService_Lib.Server;

namespace MTCG.API.Security
{
    [WebService_Lib.Attributes.Security]
    public class SecurityConfig : ISecurity
    {
        [Autowired] 
        private readonly PostgresDatabase db = null!;

        private readonly SHA512 hasher = new SHA512Managed();
        
        public AuthDetails AuthDetails(string token)
        {
            var username = token.Substring(0, token.Length - 10);
            return new AuthDetails(username, token);
        }
        
        private readonly HashSet<string> tokens = new HashSet<string>();

        public bool Authenticate(string token) => tokens.Contains(token);
        public (bool, string) Register(string username, string password)
        {
            var check = db.GetUser(username);
            if (check != null) return (false, "");
            var hashPassword = GenerateHash(password);
            if (!db.AddUser(username.ToLower() != "admin"
                ? new UserSchema(username, hashPassword, "User")
                : new UserSchema(username, hashPassword, "Admin")))
                return (false, "");
            var token = GenerateToken(username);
            AddToken(token);
            return (true, token);
        }
        public string GenerateToken(string username) => username + "-mtcgToken";
        public void AddToken(string token) => tokens.Add(token);
        public void RevokeToken(string token) => tokens.Remove(token);
        Dictionary<Method, List<string>> ISecurity.SecurePaths() => new Dictionary<Method, List<string>>()
        {
            {Method.Get, new List<string>()
            {
                "/cards", "/deck", "/users", "/stats", "/score", "/tradings", "/admin/battles", "/admin/battle"
            }},
            {Method.Post, new List<string>() {"/packages", "/transactions/packages", "/battles", "/tradings"}},
            {Method.Put, new List<string>() {"/deck", "/users"}},
            {Method.Patch, new List<string>() {}},
            {Method.Delete, new List<string>() {"/tradings"}},
        };

        public bool CheckCredentials(string username, string password)
        {
            var check = db.GetUser(username);
            if (check == null) return false;
            return GenerateHash(password) == check.Password;
        }

        private string GenerateHash(string password)
        {
            // Calculate hash
            // See: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.sha512?view=net-5.0
            // And: https://stackoverflow.com/a/11654825/12347616
            // And: https://stackoverflow.com/a/11654597/12347616
            // And: https://stackoverflow.com/a/14709940/12347616
            string hash = string.Empty;
            var hashBuffer = hasher.ComputeHash(Encoding.ASCII.GetBytes(password));
            foreach (var b in hashBuffer) hash += b.ToString("x2");
            //return Encoding.ASCII.GetString(hashBuffer, 0, hashBuffer.Length);
            return hash;
        }
    }
}