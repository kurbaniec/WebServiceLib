using System;
using System.Collections.Generic;
using System.Linq;
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
        private Dictionary<Method, Dictionary<string, (bool, MethodCaller)>> mappings;
        public Dictionary<Method, Dictionary<string, (bool, MethodCaller)>> GetMappings => mappings;

        public Mapping(List<object> controllers)
        {
            mappings = new Dictionary<Method, Dictionary<string, (bool, MethodCaller)>>
            {
                {Method.Get, new Dictionary<string, (bool, MethodCaller)>()},
                {Method.Post, new Dictionary<string, (bool, MethodCaller)>()},
                {Method.Put, new Dictionary<string, (bool, MethodCaller)>()},
                {Method.Delete, new Dictionary<string, (bool, MethodCaller)>()},
                {Method.Patch, new Dictionary<string, (bool, MethodCaller)>()}
            };

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
                    if (parameters.Length > 3)
                    {
                        Console.Error.WriteLine("Err: Wrong endpoint method syntax - more than three parameters");
                        Console.Error.WriteLine("Err: Please correct method " + method.Name + " from Class " +
                                                controller.GetType().FullName + " to restore functionality");
                        break;
                    }

                    var mappingsParam = new List<MappingParams>();
                    var pathParam = (Type?)null;
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
                        // In order to check generic types you must compare the original generic type,
                        // not the concrete one
                        // See: https://stackoverflow.com/a/457708/12347616
                        else if (parameter.ParameterType.IsGenericType && parameter.ParameterType.GetGenericTypeDefinition() == typeof(PathParam<>))
                        {
                            mappingsParam.Add(MappingParams.PathParam);
                            pathParam = parameter.ParameterType.GenericTypeArguments[0];
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
                    var hasPathParam = attribute.HasPathParam;
                    var methodCaller = new MethodCaller(method, controller, mappingsParam, pathParam);
                    mappings[MethodUtilities.GetMethod(restMethod)].Add(path, (hasPathParam, methodCaller));
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
        public Response Invoke(Method method, string path, AuthDetails? authDetails, object? payload, string? pathParam)
        {
            if (mappings[method].ContainsKey(path)) return mappings[method][path].Item2.Invoke(authDetails, payload, pathParam);
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
            private Type? pathParamType;

            public MethodCaller(MethodInfo method, object instance, List<MappingParams> paramInfo, Type? pathParamType)
            {
                this.method = method;
                this.instance = instance;
                this.paramInfo = paramInfo;
                this.pathParamType = pathParamType;
            }

            /// <summary>
            /// Invokes the method wrapped in the <c>MethodCaller</c>.
            /// </summary>
            /// <param name="authDetails"></param>
            /// <param name="payload"></param>
            /// <exception>Throws an exception when invalid parameters are given</exception>
            /// <returns>Response as a Response object</returns>
            public Response Invoke(AuthDetails? authDetails, object? payload, string? pathParam)
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
                        case MappingParams.PathParam:
                            // Make generic path parameter instance
                            // See: https://stackoverflow.com/a/43921901/12347616
                            var pathParamGenericType = typeof(PathParam<>);
                            var constructType = pathParamGenericType.MakeGenericType(pathParamType);
                            var pathParamObj = Activator.CreateInstance(constructType, pathParam);
                            parameters.Add(pathParamObj);
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
            Json,
            PathParam,
        }
    }
}