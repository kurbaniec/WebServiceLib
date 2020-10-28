using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WebService_Lib
{
    public class SimpleWebService
    {
        private Scanner scanner;

        public SimpleWebService(Assembly programAssembly)
        {
            // Convert Assembly to List<Type>
            scanner = new Scanner(programAssembly.GetTypes().ToList());
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
        }
    }
}
