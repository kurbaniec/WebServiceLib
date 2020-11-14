using WebService_Lib;

namespace WebService
{
    internal static class Program
    {
        static void Main()
        {
            var service = new SimpleWebService();
            service.Start();
        }
    }
}