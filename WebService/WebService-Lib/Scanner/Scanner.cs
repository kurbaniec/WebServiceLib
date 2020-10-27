﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebService_Lib.Attributes;

namespace WebService_Lib
{
    public class Scanner
    {
        private readonly List<Type> programAssembly;
        public Scanner(List<Type> programAssembly)
        {
            this.programAssembly = programAssembly;
        }

        public (List<Type>, List<Type>, Type?) ScanAssembly()
        {
            var components = new List<Type>();
            var controllers = new List<Type>();
            var security = (Type?)null;

            // Get program Assemblies
            // See: https://stackoverflow.com/a/1315687/12347616
            // And scan for Attribues
            // See: https://stackoverflow.com/a/1226174/12347616
            foreach (Type type in programAssembly)
            {
                var check = Attribute.GetCustomAttribute(type, typeof(Component));
                if (check != null) components.Add(type);
                else
                {
                    check = Attribute.GetCustomAttribute(type, typeof(Controller));
                    if (check != null)
                    {
                        components.Add(type);
                        controllers.Add(type);
                    }
                    else
                    {
                        check = Attribute.GetCustomAttribute(type, typeof(Security));
                        // This check does not work because of assembly mismatch
                        // See: https://stackoverflow.com/a/4963190/12347616
                        // var interfaces = type.GetInterfaces();
                        // var containsInterface = type.GetInterfaces().Any(i =>
                        //    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISecurity));

                        // This check is a workaround for the problem
                        // See: https://stackoverflow.com/a/345464/12347616
                        var containsInterface = false;
                        foreach (var itype in type.GetInterfaces())
                        {
                            if (itype.Module.FullyQualifiedName == typeof(ISecurity).Module.FullyQualifiedName)
                            {
                                containsInterface = true;
                                break;
                            }
                        }
                        if (check != null && !containsInterface)
                        {
                            Console.Error.WriteLine("Found class annotated with [Security] that " +
                                                    "does not implement 'ISecurity'\nConfiguration will not be used");
                            continue;
                        }
                        if (check != null && containsInterface)
                        {
                            components.Add(type);
                            security = type;
                        }
                    }
                }
            }

            return (components, controllers, security);
        }

    }
}