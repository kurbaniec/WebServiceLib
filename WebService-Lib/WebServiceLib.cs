using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using WebService_Lib.Logging;
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
        private readonly ILogger logger = WebServiceLogging.CreateLogger<SimpleWebService>();

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
            logger.Log(LogLevel.Information, $"Webservice has started on Port {Port}");
            server.Start();
        }

        /// <summary>
        /// Stop the SimpleWebService.
        /// </summary>
        public void Stop()
        {
            if (server == null) return;
            logger.Log(LogLevel.Information, "Stopping WebService...");
            server.Stop();
        }
    }
}
