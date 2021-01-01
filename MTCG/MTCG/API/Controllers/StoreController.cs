using System;
using System.Collections.Generic;
using System.Linq;
using MTCG.Battles.Logging;
using MTCG.Cards.Basis;
using MTCG.Cards.Basis.Monster;
using MTCG.Cards.Basis.Spell;
using MTCG.Cards.Factory;
using MTCG.Components.DataManagement.DB;
using MTCG.Components.DataManagement.Schemas;
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
            if (!(user is { } userDetails) || payload == null)
                return Response.Status(Status.BadRequest);
            return AddPackage(user, payload);
        }
        
        [Post("/transactions/packages")]
        public Response AcquirePackages(AuthDetails? user)
        {
            // Check parameters
            if (!(user is { } userDetails) )
                return Response.Status(Status.BadRequest);
            return AcquirePackage(user);
        }
        
        private Response AddPackage(AuthDetails user, Dictionary<string, object> payload)
        {
            // Package needs to consists of 5 cards
            if (!(payload["array"] is JArray rawCards) || rawCards.Count != 5)
                return Response.Status(Status.BadRequest);
            // Get user and check if its an admin account
            var userSchema = db.GetUser(user.Username);
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
        
        private Response AcquirePackage(AuthDetails user)
        {
            var packageCost = 5;
            var userStats = db.GetUserStats(user.Username);
            if (userStats is null) 
                return Response.Status(Status.BadRequest);
            if (userStats.Coins - packageCost < 0) 
                return Response.Status(Status.BadRequest);
            return Response.Status(db.AcquirePackage(user.Username, packageCost) 
                ? Status.Created : Status.BadRequest);
        }

        [Get("/tradings")]
        public Response GetTradings(AuthDetails? user)
        {
            if (user is null) return Response.Status(Status.BadRequest);
            var response = new Dictionary<string, object>();
            var tradingsQuery = db.GetTradingDeals();
            var tradings 
                = tradingsQuery.Select(trade => new Dictionary<string, object>
                {
                    ["Id"] = trade.Id, ["CardToTrade-Name"] = trade.CardToTradeName,
                    ["CardToTrade-Damage"] = trade.CardToTradeDamage, 
                    ["Wanted"] = trade.Wanted, ["Minimum Damage"] = trade.MinimumDamage
                }).ToList();
            response["tradings"] = tradings;
            return Response.Json(response);
        }

        [Post("/tradings")]
        public Response AddOrPerformTrade(
            PathVariable<string> path, AuthDetails? user, Dictionary<string, object>? payload
        )
        {
            if (user is null || payload is null) return Response.Status(Status.BadRequest);
            if (path.Ok && path.Value != null) return PerformTrade(path.Value, user, payload);
            return AddTrade(user, payload);
        }

        private Response AddTrade(AuthDetails user, Dictionary<string, object> payload)
        {
            if (payload.ContainsKey("Id") && payload["Id"] is string id &&
                payload.ContainsKey("CardToTrade") && payload["CardToTrade"] is string tradeId &&
                payload.ContainsKey("Type") && payload["Type"] is string wanted &&
                payload.ContainsKey("MinimumDamage") && 
                Convert.ToDouble(payload.ContainsKey("MinimumDamage")) is var minDamage)
            {
                return Response.Status(db.AddTradingDeal(user.Username, new StoreSchema(id, tradeId, wanted, minDamage)) 
                    ? Status.Created 
                    : Status.BadRequest);
            }
            return Response.Status(Status.BadRequest);
        }

        private Response PerformTrade(
            string storeId, AuthDetails user, Dictionary<string, object> payload
        )
        {
            if (payload.ContainsKey("value") && payload["value"] is string cardId)
            {
                var trade = db.GetTradingDeal(storeId);
                var card = db.GetUserCard(cardId);
                if (trade is {} && card is {})
                {
                    // Cannot trade with oneself
                    if (trade.Card.UserId != card.UserId)
                    {
                        // Check damage requirements
                        if (card.Damage >= trade.Store.MinimumDamage)
                        {
                            // "Print" card
                            var printedCard = CardFactory.Print(card.Name, card.Damage, new PlayerLog(card.UserId!));
                            if (printedCard is {})
                            {
                                if (trade.Store.Wanted.ToLower() == "monster")
                                {
                                    if (printedCard is IMonsterCard)
                                    {
                                        
                                    }
                                } else if (trade.Store.Wanted.ToLower() == "spell")
                                {
                                    if (printedCard is ISpellCard)
                                    {
                                        
                                    }
                                }
                                else
                                {
                                    // Direct type comparison
                                    if (string.Equals(
                                        printedCard.ToString()!, trade.Store.Wanted, 
                                        StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Response.Status(Status.Ok);
        }

        [Delete("/tradings")]
        public Response DeleteTrade(PathVariable<string> path, AuthDetails? user)
        {
            if (!path.Ok || user is null || path.Value is null) return Response.Status(Status.BadRequest);
            return Response.Status(db.DeleteTradingDeal(user.Username, path.Value) 
                ? Status.NoContent 
                : Status.BadRequest);
        }
        
        
    }
}