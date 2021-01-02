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
    /// <summary>
    /// <c>Controller</c> class that manages battle-related requests.
    /// </summary>
    [Controller]
    public class BattleController
    {
        [Autowired] 
        private readonly GameCoordinator game = null!;

        [Autowired] 
        private readonly PostgresDatabase db = null!;
        
        /// <summary>
        /// Endpoint used by players to enter matchmaking and play a game.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        /// Log of the played game as JSON
        /// </returns>
        [Post("/battles")]
        public Response LetsBattle(AuthDetails? user)
        {
            if (user is null) return Response.Status(Status.BadRequest);
            var result = game.Play(user.Username);
            return result is null ? Response.Status(Status.BadRequest) : Response.Json(result);
        }

        /// <summary>
        /// Endpoint used by admins to query battle history. A maximum of 100 battles is always returned.
        /// With a given PathVariable e.g. "/admin/battles/1" the number determines which 100 are returend.
        /// 0 means first hundred, 1 means 100 to 200 and so on.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <returns>
        /// JSON representation of all played games within the given limit
        /// </returns>
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

        /// <summary>
        /// Endpoint used by admins to query a log from a specific battle. Id is given via
        /// the PathVariable. The log is identical to the ones user get at the end of
        /// the battle.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <returns>
        /// Log of the requested game as JSON
        /// </returns>
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