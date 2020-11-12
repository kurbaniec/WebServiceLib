using System;
using System.IO;

namespace WebService_Lib.Server.RestServer.TcpClient
{
    public interface ITcpClient : IDisposable
    {
        RequestContext? ReadRequest(in Mapping mapping);
    }
}