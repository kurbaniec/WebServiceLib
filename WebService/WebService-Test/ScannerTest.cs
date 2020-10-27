using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using WebService_Lib;
using WebService_Test.Components;
using WebService_Test.Controllers;
using WebService_Test.Dummy;
using WebService_Test.Securities;

namespace WebService_Test
{
    public class ScannerTests
    {
        private List<Type> assembly;

        [OneTimeSetUp]
        public void Setup()
        {
            Type logger = typeof(TestLogger);
            Type controller = typeof(TestController);
            Type dummy = typeof(DummyClass);
            Type wrongSecurity = typeof(WrongSecurity);
            Type security = typeof(TestSecurity);
            assembly = new List<Type> { logger, controller, dummy, wrongSecurity, security };
        }

        [Test, TestCase(TestName = "Count components", Description =
        "Count components (classes annotated by 'Component', 'Controller' or 'Security') from provided Assembly")]
        public void CountComponents()
        {
            var scanner = new Scanner(assembly);

            var result = scanner.ScanAssembly();

            Assert.AreEqual(3, result.Item1.Count);
        }

        [Test, TestCase(TestName = "Check components types", Description =
        "Check if components have expected types")]
        public void CheckComponentsTypes()
        {
            var scanner = new Scanner(assembly);

            var result = scanner.ScanAssembly();

            Assert.Contains(typeof(TestLogger), result.Item1);
            Assert.Contains(typeof(TestController), result.Item1);
            Assert.Contains(typeof(TestSecurity), result.Item1);
        }

        [Test, TestCase(TestName = "Count controllers", Description =
        "Count controllers (classes annotated by 'Controller') from provided Assembly")]
        public void CountControllers()
        {
            var scanner = new Scanner(assembly);

            var result = scanner.ScanAssembly();

            Assert.AreEqual(1, result.Item2.Count);
        }

        [Test, TestCase(TestName = "Check controller types", Description =
        "Check if controllers have expected types")]
        public void CheckControllerTypes()
        {
            var scanner = new Scanner(assembly);

            var result = scanner.ScanAssembly();

            Assert.Contains(typeof(TestController), result.Item1);
        }

        [Test, TestCase(TestName = "Security exists", Description =
        "Check if there is a security config (classes annotated by 'Security' implementing 'ISecurity') from provided Assembly")]
        public void SecurityExists()
        {
            var scanner = new Scanner(assembly);

            var result = scanner.ScanAssembly();

            Assert.NotNull(result.Item3);
        }

        [Test, TestCase(TestName = "Check security type", Description =
        "Check if security config has expected type")]
        public void CheckSecurityType()
        {
            var scanner = new Scanner(assembly);

            var result = scanner.ScanAssembly();

            Assert.AreEqual(typeof(TestSecurity), result.Item3);
        }

        [Test, TestCase(TestName = "Count and check components, controllers and security config with extracted execution assembly", Description =
        "Count and check components, controllers and security config from extracted execution Assembly")]
        public void UseRealAssembly()
        {
            var executionAssembly = Assembly.GetExecutingAssembly().GetTypes().ToList();
            var scanner = new Scanner(executionAssembly);

            var result = scanner.ScanAssembly();

            Assert.AreEqual(3, result.Item1.Count);
            Assert.AreEqual(1, result.Item2.Count);
            Assert.NotNull(result.Item3);
            Assert.Contains(typeof(TestLogger), result.Item1);
            Assert.Contains(typeof(TestController), result.Item1);
            Assert.Contains(typeof(TestController), result.Item2);
            Assert.AreEqual(typeof(TestSecurity), result.Item3);
        }
    }
}