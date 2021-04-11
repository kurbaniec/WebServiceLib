using System;

namespace WebService_Lib.Attributes
{
    /// <summary>
    /// Used to Mark string`s in WebService_Lib endpoints that are not plaintext requests
    /// but Json, that one does not want to be parsed as Dictionary.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class JsonString : Attribute
    {

    }
}