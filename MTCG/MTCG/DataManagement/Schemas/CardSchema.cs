namespace MTCG.DataManagement.Schemas
{
    public class CardSchema
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public uint Damage { get; set; }
        public string? PackageId { get; set; }
        public string? UserId { get; set; }
        public string? StoreId { get; set; }
        public bool InDeck { get; set; }

        public CardSchema(string id, string name, uint damage, string? packageId, string? userId, string? storeId, bool inDeck)
        {
            Id = id;
            Name = name;
            Damage = damage;
            PackageId = packageId;
            UserId = userId;
            StoreId = storeId;
            InDeck = inDeck;
        }
    }
}