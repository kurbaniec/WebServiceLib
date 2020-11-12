using System;
using System.Collections.Generic;
using NUnit.Framework;
using WebService_Lib;
using WebService_Lib.Server;
using WebService_Lib.Server.Exceptions;
using WebService_Test.Components;
using WebService_Test.Controllers;
using WebService_Test.Securities;

namespace WebService_Test
{
    public class MappingTest
    {
        private TestController controller;

        [SetUp]
        public void Setup()
        {
            controller = new TestController();
        }


        [Test, TestCase(TestName = "Count endpoints", Description =
             "Check if number of endpoints, defined in 'TestSecurity', matches mapping count")]
        public void CountEndpoints()
        {
            var mapping = new Mapping(new List<object> { this.controller });

            Assert.AreEqual(2, mapping.GetMappings[Method.Get].Count);
            Assert.AreEqual(1, mapping.GetMappings[Method.Post].Count);
            Assert.AreEqual(1, mapping.GetMappings[Method.Put].Count);
            Assert.AreEqual(1, mapping.GetMappings[Method.Patch].Count);
            Assert.AreEqual(2, mapping.GetMappings[Method.Delete].Count);
        }

        [Test, TestCase(TestName = "Check endpoint paths", Description =
             "Check if paths, defined in 'TestSecurity', are found in the mappings")]
        public void CheckPaths()
        {
            var mapping = new Mapping(new List<object> { this.controller });

            Assert.IsTrue(mapping.GetMappings[Method.Get].ContainsKey("/hi"));
            Assert.IsTrue(mapping.GetMappings[Method.Get].ContainsKey("/secured"));
            Assert.IsTrue(mapping.GetMappings[Method.Post].ContainsKey("/secured2"));
            Assert.IsTrue(mapping.GetMappings[Method.Put].ContainsKey("/insert"));
            Assert.IsTrue(mapping.GetMappings[Method.Patch].ContainsKey("/patch"));
            Assert.IsTrue(mapping.GetMappings[Method.Delete].ContainsKey("/delete"));
        }

        [Test, TestCase(TestName = "Invoke endpoint with no parameters", Description =
             "Invoke GET endpoint '/hi', defined in 'TestSecurity', with no parameters")]
        public void InvokeNoParams()
        {
            var mapping = new Mapping(new List<object> { this.controller });
            var response = mapping.Invoke(Method.Get, "/hi", null, null, null, null);

            Assert.NotNull(response);
        }

        [Test, TestCase(TestName = "Invoke endpoint with one parameter (AuthDetails)", Description =
             "Invoke GET endpoint '/secured', defined in 'TestSecurity', with 'AuthDetails' parameter")]
        public void InvokeAuthDetailsParam()
        {
            var authDetails = new AuthDetails("test", "test");

            var mapping = new Mapping(new List<object> { this.controller });
            var response = mapping.Invoke(Method.Get, "/secured", authDetails, null, null, null);

            Assert.NotNull(response);
        }

        [Test, TestCase(TestName = "Invoke endpoint with two parameters (AuthDetails, string)", Description =
             "Invoke POST endpoint '/secured2', defined in 'TestSecurity', with 'AuthDetails' and string plaintext parameter")]
        public void InvokeAuthDetailsAndPlainTextParam()
        {
            var authDetails = new AuthDetails("test", "test");
            var plainText = "test";

            var mapping = new Mapping(new List<object> { this.controller });
            var response = mapping.Invoke(Method.Post, "/secured2", authDetails, plainText, null, null);

            Assert.NotNull(response);
        }

        [Test, TestCase(TestName = "Invoke endpoint with one parameter (Dictionary)", Description =
             "Invoke PUT endpoint '/insert', defined in 'TestSecurity', with Dictionary json parameter")]
        public void InvokeJsonParam()
        {
            var json = new Dictionary<string, object> { ["test"] = 1 };

            var mapping = new Mapping(new List<object> { this.controller });
            var response = mapping.Invoke(Method.Put, "/insert", null, json, null, null);

            Assert.NotNull(response);
        }

        [Test, TestCase(TestName = "Invoke endpoint with two parameters (string, AuthDetails)", Description =
             "Invoke PATCH endpoint '/patch', defined in 'TestSecurity', with string plaintext and 'AuthDetails' parameter")]
        public void InvokePlainTextAndAuthDetailsParam()
        {
            var authDetails = new AuthDetails("test", "test");
            var plainText = "test";

            var mapping = new Mapping(new List<object> { this.controller });
            var response = mapping.Invoke(Method.Patch, "/patch", authDetails, plainText, null, null);

            Assert.NotNull(response);
        }

        [Test, TestCase(TestName = "Invoke endpoint with one parameter (PathVariable)", Description =
             "Invoke DELETE endpoint '/delete', defined in 'TestSecurity', with PathVariable<int> parameter")]
        public void InvokePathVariable()
        {
            var pathVariable = "1";

            var mapping = new Mapping(new List<object> { this.controller });
            var response = mapping.Invoke(Method.Delete, "/delete", null, null, pathVariable, null);

            Assert.NotNull(response);
        }
        
        [Test, TestCase(TestName = "Invoke endpoint with one parameter (RequestParam)", Description =
             "Invoke DELETE endpoint '/delete2', defined in 'TestSecurity', with RequestParam parameter")]
        public void InvokeRequestParam()
        {
            var requestParam = "id=1";

            var mapping = new Mapping(new List<object> { this.controller });
            var response = mapping.Invoke(Method.Delete, "/delete", null, null, null, requestParam);

            Assert.NotNull(response);
        }

        [Test, TestCase(TestName = "Make invalid invoke", Description =
             "Check if exception is thrown when an illegal parameter is used")]
        public void ExceptionThroughIllegalParam()
        {
            var authDetails = new AuthDetails("test", "test");
            var illegalParam = 42L;

            var mapping = new Mapping(new List<object> { this.controller });

            Assert.Throws<InvokeInvalidParamException>(() => mapping.Invoke(Method.Patch, "/patch", authDetails, illegalParam, null, null));
        }

    }
}