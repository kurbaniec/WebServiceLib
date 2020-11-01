using System;

namespace WebService_Lib.Attributes.Rest
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Delete : Attribute, IMethod
    {
        public string Path { get; }
        public bool HasPathParam { get; }

        public Delete(string path, bool hasPathParam = false)
        {
            this.Path = path;
            this.HasPathParam = hasPathParam;
        }
    }
}