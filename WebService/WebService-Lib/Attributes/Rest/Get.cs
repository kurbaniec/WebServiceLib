using System;

namespace WebService_Lib.Attributes.Rest
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Get : Attribute
    {
        private string path;

        public Get(string path)
        {
            this.path = path;
        }
    }
}