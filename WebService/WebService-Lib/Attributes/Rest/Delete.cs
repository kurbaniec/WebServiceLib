using System;

namespace WebService_Lib.Attributes.Rest
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Delete : Attribute
    {
        private string path;

        public Delete(string path)
        {
            this.path = path;
        }
    }
}