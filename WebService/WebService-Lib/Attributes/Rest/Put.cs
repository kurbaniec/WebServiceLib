using System;

namespace WebService_Lib.Attributes.Rest
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Put : Attribute
    {
        private string path;

        public Put(string path)
        {
            this.path = path;
        }
    }
}