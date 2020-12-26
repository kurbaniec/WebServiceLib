using System.Collections.Generic;
using WebService_Lib;
using WebService_Lib.Attributes;
using WebService_Lib.Attributes.Rest;
using WebService_Lib.Server;

namespace MTCG.API.Controllers
{
    [Controller]
    public class UserConroller
    {
        [Autowired]
        private readonly AuthCheck auth = null!;
        
        [Post("/users")]
        public Response Register(Dictionary<string, object>? payload)
        {
            if (payload == null) return Response.Status(Status.BadRequest);
            if (!payload.ContainsKey("Username") || !payload.ContainsKey("Password") ||
                !(payload["Username"] is string) ||  !(payload["Password"]is string))
                return Response.Status(Status.BadRequest);
            var result 
                = auth.Register((payload["Username"] as string)!, (payload["Password"] as string)!);
            return !result.Item1 ? Response.Status(Status.Conflict) : Response.PlainText(result.Item2, Status.Created);
        } 
    }
}