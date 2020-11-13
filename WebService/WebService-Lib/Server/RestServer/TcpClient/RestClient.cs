using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace WebService_Lib.Server.RestServer.TcpClient
{
    public class RestClient : ITcpClient
    {
        private readonly System.Net.Sockets.TcpClient client;
        
        public RestClient(System.Net.Sockets.TcpClient client)
        {
            this.client = client;
        }

        public RequestContext? ReadRequest(in Mapping mapping)
        {
            // Read request
            // See: https://developer.mozilla.org/en-US/docs/Web/HTTP/Messages
            // And: https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.tcplistener?view=net-5.0#methods
            // And: https://github.com/kienboec/CountStringFromWeb
            
            Method method = Method.Get;
            string? path = null;
            string? version = null;
            Dictionary<string, string> header = new Dictionary<string, string>();
            object? payload = null;
            string? requestParam = null;
            string? pathVariable = null;
            
            StreamReader reader = new StreamReader(client.GetStream());
            string? line;
            var contentLength = 0;
            bool first = true;
            // Read http header information until blank line before payload (when existing)
            while (!string.IsNullOrWhiteSpace(line = reader.ReadLine()))
            {
                if (line == null) continue;
                line = line.Trim();
                if (first)
                {
                    var info = line.Split(' ');
                    method = MethodUtilities.GetMethod(info[0]);
                    path = info[1];
                    version = info[2];
                    // Check path
                    if (path.LastIndexOf('?') != -1)
                    {
                        requestParam = path.Substring(path.LastIndexOf('?') + 1);
                        path = path.Substring(0, path.LastIndexOf('?') - 1);
                    }

                    if (!mapping.Contains((Method) method, path))
                    {
                        pathVariable = path.Substring(path.LastIndexOf('/') + 1);
                        path = path.Substring(0, path.LastIndexOf('/') - 1);
                        // No mapping for this endpoint found
                        // Stop read process and Return null
                        if (!mapping.Contains((Method) method, path))
                        {
                            //reader.Close();
                            return null;
                        }
                    }
                    first = false;
                }
                else
                {
                    var info = line.Split(':');
                    header.Add(info[0], info[1]);
                    if (info[0] == "Content-Length")
                    {
                        contentLength = int.Parse(info[1]);
                    }
                }
            }
            
            if (path == null || version == null)
            {
                //reader.Close();
                return null;
            }
            
            
            // Read http body (when existing)
            if (contentLength > 0 && header.ContainsKey("Content-Type"))
            {
                StringBuilder data = new StringBuilder(200);
                char[] buffer = new char[1024];
                int bytesReadTotal = 0;
                while (bytesReadTotal < contentLength)
                {
                    var bytesRead = reader.Read(buffer, 0, 1024);
                    bytesReadTotal += bytesRead;
                    if (bytesRead == 0) break;
                    data.Append(buffer, 0, bytesRead);
                }
                payload = data.ToString();
                RequestContext.ParsePayload(ref payload, header["Content-Type"]);
            }

            //reader.Close();
            return new RequestContext(method, path, version, header, payload, pathVariable, requestParam);
        }

        public void SendResponse(in Response response)
        {
            
        }


        public void Dispose()
        {
            client.Close();
        }
    }
}