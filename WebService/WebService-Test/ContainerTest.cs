using System;
using System.Collections.Generic;
using NUnit.Framework;
using WebService_Lib;
using WebService_Test.Components;
using WebService_Test.Controllers;

namespace WebService_Test
{
    public class ContainerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, TestCase(TestName = "Count instances in Container", Description =
             "Counts instances initialized by the Container from the component list")]
        public void CountInstances()
        {
            var logger = typeof(TestLogger);
            var controller = typeof(TestController);
            var components = new List<Type> { logger, controller };

            var container = new Container(components).GetContainer;

            Assert.AreEqual(2, container.Count);
        }

        [Test, TestCase(TestName = "Check instance types", Description =
             "Check if the instances initialized by the Container match the given components")]
        public void CheckTypes()
        {
            var logger = typeof(TestLogger);
            var controller = typeof(TestController);
            var components = new List<Type> { logger, controller };

            var container = new Container(components).GetContainer;

            Assert.IsTrue(container.ContainsKey(logger));
            Assert.AreEqual(logger, container[logger].GetType());
            Assert.IsTrue(container.ContainsKey(controller));
            Assert.AreEqual(controller, container[controller].GetType());
        }

        [Test, TestCase(TestName = "Test Autowiring", Description =
             "Check if field 'logger' in 'TestController' is automatically set to the instance of 'TestLogger'")]
        public void TestAutowiring()
        {
            var logger = typeof(TestLogger);
            var controller = typeof(TestController);
            var components = new List<Type> { logger, controller };

            var container = new Container(components).GetContainer;

            var controllerInstance = (TestController)container[controller];
            Assert.NotNull(controllerInstance.Logger);
        }
    }
}