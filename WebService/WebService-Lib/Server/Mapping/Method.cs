using System;
using WebService_Lib.Attributes.Rest;

namespace WebService_Lib.Server
{
    /// <summary>
    /// Enum that lists all possible REST methods.
    /// </summary>
    public enum Method
    {
        Get,
        Post,
        Put,
        Delete,
        Patch,
    }

    /// <summary>
    /// Utility class to work with <c>Method</c> enums.
    /// </summary>
    public static class MethodUtilities
    {
        /// <summary>
        /// Return the corresponding REST method through its name.
        /// </summary>
        /// <param name="method"></param>
        /// <returns>
        /// Corresponding REST Method, defaults to GET in error case.
        /// </returns>
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
                case "patch":
                    parsedMethod = Method.Patch;
                    break;
                default:
                    parsedMethod = Method.Get;
                    break;
            }

            return parsedMethod;
        }

        /// <summary>
        /// Return the corresponding REST method through a given attribute.
        /// </summary>
        /// <param name="method"></param>
        /// <returns>
        /// Corresponding REST Method, defaults to GET in error case.
        /// </returns>
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