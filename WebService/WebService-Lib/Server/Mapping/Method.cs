using System;

namespace WebService_Lib.Server
{
    public enum Method
    {
        Get,
        Post,
        Put,
        Delete,
        Patch,
    }

    public static class MethodUtilities
    {
        public static Method GetMethod(string method)
        {
            method = method.ToLower();
            Method parsedMethod;
            switch (method)
            {
                case "get":
                    parsedMethod = Method.Get;
                    break;
                case "post":
                    parsedMethod = Method.Post;
                    break;
                case "put":
                    parsedMethod = Method.Put;
                    break;
                case "delete":
                    parsedMethod = Method.Delete;
                    break;
                case "Patch":
                    parsedMethod = Method.Patch;
                    break;
                default:
                    parsedMethod = Method.Get;
                    break;
            }

            return parsedMethod;
        }
    }
}