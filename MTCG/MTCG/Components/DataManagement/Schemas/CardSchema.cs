using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace MTCG.Components.DataManagement.Schemas
{
    /// <summary>
    /// Data class that represent a card.
    /// </summary>
    public class CardSchema
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Damage { get; set; }
        public string? PackageId { get; set; }
        public string? UserId { get; set; }
        public string? StoreId { get; set; }
        public bool InDeck { get; set; }

        // Used when inserting new cards
        public CardSchema(string id, string name, double damage)
        {
            Id = id;
            Name = name;
            Damage = damage;
        }
        
        public CardSchema(string id, string name, double damage, string userId)
        {
            Id = id;
            Name = name;
            Damage = damage;
            UserId = userId;
        }
        
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

        /// <summary>
        /// Used to parse a JSON request of cards into a list of <c>CardSchema</c>s.
        /// </summary>
        /// <param name="array"></param>
        /// <returns>
        /// List of <c>CardSchema</c>s
        /// </returns>
        public static List<CardSchema> ParseRequest(JArray array)
        {
            // Parse Cards
            // See: https://stackoverflow.com/a/41810862/12347616
            var cards = new List<CardSchema>();
            foreach (var jToken in array)
            {
                if (!(jToken is JObject item)) continue;
                if (item["Id"] == null || item["Name"] == null || item["Damage"] == null) continue;
                try
                {
                    // Convert values
                    var id = item.GetValue("Id").ToObject<string>();
                    var name = item.GetValue("Name").ToObject<string>();
                    var damage = item.GetValue("Damage").ToObject<double>();
                    cards.Add(new CardSchema(id, name, damage));
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return cards;
        }
    }
}