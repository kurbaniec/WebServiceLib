namespace MTCG.Components.DataManagement.Schemas
{
    public class Trade
    {
        public StoreSchema Store { get; }
        public CardSchema Card { get; }

        public Trade(StoreSchema store, CardSchema card)
        {
            Store = store;
            Card = card;
        }
    }
}