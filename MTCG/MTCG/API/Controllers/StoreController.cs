using System.Collections.Generic;
using MTCG.DataManagement.DB;
using MTCG.DataManagement.Schemas;
using Newtonsoft.Json.Linq;
using WebService_Lib;
using WebService_Lib.Attributes;
using WebService_Lib.Attributes.Rest;
using WebService_Lib.Server;

namespace MTCG.API.Controllers
{
    [Controller]
    public class StoreController
    {
        [Autowired]
        private readonly AuthCheck auth = null!;

        [Autowired] 
        private readonly PostgresDatabase db = null!;
        
        [Post("/packages")]
        public Response AddPackages(AuthDetails? user, Dictionary<string, object>? payload)
        {
            if (!(user is { } userDetails) || !(payload is { } json))
                return Response.Status(Status.BadRequest);
            // Package needs to consists of 5 cards
            if (!(json["array"] is JArray rawCards) || rawCards.Count != 5)
                return Response.Status(Status.BadRequest);
            var userSchema = db.GetUser(userDetails.Username);
            if (userSchema is null) return Response.Status(Status.BadRequest);
            
            if (userSchema.Role == Role.Admin)
            {
                        
            } else return Response.Status(Status.Forbidden);
            return Response.Status(Status.BadRequest);
        }
    }
}