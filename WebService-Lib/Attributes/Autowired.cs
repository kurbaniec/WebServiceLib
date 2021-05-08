using System;

namespace WebService_Lib.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class Autowired : Attribute
    {

    }
}