using System;
using System.Collections.Generic;
using System.Reflection;
using WebService_Lib.Attributes.Rest;
using WebService_Lib.Server.Exceptions;

namespace WebService_Lib.Server
{
    /// <summary>
    /// Maps the paths of the service endpoint to their corresponding methods.
    /// Is also responsible for their invocation.
    /// </summary>
    public class Mapping
    {
        private Dictionary<Method, Dictionary<string, MethodCaller>> mappings;
        public Dictionary<Method, Dictionary<string, MethodCaller>> GetMappings => mappings;

        public Mapping(List<object> controllers)
        {
            mappings = new Dictionary<Method, Dictionary<string, MethodCaller>>();
            mappings.Add(Method.Get, new Dictionary<string, MethodCaller>());
            mappings.Add(Method.Post, new Dictionary<string, MethodCaller>());
            mappings.Add(Method.Put, new Dictionary<string, MethodCaller>());
            mappings.Add(Method.Delete, new Dictionary<string, MethodCaller>());
            mappings.Add(Method.Patch, new Dictionary<string, MethodCaller>());

            foreach (var controller in controllers)
            {
                var cType = controller.GetType();
                var methods = cType.GetMethods();
                foreach (var method in methods)
                {
                    AddMapping(method, controller);
                }
            }
        }

        /// <summary>
        /// Check if a method contains a service endpoint definition.
        /// If so, create a wrapper for it and save it.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="controller"></param>
        private void AddMapping(MethodInfo method, object controller)
        {
            var restMethods = new List<Type> { typeof(Get), typeof(Post), typeof(Put), typeof(Delete), typeof(Patch) };
            foreach (var restMethod in restMethods)
            {
                // Get custom attribute 
                // See: https://stackoverflow.com/a/6538438/12347616
                var customAttribute = method.GetCustomAttribute(restMethod);
                if (customAttribute is IMethod attribute)
                {
                    var parameters = method.GetParameters();
                    if (parameters.Length > 2)
                    {
                        Console.Error.WriteLine("Err: Wrong endpoint method syntax - more than two parameters");
                        Console.Error.WriteLine("Err: Please correct method " + method.Name + " from Class " +
                                                controller.GetType().FullName + " to restore functionality");
                        break;
                    }

                    var mappingsParam = new List<MappingParams>();
                    var error = false;
                    foreach (var parameter in parameters)
                    {
                        if (parameter.ParameterType == typeof(AuthDetails))
                        {
                            mappingsParam.Add(MappingParams.Auth);
                        }
                        else if (parameter.ParameterType == typeof(string))
                        {
                            mappingsParam.Add(MappingParams.Text);
                        }
                        else if (parameter.ParameterType == typeof(Dictionary<string, object>))
                        {
                            mappingsParam.Add(MappingParams.Json);
                        }
                        else
                        {
                            Console.Error.WriteLine("Err: Wrong endpoint method syntax - usage of not supported type " +
                                                    mappingsParam.GetType());
                            Console.Error.WriteLine("Err: Please correct method " + method.Name + " from Class " +
                                                    controller.GetType().FullName + " to restore functionality");
                            error = true;
                        }
                    }

                    if (error) break;

                    var path = attribute.Path;
                    var methodCaller = new MethodCaller(method, controller, mappingsParam);
                    mappings[MethodUtilities.GetMethod(restMethod)].Add(path, methodCaller);
                    break;
                }
            }
        }

        /// <summary>
        /// Invokes the method for the corresponding method and path.
        /// </summary>
        /// <param name="authDetails"></param>
        /// <param name="payload"></param>
        /// <exception>
        /// Throws an exception when invalid parameters are given or when the given endpoint
        /// does not exist.
        /// </exception>
        /// <returns>Response as a Response object</returns>
        public Response Invoke(Method method, string path, AuthDetails? authDetails, object? payload)
        {
            var methodCaller = mappings[method][path];
            if (methodCaller != null) return methodCaller.Invoke(authDetails, payload);
            throw new EndpointNotFoundException();
        }


        /// <summary>
        /// Wrapper that is used to call the methods that define service endpoints.
        /// </summary>
        public class MethodCaller
        {
            private MethodInfo method;
            private object instance;
            private List<MappingParams> paramInfo;

            public MethodCaller(MethodInfo method, object instance, List<MappingParams> paramInfo)
            {
                this.method = method;
                this.instance = instance;
                this.paramInfo = paramInfo;
            }

            /// <summary>
            /// Invokes the method wrapped in the <c>MethodCaller</c>.
            /// </summary>
            /// <param name="authDetails"></param>
            /// <param name="payload"></param>
            /// <exception>Throws an exception when invalid parameters are given</exception>
            /// <returns>Response as a Response object</returns>
            public Response Invoke(AuthDetails? authDetails, object? payload)
            {
                var parameters = new List<object>();
                foreach (var param in paramInfo)
                {
                    switch (param)
                    {
                        case MappingParams.Auth:
                            parameters.Add(authDetails);
                            break;
                        case MappingParams.Json:
                            if (payload is Dictionary<string, object>) parameters.Add(payload);
                            else throw new InvokeInvalidParamException();
                            break;
                        case MappingParams.Text:
                            if (payload is string) parameters.Add(payload);
                            else throw new InvokeInvalidParamException();
                            break;
                    }
                }
                // Invoke method
                // See: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.methodbase.invoke?view=netcore-3.1
                return (Response)method.Invoke(instance, parameters.ToArray());
            }

        }

        /// <summary>
        /// Enum is used to declare valid parameters in <c>MethodCaller</c>.
        /// </summary>
        public enum MappingParams
        {
            Auth,
            Text,
            Json
        }
    }
}