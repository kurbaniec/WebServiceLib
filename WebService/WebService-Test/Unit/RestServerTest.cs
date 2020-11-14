using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using WebService_Lib.Server;
using WebService_Lib.Server.RestServer;
using WebService_Lib.Server.RestServer.TcpClient;
using WebService_Lib.Server.RestServer.TcpListener;

namespace WebService_Test.Unit
{
    public class RestServerTest
    {
        [Test, TestCase(TestName = "Count controllers", Description =
             "Count controllers (classes annotated by 'Controller') from provided Assembly")]
        public void Testsimple()
        {
            // Mocking with Moq
            // See: https://github.com/Moq/moq4/wiki/Quickstart
            // And: https://github.com/kienboec/DotnetLightsaberFight/blob/main/DotnetLightsaberFight.Test/CombatTest.cs
            var mapping = new Mock<IMapping>();
            mapping.Setup(m => m.Contains(Method.Get, "/hi"))
                .Returns(true);
            mapping.Setup(m => m.Invoke(Method.Get, "/hi", null, null, null, null))
                .Returns(Response.Status(Status.Ok));
            var client = new Mock<ITcpClient>();
            client.Setup(i => i.ReadRequest(It.IsAny<IMapping>()))
                .Returns(new RequestContext(Method.Get, "/hi", "http/1.1",
                new Dictionary<string, string>(), null, null, null));
            client.Setup(i => i.SendResponse(It.IsAny<Response>()));
            var listener = new Mock<ITcpListener>();
            listener.Setup(l => l.AcceptTcpClient()).Returns(client.Object);
            var restServer = new RestServer(listener.Object, mapping.Object, null);
            
            Task t = Task.Run(() => restServer.Start());
            Thread.Sleep(10);
            restServer.Stop();
            t.Wait();
            
            // Mock has some problems with Verify & multithreading
            // Not working:
            // client.Verify(i => i.SendResponse(It.IsAny<Response>()), Times.AtLeastOnce);
            // See: https://github.com/moq/moq4/issues/91
            // Alternative way: https://www.thecodebuzz.com/logger-expected-invocation-on-the-mock-moq/
            // --
            // Workaround for now, check manually if invocation happened
            var invoked = client.Invocations.Any(i => i.Method.Name == "SendResponse");
            Assert.True(invoked);
        }
    }
}