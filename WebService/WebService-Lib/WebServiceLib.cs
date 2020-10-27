using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace WebService_Lib
{
    public class SimpleWebService
    {
        private Scanner scanner;

        public SimpleWebService(Assembly programAssembly)
        {
            scanner = new Scanner(programAssembly);
        }

        public void Start()
        {
            Console.WriteLine("WebService has started...");
            var result = scanner.ScanAssembly();
        }
    }
}
