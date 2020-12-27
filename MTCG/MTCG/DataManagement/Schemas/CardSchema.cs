using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace MTCG.DataManagement.Schemas
{
    public class CardSchema
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Damage { get; set; }
        public string? PackageId { get; set; }
        public string? UserId { get; set; }
        public string? StoreId { get; set; }
        public bool InDeck { get; set; }

        public CardSchema(string id, string name, double damage, string? packageId, string? userId, string? storeId, bool inDeck)
        {
            Id = id;
            Name = name;
            Damage = damage;
            PackageId = packageId;
            UserId = userId;
            StoreId = storeId;
            InDeck = inDeck;
        }

        public static List<CardSchema> ParseRequest(JArray array)
        {
            // Parse Cards
            // See: https://stackoverflow.com/a/41810862/12347616
            var cards = new List<CardSchema>();
            foreach (var arrayToken in array)
            {
                var rawCard = (JArray) arrayToken;
                foreach (var itemToken in rawCard)
                {
                    var item = (JObject) itemToken;
                    
                }
            }

            return cards;
        }
    }
}