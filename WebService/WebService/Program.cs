using System;
using System.Reflection;
using WebService_Lib;

namespace WebService
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new SimpleWebService(Assembly.GetExecutingAssembly());
            service.Start();
        }
    }
}