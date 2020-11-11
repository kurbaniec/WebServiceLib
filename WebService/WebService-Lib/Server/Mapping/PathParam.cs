using System;
using System.Collections.Generic;

namespace WebService_Lib.Server
{
    /// <summary>
    /// Used to extract values from URI Path like:
    /// <c>/foos/{id}</c>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PathVariable<T> where T : struct, IComparable
    {
        // Class should only allow string or numbers
        // See: https://stackoverflow.com/a/3329610/12347616
        public T? Value { get; }
        public PathVariable(string? value)
        {
            if (value == null) Value = null;
            else
            {
                // Change type dynamically
                // See: https://stackoverflow.com/a/4010198/12347616
                var cast = Convert.ChangeType(value, typeof(T));
                if (cast != null) Value = (T)cast;
                else Value = null;
            }
        }
    }

    /// <summary>
    /// Used to extract values from query string like:
    /// <c>/foos/?id=abc</c>
    /// </summary>
    public class RequestParam
    {
        public Dictionary<string, string> Value { get; }

        public RequestParam(string? value)
        {
            Value = new Dictionary<string, string>();
            if (value != null)
            {
                var entries = value.Split('&');
                foreach (var entry in entries)
                {
                    var tmp = entry.Split('=');
                    if (tmp.Length == 2)
                    {
                        var key = tmp[0];
                        var val = tmp[1];
                        Value.Add(key, val);
                    }
                }
            }
            
        }
    }
}