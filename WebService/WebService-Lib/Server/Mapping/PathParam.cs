using System;

namespace WebService_Lib.Server
{
    // Class should only allow string or numbers
    // See: https://stackoverflow.com/a/3329610/12347616
    public class PathParam<T> where T : struct, IComparable
    {
        public T? Value { get; }
        public PathParam(string? value)
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
}