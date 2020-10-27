using System;
using System.Collections.Generic;
using System.Reflection;
using WebService_Lib.Attributes;

namespace WebService_Lib
{
    public class Scanner
    {
        private readonly Assembly programAssembly;
        public Scanner(Assembly programAssembly)
        {
            this.programAssembly = programAssembly;
        }

        public (List<Type>, List<Type>) ScanAssembly()
        {
            var components = new List<Type>();
            var controllers = new List<Type>();

            // Get program Assemblies
            // See: https://stackoverflow.com/a/1315687/12347616
            // And scan for Attribues
            // See: https://stackoverflow.com/a/1226174/12347616
            foreach (Type type in programAssembly.GetTypes())
            {
                var check = Attribute.GetCustomAttribute(type, typeof(Controller));
                if (check != null)
                {
                    components.Add(type);
                    controllers.Add(type);
                }
                else
                {
                    check = Attribute.GetCustomAttribute(type, typeof(Component));
                    if (check != null) components.Add(type);
                }
            }

            return (components, controllers);
        }

    }
}