using System;
using WebService_Lib;

namespace MTCG
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new SimpleWebService();
            service.Start();
        }
    }
}
