using System.Collections.Generic;
using MTCG.Components.DataManagement.Schemas;

namespace MTCG.Components.DataManagement.DB
{
    public interface IDatabase
    {
        void CreateDatabaseIfNotExists();
        
        bool AddPackage(List<CardSchema> cards);
        bool AcquirePackage(string username, long packageCost);

        bool AddUser(UserSchema user);
        UserSchema? GetUser(string username);
        StatsSchema? GetUserStats(string username);
        
        bool EditUserProfile(string username, string bio, string image);

        bool ConfigureDeck(string username, List<string> cardIds);
        List<CardSchema> GetUserDeck(string username);
        List<CardSchema> GetUserCards(string username);
        
        List<StatsSchema> GetScoreboard();

        List<StoreSchema> GetTradingDeals();
        bool AddTradingDeal(string username, StoreSchema deal);
        bool Trade(string username, string myDeal, string otherDeal);

        
    }
}