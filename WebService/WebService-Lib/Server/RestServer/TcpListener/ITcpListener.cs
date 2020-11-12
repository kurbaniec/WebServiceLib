using WebService_Lib.Server.RestServer.TcpClient;

namespace WebService_Lib.Server.RestServer.TcpListener
{
    public interface ITcpListener
    {
        void Start();

        ITcpClient AcceptTcpClient();

        void Stop();
    }
}