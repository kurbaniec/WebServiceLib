using System;

namespace WebService_Lib.Attributes.Rest
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Post : Attribute
    {
        private string path;

        public Post(string path)
        {
            this.path = path;
        }
    }
}