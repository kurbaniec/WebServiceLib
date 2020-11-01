using System;

namespace WebService_Lib.Server
{
    // Class should only allow string or numbers
    // See: https://stackoverflow.com/a/3329610/12347616
    public class PathParam<T> where T : struct, IComparable
    {
        public T? Value { get; }
        public PathParam(object? value)
        {
            if (value is T cast)
            {
                Value = cast;
            }
            else
            {
                value = null;
            }
        }
    }
}