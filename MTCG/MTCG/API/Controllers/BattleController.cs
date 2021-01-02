using System.Collections.Generic;
using MTCG.Components.DataManagement.DB;
using MTCG.Components.DataManagement.Schemas;
using MTCG.Components.Service;
using Newtonsoft.Json;
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
        private readonly GameCoordinator game = null!;

        [Autowired] private readonly PostgresDatabase db = null!;
        
        [Post("/battles")]
        public Response LetsBattle(AuthDetails? user)
        {
            if (user is null) return Response.Status(Status.BadRequest);
            var result = game.Play(user.Username);
            return result is null ? Response.Status(Status.BadRequest) : Response.Json(result);
        }

        [Get("/admin/battles")]
        public Response GetBattleHistory(PathVariable<int> path, AuthDetails? user)
        {
            if (user is null) return Response.Status(Status.BadRequest);
            var userSchema = db.GetUser(user.Username);
            if (userSchema is null) return Response.Status(Status.BadRequest);
            if (userSchema.Role != Role.Admin) return Response.Status(Status.Forbidden);
            var battles = db.GetBattleHistory(path.Value);
            var response = new Dictionary<string, object>();
            var battleList = new List<Dictionary<string, object>>();
            foreach (var battle in battles)
            {
                if (battle.IsDraw)
                {
                    battleList.Add(new Dictionary<string, object>()
                    {
                        {"Id", battle.Id}, {"PlayerA", battle.PlayerA}, {"PlayerB", battle.PlayerB},
                        {"Draw", battle.IsDraw}
                    });
                }
                else
                {
                    battleList.Add(new Dictionary<string, object>()
                    {
                        {"Id", battle.Id}, {"PlayerA", battle.PlayerA}, {"PlayerB", battle.PlayerB},
                        {"Draw", battle.IsDraw}, {"Winner", battle.Winner!}, {"Looser", battle.Looser!}
                    });
                }
            }

            response["battles"] = battleList;
            return Response.Json(response);
        }

        [Get("/admin/battle")]
        public Response GetBattleLog(PathVariable<int> path, AuthDetails? user)
        {
            if (user is null || !path.Ok) return Response.Status(Status.BadRequest);
            var userSchema = db.GetUser(user.Username);
            if (userSchema is null) return Response.Status(Status.BadRequest);
            if (userSchema.Role != Role.Admin) return Response.Status(Status.Forbidden);
            var log = db.GetBattleLog(path.Value);
            if (log is null) return Response.Status(Status.BadRequest);
            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(log);
            return json is {} 
                ? Response.Json(json) 
                : Response.Status(Status.BadRequest);
        }
    }
}