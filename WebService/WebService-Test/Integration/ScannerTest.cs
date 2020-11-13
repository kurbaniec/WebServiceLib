using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using WebService_Lib;
using WebService_Lib.Attributes;

namespace WebService_Test.Integration
{
    public class ScannerTest
    {
        // Dummy classes to test functionality
        [Component]
        class DummyComponent {}

        [Controller]
        class DummyController {}
        
        [Security]
        class DummySecurity : ISecurity
        {
            public bool Authenticate(string token) => throw new System.NotImplementedException();
            public string GenerateToken(string username) => throw new System.NotImplementedException();
            public void AddToken(string token) => throw new System.NotImplementedException();
            public void RevokeToken(string token) => throw new System.NotImplementedException();
            public AuthDetails AuthDetails(string token) => throw new System.NotImplementedException();
            public List<string> SecurePaths() => throw new System.NotImplementedException();
            public bool CheckCredentials(string username, string password) => throw new System.NotImplementedException();
        }
        
        [Test, TestCase(TestName = "Check components, controllers and security config with extracted execution assembly", 
             Description = "Check components, controllers and security config from extracted execution Assembly")]
        public void UseRealAssembly()
        {
            // Use assembly of this class
            var executionAssembly = Assembly.GetExecutingAssembly().GetTypes().ToList();
            var scanner = new Scanner(executionAssembly);

            var result = scanner.ScanAssembly();
            
            Assert.NotNull(result.Item3);
            Assert.Contains(typeof(DummyComponent), result.Item1);
            Assert.Contains(typeof(DummyController), result.Item1);
            Assert.Contains(typeof(DummySecurity), result.Item1);
            Assert.Contains(typeof(DummyController), result.Item2);
            Assert.AreEqual(typeof(DummySecurity), result.Item3);
        }
    }
}