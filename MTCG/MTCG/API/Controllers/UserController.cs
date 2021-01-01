using System.Collections.Generic;
using System.Linq;
using MTCG.Components.DataManagement.DB;
using Newtonsoft.Json.Linq;
using WebService_Lib;
using WebService_Lib.Attributes;
using WebService_Lib.Attributes.Rest;
using WebService_Lib.Server;

namespace MTCG.API.Controllers
{
    [Controller]
    public class UserController
    {
        [Autowired]
        private readonly AuthCheck auth = null!;

        [Autowired] 
        private readonly PostgresDatabase db = null!;
        
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

        [Post("/sessions")]
        public Response Login(Dictionary<string, object>? payload)
        {
            if (payload == null) return Response.Status(Status.BadRequest);
            if (!payload.ContainsKey("Username") || !payload.ContainsKey("Password") ||
                !(payload["Username"] is string) ||  !(payload["Password"]is string))
                return Response.Status(Status.BadRequest);
            var result
                = auth.Authenticate((payload["Username"] as string)!, (payload["Password"] as string)!);
            return result.Item1 ? Response.PlainText(result.Item2!) : Response.Status(Status.Unauthorized);
        }

        [Get("/cards")]
        public Response GetCards(AuthDetails? user)
        {
            if (user is null) return Response.Status(Status.BadRequest);
            var cards = db.GetUserCards(user.Username);
            var response = new Dictionary<string, object>();
            var cardsResponse
                = cards.Select(card => new Dictionary<string, object>
                {
                    ["Id"] = card.Id, ["Name"] = card.Name, ["Damage"] = card.Damage
                }).ToList();
            response["cards"] = cardsResponse; 
            return Response.Json(response); 
        }
        
        [Get("/deck")]
        public Response GetDeck(RequestParam param, AuthDetails? user)
        {
            if (user is null) return Response.Status(Status.BadRequest);
            var cards = db.GetUserDeck(user.Username);
            // Send in requested format
            if (!param.Empty && param.Value["format"] == "plain")
            {
                // Send in plaintext
                var plainResponse 
                    = cards.Aggregate("Id,Name,Damage\r\n", (current, card) 
                        => current + $"{card.Id},{card.Name},{card.Damage}\r\n");
                // Trim last newline
                // See: https://stackoverflow.com/a/1038062/12347616
                plainResponse = plainResponse.TrimEnd( '\r', '\n' );
                return Response.PlainText(plainResponse);
            }
            // Send in JSON
            var jsonResponse = new Dictionary<string, object>();
            var cardsResponse
                = cards.Select(card => new Dictionary<string, object>
                {
                    ["Id"] = card.Id, ["Name"] = card.Name, ["Damage"] = card.Damage
                }).ToList();
            jsonResponse["deck"] = cardsResponse; 
            return Response.Json(jsonResponse); 
        }

        [Put("/deck")]
        public Response ConfigureDeck(AuthDetails? user, Dictionary<string, object>? payload)
        {
            if (user is null) return Response.Status(Status.BadRequest);
            Status status;
            // Deck consists of 4 cards
            if (!(payload?["array"] is JArray rawIds) || rawIds.Count != 4)
                status = Status.BadRequest;
            else
            {
                var cardIds = rawIds.ToObject<List<string>>();
                status = db.ConfigureDeck(user.Username, cardIds) 
                    ? Status.Created : Status.BadRequest;
            }
            var cards = db.GetUserDeck(user.Username);
            var response = new Dictionary<string, object>();
            var cardsResponse
                = cards.Select(card => new Dictionary<string, object>
                {
                    ["Id"] = card.Id, ["Name"] = card.Name, ["Damage"] = card.Damage
                }).ToList();
            response["deck"] = cardsResponse; 
            return Response.Json(response, status);
        }

        [Get("/stats")]
        public Response GetStats(AuthDetails? user)
        {
            if (user is null) return Response.Status(Status.BadRequest);
            var stats = db.GetUserStats(user.Username);
            if (stats is null) return Response.Status(Status.BadRequest);
            var response = new Dictionary<string, object>
            {
                ["Elo"] = stats.Elo, ["Wins"] = stats.Wins, ["Looses"] = stats.Looses, 
                ["Draws"] = stats.Draws, ["Total Games Played"] = stats.GamesPlayed,
                ["Coins"] = stats.Coins
            };
            return Response.Json(response);
        }

        [Get("/users")]
        public Response GetUserProfile(PathVariable<string> username, AuthDetails? user)
        {
            if (!username.Ok || user is null) return Response.Status(Status.BadRequest);
            if (username.Value != user.Username) return Response.Status(Status.Forbidden);
            var stats = db.GetUserStats(user.Username);
            if (stats is null) return Response.Status(Status.BadRequest);
            var response = new Dictionary<string, object>
            {
                ["Username"] = user.Username, ["Realname"] = stats.Realname,
                ["Bio"] = stats.Bio, ["Image"] = stats.Image
            };
            return Response.Json(response);
        }

        [Put("/users")]
        public Response UpdateUserProfile(
            PathVariable<string> username, AuthDetails? user, Dictionary<string, object>? payload
        )
        {
            if (!username.Ok || user is null || payload is null) return Response.Status(Status.BadRequest);
            if (username.Value != user.Username) return Response.Status(Status.Forbidden);
            if (!payload.ContainsKey("Name") || !(payload["Name"] is string name) || 
                !payload.ContainsKey("Bio") || !(payload["Bio"] is string bio) || 
                !payload.ContainsKey("Image") || !(payload["Image"] is string image)) 
                return Response.Status(Status.BadRequest);
            return db.EditUserProfile(user.Username, name, bio, image) ? 
                GetUserProfile(username, user) : 
                Response.Status(Status.BadRequest);
        }

        [Get("/score")]
        public Response GetScoreboard()
        {
            var stats = db.GetScoreboard();
            var response = new Dictionary<string, object>();
            var users
                = stats.Select(stat => new Dictionary<string, object>
                {
                    ["Username"] = stat.Username, ["Elo"] = stat.Elo,
                    ["Wins"] = stat.Wins, ["Looses"] = stat.Looses,
                    ["Draws"] = stat.Draws
                }).ToList();
            response["scoreboard"] = users;
            return Response.Json(response);
        }
    }
}