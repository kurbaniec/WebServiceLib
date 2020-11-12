using System.IO;

namespace WebService_Lib.Server.RestServer.TcpClient
{
    public class RestClient : ITcpClient
    {
        private readonly System.Net.Sockets.TcpClient client;
        
        public RestClient(System.Net.Sockets.TcpClient client)
        {
            this.client = client;
        }

        public Stream GetStream()
        {
            return client.GetStream();
        }
        
        public void Dispose()
        {
            client.Close();
        }
    }
}