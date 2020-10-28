using System;
using System.Collections.Generic;
using NUnit.Framework;
using WebService_Lib;
using WebService_Test.Components;
using WebService_Test.Controllers;
using WebService_Test.Securities;

namespace WebService_Test
{
    public class AuthCheckTest
    {
        private ISecurity security;

        [OneTimeSetUp]
        public void Setup()
        {
            security = new TestSecurity();
        }

        [Test, TestCase(TestName = "Check secured path", Description =
             "Check if secured path, defined in 'TestSecurity', is really secured")]
        public void CheckSecuredPath()
        {
            var authCheck = new AuthCheck(security);

            var isSecured = authCheck.IsSecured("/secured");

            Assert.IsTrue(isSecured);
        }

        [Test, TestCase(TestName = "Check not secured path", Description =
             "Check if not secured path is correctly interpreted")]
        public void CheckNotSecuredPath()
        {
            var authCheck = new AuthCheck(security);

            var isSecured = authCheck.IsSecured("/notsecured");

            Assert.IsFalse(isSecured);
        }

        [Test, TestCase(TestName = "Check authentication with correct credentials", Description =
             "Check if authentication, defined in 'TestSecurity', is correctly handled with correct credentials")]
        public void CheckAuthenticationCorrectCredentials()
        {
            var authCheck = new AuthCheck(security);

            var isAuthenticated = authCheck.Authenticate("admin", "admin");

            Assert.IsTrue(isAuthenticated);
        }

        [Test, TestCase(TestName = "Check authentication with incorrect credentials", Description =
             "Check if authentication, defined in 'TestSecurity', is correctly handled with incorrect credentials")]
        public void CheckAuthenticationIncorrectCredentials()
        {
            var authCheck = new AuthCheck(security);

            var isAuthenticated = authCheck.Authenticate("admin", "1234");

            Assert.IsFalse(isAuthenticated);
        }
    }
}