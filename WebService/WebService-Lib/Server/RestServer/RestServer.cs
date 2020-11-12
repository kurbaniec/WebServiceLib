using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using WebService_Lib.Server.RestServer.TcpClient;
using WebService_Lib.Server.RestServer.TcpListener;

namespace WebService_Lib.Server.RestServer
{
    public class RestServer
    {
        private ITcpListener server;
        private ConcurrentBag<Task> tasks;
        private bool listening;
        
        public RestServer(ITcpListener server, Mapping mapping, AuthCheck? authCheck)
        {
            this.server = server;
            tasks = new ConcurrentBag<Task>();
            listening = true;
        }

        public void Start()
        {
            // Generate cancellation token
            // See: https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/how-to-cancel-a-task-and-its-children
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            
            // Wait for request and work through them
            // See: https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.tcplistener?view=net-5.0#see-also
            while (listening)
            {
                try
                {
                    var client = server.AcceptTcpClient();
                    var task = Task.Run(() => Process(client, token, this), token);
                    tasks.Add(task);

                }
                catch (SocketException e) { }
            }
        }

        private void Process(ITcpClient client, CancellationToken ct, RestServer callback)
        {
            
        }
        

        public void Stop()
        {
            // TODO clear tasks
        }
    }
}