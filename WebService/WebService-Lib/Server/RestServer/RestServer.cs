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
        private readonly ITcpListener server;
        private readonly Mapping mapping;
        private readonly AuthCheck? authCheck;
        private ConcurrentBag<Task> tasks;
        private bool listening;

        public RestServer(ITcpListener server, Mapping mapping, AuthCheck? authCheck)
        {
            this.server = server;
            this.mapping = mapping;
            this.authCheck = authCheck;
            tasks = new ConcurrentBag<Task>();
            listening = true;
        }

        public void Start()
        {
            // Generate cancellation token
            // See: https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/how-to-cancel-a-task-and-its-children
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            server.Start();
            
            // Wait for request and work through them
            // See: https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.tcplistener?view=net-5.0#see-also
            while (listening)
            {
                try
                {
                    var client = server.AcceptTcpClient();
                    var task = Task.Run(() => Process(client, token), token);
                    tasks.Add(task);

                }
                catch (SocketException e) { }
            }
        }

        private void Process(ITcpClient client, CancellationToken ct)
        {
            Response response;
            AuthDetails? auth = null;
            
            // Read request
            var request = client.ReadRequest(mapping);
            // Check if no corresponding endpoint was found (ReadRequest returns null)
            if (request == null)
            {
                // Return 'Not Found' response
                response = Response.Status(Status.NotFound);
            }
            else
            {
                // Check if path is secured and authenticate if so
                if (authCheck != null && authCheck.IsSecured(request.Path))
                {
                    // Check if Authorization header was send
                    if (request.Header.ContainsKey("Authorization"))
                    {
                        // If so, check credentials
                        var line = request.Header["Authorization"].Split(' ');
                        string type = line[0], token = line[1];
                        if (line.Length == 2)
                        {
                            if (authCheck.Authenticate(token))
                            {
                                auth = authCheck.AuthDetails(token);
                                response = mapping.Invoke(request.Method, request.Path, auth, request.Payload,
                                    request.PathVariable, request.RequestParam);
                            }
                            else response = Response.Status(Status.Forbidden);
                        }
                        // It not, return 'Forbidden' response
                        else response = Response.Status(Status.Forbidden);
                    }
                    // If path is secured but no Authorization header was send
                    // return 'Unauthorized' response
                    else response = Response.Status(Status.Unauthorized);
                }
                // If endpoint is not secured just invoke the endpoint
                else response = mapping.Invoke(request.Method, request.Path, auth, request.Payload,
                        request.PathVariable, request.RequestParam);
            }
            // Send response
            client.SendResponse(response);
        }

        public void Stop()
        {
            // TODO clear tasks
        }
    }
}