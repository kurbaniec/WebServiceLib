using System;
using System.Linq;
using System.Reflection;
using WebService_Lib.Server;
using WebService_Lib.Server.RestServer;
using WebService_Lib.Server.RestServer.TcpListener;

namespace WebService_Lib
{
    public class SimpleWebService
    {
        private readonly IScanner scanner;
        private IContainer container = null!;
        private IMapping mapping = null!;
        private AuthCheck? authCheck;
        private RestServer? server;
        private readonly uint port;

        public SimpleWebService(Assembly programAssembly, uint port = 8080)
        {
            // Convert Assembly to List<Type>
            this.scanner = new Scanner(programAssembly.GetTypes().ToList());
            this.port = port;
        }

        public void Start()
        {
            Console.WriteLine("WebService has started...");
            var result = scanner.ScanAssembly();
            container = new Container(result.Item1);
            if (result.Item3 != null)
            {
                authCheck = new AuthCheck((ISecurity)container.GetContainer[result.Item3]);
                container.Add(authCheck);
            }
            mapping = new Mapping(container.GetObjects(result.Item2));
            var listener = new RestListener(port);
            server = new RestServer(listener, mapping, authCheck);
            server.Start();
        }

        public void Stop()
        {
            if (server == null) return;
            Console.WriteLine("Stopping WebService...");
            server.Stop();
        }
    }
}
