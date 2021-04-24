using System;
using System.Linq;
using System.Reflection;
using WebService_Lib.Server;
using WebService_Lib.Server.RestServer;
using WebService_Lib.Server.RestServer.TcpListener;

namespace WebService_Lib
{
    /// <summary>
    /// Core class of <c>WebService_Lib</c>.
    /// Used to start the library with its service.
    /// </summary>
    public class SimpleWebService
    {
        private readonly IScanner scanner;
        private IContainer container = null!;
        private IMapping mapping = null!;
        private AuthCheck? authCheck;
        private RestServer? server;

        public static uint Port = 8080;

        public SimpleWebService(Assembly programAssembly, uint? port = null)
        {
            // Convert Assembly to List<Type>
            this.scanner = new Scanner(programAssembly.GetTypes().ToList());
            if (port is { } portValue) Port = portValue;
        }
        
        public SimpleWebService(uint? port = null)
        {
            // Get Assembly from caller
            Assembly programAssembly = Assembly.GetCallingAssembly();
            this.scanner = new Scanner(programAssembly.GetTypes().ToList());
            if (port is { } portValue) Port = portValue;
        }

        /// <summary>
        /// Start the SimpleWebService.
        /// </summary>
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
            var listener = new RestListener(Port);
            server = new RestServer(listener, mapping, authCheck);
            server.Start();
        }

        /// <summary>
        /// Stop the SimpleWebService.
        /// </summary>
        public void Stop()
        {
            if (server == null) return;
            Console.WriteLine("Stopping WebService...");
            server.Stop();
        }
    }
}
