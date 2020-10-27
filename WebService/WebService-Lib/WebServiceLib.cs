using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace WebService_Lib
{
    public class SimpleWebService
    {
        private Assembly programAssembly;

        public SimpleWebService(Assembly programAssembly)
        {
            this.programAssembly = programAssembly;
        }

        public void Start()
        {
            // Get program Assemblies
            // See: https://stackoverflow.com/a/1315687/12347616
            Console.WriteLine("WebService has started...");
            foreach (Type type in programAssembly.GetTypes())
            {
                Console.WriteLine(type.FullName);
            }
        }
    }
}
