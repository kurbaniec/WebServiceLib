using System;

namespace WebService_Lib.Attributes.Rest
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Get : Attribute, IMethod
    {
        public string Path { get; }
        public bool HasPathParam { get; }

        public Get(string path, bool hasPathParam = false)
        {
            this.Path = path;
            this.HasPathParam = hasPathParam;
        }
    }
}