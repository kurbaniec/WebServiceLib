using System.Collections.Generic;
using MTCG.Components.Service;
using WebService_Lib;
using WebService_Lib.Attributes;
using WebService_Lib.Attributes.Rest;
using WebService_Lib.Server;

namespace MTCG.API.Controllers
{
    [Controller]
    public class BattleController
    {
        [Autowired] 
        private GameCoordinator game = null!;
        
        [Get("/battles")]
        public Response LetsBattle(AuthDetails? user)
        {
            // TODO dont battle same user...
            if (user is null) return Response.Status(Status.BadRequest);
            var result = game.Play(user.Username);
            return result is null ? Response.Status(Status.BadRequest) : Response.Json(result);
        }
    }
}