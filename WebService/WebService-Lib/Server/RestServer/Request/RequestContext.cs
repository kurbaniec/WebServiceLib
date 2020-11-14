using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebService_Lib.Server.RestServer
{
    /// <summary>
    /// Summarises a http request.
    /// See: https://developer.mozilla.org/en-US/docs/Web/HTTP/Messages
    /// </summary>
    public class RequestContext
    {
        public Method Method;
        public string Path;
        public string Version;
        public Dictionary<string, string> Header;
        public object? Payload;
        public string? PathVariable;
        public string? RequestParam;
        

        public RequestContext(
            Method method, string path, string version, Dictionary<string, string> header,
            object? payload, string? pathVariable, string? requestParam
        )
        {
            Method = method;
            Path = path;
            Version = version;
            Header = header;
            Payload = payload;
            PathVariable = pathVariable;
            RequestParam = requestParam;
        }

        /// <summary>
        /// Parse supported payloads to corresponding forms used in <c>WebService_Lib</c>.
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="contentType"></param>
        public static void ParsePayload(ref object? payload, string contentType)
        {
            switch (contentType)
            {
                case "text/plain":
                    if (payload == null || payload.GetType() != typeof(string))
                    {
                        payload = "";
                    }
                    break;
                case "application/json":
                    if (payload == null || payload.GetType() != typeof(string))
                    {
                        payload = "{}";
                    }
                    try
                    {
                        payload = JsonConvert.DeserializeObject<Dictionary<string, object>>((string) payload);
                    }
                    catch (Exception)
                    {
                        payload = null;
                    }
                    break;
                default:
                    payload = null;
                    break;
            }
        }
    }
}