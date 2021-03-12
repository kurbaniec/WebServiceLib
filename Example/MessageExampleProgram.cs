using System;
using System.IO;
using WebService_Lib;

namespace WebService
{
    /// <summary>
    /// Simple example program that shows 'WebService_Lib' functionality
    /// through message endpoints.
    /// </summary>
    internal static class MessageExampleProgram
    {
        static void Main()
        {
            // Start 'WebService_Lib' through its 'SimpleWebService'.
            // This will scan the assembly for 'MessageController' and 'MesssageHandler'
            // and automatically instantiate both of them.
            // The controller will provide the endpoints which are then used
            // in the internal REST server.
            
            string runningPath = AppDomain.CurrentDomain.BaseDirectory!;
            // Platform agnostic path
            // See: https://stackoverflow.com/a/38428899/12347616
            string img =
                $"{Path.GetFullPath(Path.Combine(runningPath!, @$"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}"))}res{Path.DirectorySeparatorChar}doge.jpg";
            Console.WriteLine($"{img}");
            
            var service = new SimpleWebService();
            service.Start();
        }
    }
}