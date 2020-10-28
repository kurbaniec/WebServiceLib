using System.Collections.Generic;
using System.Reflection;

namespace WebService_Lib.Server
{
    public class Mapping
    {
        private Dictionary<Method, Dictionary<string, MethodCaller>> mappings;

        public Mapping(List<object> contoller)
        {

        }
    }



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
                        else
                        {
                            // TODO throw error
                        }

                        break;
                    case MappingParams.Text:
                        if (payload is string) parameters.Add(payload);
                        else
                        {
                            // TODO throw error
                        }

                        break;
                }
            }

            return (Response)method.Invoke(instance, parameters.ToArray());
        }

    }

    public enum MappingParams
    {
        Auth,
        Text,
        Json
    }
}