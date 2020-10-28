using System;

namespace WebService_Lib.Attributes.Rest
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Patch : Attribute
    {
        private string path;

        public Patch(string path)
        {
            this.path = path;
        }
    }
}