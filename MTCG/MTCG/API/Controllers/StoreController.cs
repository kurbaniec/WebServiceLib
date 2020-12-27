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
            // Check parameters
            if (!(user is { } userDetails) || !(payload is { } json))
                return Response.Status(Status.BadRequest);
            // Package needs to consists of 5 cards
            if (!(json["array"] is JArray rawCards) || rawCards.Count != 5)
                return Response.Status(Status.BadRequest);
            // Get user and check if its an admin account
            var userSchema = db.GetUser(userDetails.Username);
            if (userSchema is null) return Response.Status(Status.BadRequest);
            if (userSchema.Role != Role.Admin) return Response.Status(Status.Forbidden);
            // Parse given cards
            var cards = CardSchema.ParseRequest(rawCards);
            // Check if all cards were correctly parsed
            if (cards.Count != 5) return Response.Status(Status.BadRequest);
            // Add package and return corresponding response
            var result = db.AddPackage(cards);
            return Response.Status(result ? Status.Created : Status.Conflict);
        }
    }
}