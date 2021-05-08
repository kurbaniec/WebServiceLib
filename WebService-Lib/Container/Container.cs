using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using WebService_Lib.Attributes;
using WebService_Lib.Logging;

namespace WebService_Lib
{
    /// <summary>
    /// Initializes all classes to concrete instance that should be managed
    /// by <c>WebService_Lib</c>. Can also autowire (=set field values) components
    /// with each other.
    /// </summary>
    public class Container : IContainer
    {
        private Dictionary<Type, object> container = new Dictionary<Type, object>();
        public Dictionary<Type, object> GetContainer => container;
        private readonly ILogger logger = WebServiceLogging.CreateLogger<Container>();

        public Container(List<Type> components)
        {
            foreach (var component in components)
            {
                // Instance components
                // See: https://stackoverflow.com/a/755/12347616
                var instance = Activator.CreateInstance(component);
                if (!container.ContainsKey(component))
                    container.Add(component, instance);
                else
                {
                    logger.Log(LogLevel.Warning, 
                        $"Container already contains Component of type {component}");
                    logger.Log(LogLevel.Warning, "Component will be skipped");
                }
                
                // Also associate instance to its (directly) implemented interfaces
                var interfaces = component.DumpInterface();
                foreach (var iInterface in interfaces)
                    if (!container.ContainsKey(iInterface))
                        container.Add(iInterface, instance);
                    else
                    {
                        logger.Log(LogLevel.Warning, 
                            $"Container already contains concrete component for interface of type {iInterface}");
                        logger.Log(LogLevel.Warning, 
                            $"Component of type {component} will not be associated with interface");
                    }
            }

            Autowire(components);
        }

        /// <summary>
        /// Add an object to the container and perform autowiring.
        /// </summary>
        /// <param name="obj">Object to be added</param>
        public void Add(object obj)
        {
            container.Add(obj.GetType(), obj);
            Autowire(new List<Type>(container.Keys));
        }

        /// <summary>
        /// Get object from container that matches given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Found object or null</returns>
        public object? Get(Type type)
        {
            return container[type];
        }

        /// <summary>
        /// Get all objects from container that match list of types.
        /// </summary>
        /// <param name="types"></param>
        /// <returns>List of objects</returns>
        public List<object> GetObjects(List<Type> types)
        {
            var objects = new List<object>();
            foreach (var type in types)
            {
                if (container.ContainsKey(type)) objects.Add(container[type]);
            }

            return objects;
        }

        /// <summary>
        /// Perform autowiring on all components.
        /// Autowiring means that fields with the <c>[Autowired]</c> attribute will
        /// be dynamically set when a matching component is found.
        /// </summary>
        /// <param name="components"></param>
        private void Autowire(List<Type> components)
        {
            foreach (var component in components)
            {
                // In C# there is a distinct difference between fields and props
                // See: https://stackoverflow.com/a/295109/12347616
                // ---
                // Old versions of WebServiceLib autowired just fields, new versions 
                // also supports now properties
                // ---
                // Also BindingFlags need to be set in order to get private fields
                // See: https://stackoverflow.com/a/1040816/12347616
                var fields = component.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                foreach (var field in fields)
                {
                    // Check if Autowiring needs to be performed
                    // See: https://stackoverflow.com/a/2051116/12347616
                    var needsAutowire = Attribute.IsDefined(field, typeof(Autowired));
                    if (needsAutowire)
                    {
                        // Perform it when possible
                        Type fType = field.FieldType;
                        if (container.ContainsKey(fType))
                        {
                            var instance = container[component];
                            var instanceToWire = container[fType];
                            // Set object field using reflection
                            // See: https://stackoverflow.com/a/619788/12347616
                            field.SetValue(instance, instanceToWire);
                        }
                        // Omit error message for auth autowiring
                        // Happens later on
                        else if (fType != typeof(AuthCheck))
                        {
                            logger.Log(LogLevel.Error, "Can not find property of type " + fType.FullName +
                                                       " to autowire field " + field.Name + " in Class " + component.FullName);
                            logger.Log(LogLevel.Error, "Field will not be initialized");
                        }
                    }
                }
                var properties = component.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                foreach (var property in properties)
                {
                    var needsAutowire = Attribute.IsDefined(property, typeof(Autowired));
                    if (needsAutowire)
                    {
                        Type fType = property.PropertyType;
                        if (container.ContainsKey(fType))
                        {
                            var instance = container[component];
                            var instanceToWire = container[fType];
                            property.SetValue(instance, instanceToWire);
                        }
                        else if (fType != typeof(AuthCheck))
                        {
                            logger.Log(LogLevel.Error, "Can not find property of type " + fType.FullName +
                                                       " to autowire field " + property.Name + " in Class " + component.FullName);
                            logger.Log(LogLevel.Error, "Field will not be initialized");
                        }
                    }
                }
            }
        }
    }
}