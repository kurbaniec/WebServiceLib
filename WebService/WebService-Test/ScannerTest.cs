using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using WebService_Lib;
using WebService_Test.Components;
using WebService_Test.Controllers;
using WebService_Test.Dummy;

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
            assembly = new List<Type> { logger, controller, dummy };
        }

        [Test, TestCase(TestName = "Count components", Description =
        "Count components (classes annotated by 'Component' or 'Controller') from provided Assembly")]
        public void CountComponents()
        {
            var scanner = new Scanner(assembly);

            var result = scanner.ScanAssembly();

            Assert.AreEqual(2, result.Item1.Count);
        }

        [Test, TestCase(TestName = "Check components types", Description =
        "Check if components have expected types")]
        public void CheckComponentsTypes()
        {
            var scanner = new Scanner(assembly);

            var result = scanner.ScanAssembly();

            Assert.Contains(typeof(TestLogger), result.Item1);
            Assert.Contains(typeof(TestController), result.Item1);
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

        [Test, TestCase(TestName = "Count and check components and controllers with extracted execution assembly", Description =
        "Count and check components and controllers from extracted execution Assembly")]
        public void UseRealAssembly()
        {
            var executionAssembly = Assembly.GetExecutingAssembly().GetTypes().ToList();
            var scanner = new Scanner(executionAssembly);

            var result = scanner.ScanAssembly();

            Assert.AreEqual(2, result.Item1.Count);
            Assert.AreEqual(1, result.Item2.Count);
            Assert.Contains(typeof(TestLogger), result.Item1);
            Assert.Contains(typeof(TestController), result.Item1);
            Assert.Contains(typeof(TestController), result.Item2);
        }
    }
}