using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WebService_Lib.Server;

namespace WebService_Lib
{
    public class SimpleWebService
    {
        private Scanner scanner;
        private uint port;

        public SimpleWebService(Assembly programAssembly, uint port = 8080)
        {
            // Convert Assembly to List<Type>
            this.scanner = new Scanner(programAssembly.GetTypes().ToList());
            this.port = port;
        }

        public void Start()
        {
            Console.WriteLine("WebService has started...");
            var result = scanner.ScanAssembly();
            var container = new Container(result.Item1);
            var auth = (AuthCheck?)null;
            if (result.Item3 != null)
            {
                auth = new AuthCheck((ISecurity)container.GetContainer[result.Item3]);
                container.Add(auth);
            }

            var mapping = new Mapping(container.GetObjects(result.Item2));
        }
    }
}
