using System.Net;
using WebService_Lib.Server.RestServer.TcpClient;

namespace WebService_Lib.Server.RestServer.TcpListener
{
    public class RestListener : ITcpListener
    {
        private readonly System.Net.Sockets.TcpListener server;
        
        public RestListener(uint port)
        {
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            server = new System.Net.Sockets.TcpListener(localAddr, (int) port);
        }
        
        public void Start()
        {
            server.Start();
        }

        public ITcpClient AcceptTcpClient()
        {
            return new RestClient(server.AcceptTcpClient());
        }

        public void Stop()
        {
            server.Stop();
        }
    }
}