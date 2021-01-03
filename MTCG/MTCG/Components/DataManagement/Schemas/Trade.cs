namespace MTCG.Components.DataManagement.Schemas
{
    /// <summary>
    /// Data class that represent a trading deal and the associated card to trade.
    /// </summary>
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