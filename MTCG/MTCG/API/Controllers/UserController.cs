using System;
using System.Collections.Generic;
using System.Linq;
using MTCG.DataManagement.DB;
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
        public Response GetDeck(AuthDetails? user)
        {
            if (user is null) return Response.Status(Status.BadRequest);
            var cards = db.GetUserDeck(user.Username);
            var response = new Dictionary<string, object>();
            var cardsResponse
                = cards.Select(card => new Dictionary<string, object>
                {
                    ["Id"] = card.Id, ["Name"] = card.Name, ["Damage"] = card.Damage
                }).ToList();
            response["deck"] = cardsResponse; 
            return Response.Json(response); 
        }

        [Post("/deck")]
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
    }
}