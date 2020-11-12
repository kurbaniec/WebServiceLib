using System;
using WebService_Lib.Attributes.Rest;

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
                case "Get":
                    parsedMethod = Method.Get;
                    break;
                case "post":
                case "Post":
                    parsedMethod = Method.Post;
                    break;
                case "put":
                case "Put":
                    parsedMethod = Method.Put;
                    break;
                case "delete":
                case "Delete":
                    parsedMethod = Method.Delete;
                    break;
                case "patch":
                case "Patch":
                    parsedMethod = Method.Patch;
                    break;
                default:
                    parsedMethod = Method.Get;
                    break;
            }

            return parsedMethod;
        }

        public static Method GetMethod(Type method)
        {
            Method parsedMethod;
            if (method == typeof(Get))
            {
                return Method.Get;
            }
            else if (method == typeof(Post))
            {
                return Method.Post;
            }
            else if (method == typeof(Put))
            {
                return Method.Put;
            }
            else if (method == typeof(Delete))
            {
                return Method.Delete;
            }
            else if (method == typeof(Patch))
            {
                return Method.Patch;
            }
            else if (method == typeof(Post))
            {
                return Method.Post;
            }

            return Method.Get;
        }
    }
}