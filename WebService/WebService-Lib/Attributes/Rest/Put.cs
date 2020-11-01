using System;

namespace WebService_Lib.Attributes.Rest
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Put : Attribute, IMethod
    {
        public string Path { get; }
        public bool HasPathParam { get; }

        public Put(string path, bool hasPathParam = false)
        {
            this.Path = path;
            this.HasPathParam = hasPathParam;
        }
    }
}