using System.Collections.Generic;
using MTCG.Components.DataManagement.Schemas;

namespace MTCG.Components.DataManagement.DB
{
    public interface IDatabase
    {
        void CreateDatabaseIfNotExists();
        
        bool AddPackage(List<CardSchema> cards);
        bool AcquirePackage(string username, long packageCost = 5);

        bool AddUser(UserSchema user);
        UserSchema? GetUser(string username);
        StatsSchema? GetUserStats(string username);
        bool EditUserProfile(string username, string realname, string bio, string image);
        bool ConfigureDeck(string username, List<string> cardIds);
        List<CardSchema> GetUserDeck(string username);
        List<CardSchema> GetUserCards(string username);
        CardSchema? GetUserCard(string cardId);
        List<StatsSchema> GetScoreboard();
        List<StoreSchema> GetTradingDeals();
        Trade? GetTradingDeal(string id);
        bool AddTradingDeal(string username, StoreSchema deal);
        bool DeleteTradingDeal(string username, string id);

        bool Trade(
            string cardUser, string cardOffer,
            string storeUser, string cardToTrade, string storeId
        );
        bool AddBattleResultModifyEloAndGiveCoins(
            string playerA, string playerB, string log, bool draw, 
            string winner = "", string looser = "", 
            int eloWin = 30, int eloLoose = 50,
            int coinsWin = 2, int coinsDraw = 1
        );
    }
}