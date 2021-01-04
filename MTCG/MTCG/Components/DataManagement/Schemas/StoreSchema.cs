using System;

namespace MTCG.Components.DataManagement.Schemas
{
    /// <summary>
    /// Data class that represent a trading deal.
    /// </summary>
    public class StoreSchema
    {
        public string Id { get; set; }
        public string CardToTradeId { get; set; }
        public string CardToTradeName { get; set; }
        public double CardToTradeDamage { get; set; }
        public string Wanted { get; set; }
        public double MinimumDamage { get; set; }

        public StoreSchema(string id, string cardId, string cardName, double cardDamage, string wanted, double minimumDamage)
        {
            Id = id;
            CardToTradeId = cardId;
            CardToTradeName = cardName;
            CardToTradeDamage = cardDamage;
            Wanted = wanted;
            MinimumDamage = minimumDamage;
        }
        
        public StoreSchema(string id, string cardId, string wanted, double minimumDamage)
        {
            Id = id;
            CardToTradeId = cardId;
            Wanted = wanted;
            MinimumDamage = minimumDamage;
            CardToTradeName = string.Empty;
        }
    }
}