namespace MTCG.Components.DataManagement.Schemas
{
    public class StoreSchema
    {
        public string Id { get; set; }
        public string CardToTrade { get; set; }
        public string Wanted { get; set; }
        public uint MinimumDamage { get; set; }

        public StoreSchema(string id, string cardId, string wanted, uint minimumDamage)
        {
            Id = id;
            CardToTrade = cardId;
            Wanted = wanted;
            MinimumDamage = minimumDamage;
        }
    }
}