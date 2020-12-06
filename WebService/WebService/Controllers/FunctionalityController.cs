using System;
using System.Collections.Generic;
using WebService_Lib;
using WebService_Lib.Attributes;
using WebService_Lib.Attributes.Rest;
using WebService_Lib.Server;

namespace WebService.Controllers
{
    /// <summary>
    /// Controller used to test <c>WebService_Lib</c> functionality.
    /// </summary>
    [Controller]
    public class FunctionalityController
    {
        [Autowired]
        private readonly AuthCheck auth = null!;
        
        [Post("/register")]
        public Response Register(Dictionary<string, object>? payload)
        {
            if (payload == null) return Response.Status(Status.BadRequest);
            if (!payload.ContainsKey("username") || !payload.ContainsKey("password") ||
                !(payload["username"] is string) ||  !(payload["password"]is string))
                return Response.Status(Status.BadRequest);
            var result 
                = auth.Register((payload["username"] as string)!, (payload["password"] as string)!);
            return !result.Item1 ? Response.Status(Status.Conflict) : Response.PlainText(result.Item2, Status.Created);
        }
    }
}