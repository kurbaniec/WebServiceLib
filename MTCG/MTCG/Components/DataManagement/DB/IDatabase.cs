using System.Collections.Generic;
using MTCG.Components.DataManagement.Schemas;

namespace MTCG.Components.DataManagement.DB
{
    /// <summary>
    /// Interface that describes needed database functionality.
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Checks if the database exists. If not, the database
        /// and all needed tables are created.
        /// </summary>
        void CreateDatabaseIfNotExists();
        
        /// <summary>
        /// Add a new package from a list of cards.
        /// </summary>
        /// <param name="cards">
        /// Cards that will be part of the package
        /// </param>
        /// <returns>
        /// True if operation succeeded, else false
        /// </returns>
        bool AddPackage(List<CardSchema> cards);
        
        /// <summary>
        /// Used to acquire a new package of cards by an user.
        /// </summary>
        /// <param name="username">
        /// User who wants to acquire the package
        /// </param>
        /// <param name="packageCost">
        /// Cost of coins for one package
        /// </param>
        /// <returns>
        /// True if operation succeeded, else false
        /// </returns>
        bool AcquirePackage(string username, long packageCost = 5);

        /// <summary>
        /// Used to a new user (regular and admin) to the system.
        /// </summary>
        /// <param name="user">New user</param>
        /// <returns>
        /// True if operation succeeded, else false
        /// </returns>
        bool AddUser(UserSchema user);
        
        /// <summary>
        /// Used to query basic user information
        ///  (username, hashed-password, user/admin).
        /// </summary>
        /// <param name="username"></param>
        /// <returns>
        /// <c>UserSchema</c> when user exists, else null
        /// </returns>
        UserSchema? GetUser(string username);
        
        /// <summary>
        /// Used to query user stats.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>
        /// <c>StatsSchema</c> when user exists, else null
        /// </returns>
        StatsSchema? GetUserStats(string username);
        
        /// <summary>
        /// Used to edit user profile information (realname, bio and image; part of stats schema).
        /// </summary>
        /// <param name="username"></param>
        /// <param name="realname"></param>
        /// <param name="bio"></param>
        /// <param name="image"></param>
        /// <returns>
        /// True if operation succeeded, else false
        /// </returns>
        bool EditUserProfile(string username, string realname, string bio, string image);
        
        /// <summary>
        /// Used to configure the users deck.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="cardIds">Ids of the cards that should be part of the deck</param>
        /// <returns>
        /// True if operation succeeded, else false
        /// </returns>
        bool ConfigureDeck(string username, List<string> cardIds);
        
        /// <summary>
        /// Used to query a user's deck.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>
        /// List of <c>CardSchema</c>s
        /// </returns>
        List<CardSchema> GetUserDeck(string username);
        
        /// <summary>
        /// Used to query all user cards.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>
        /// List of <c>CardSchema</c>s
        /// </returns>
        List<CardSchema> GetUserCards(string username);
        
        /// <summary>
        /// Used to query a specific card.
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns>
        /// <c>CardSchema</c> when card exists, else null
        /// </returns>
        CardSchema? GetUserCard(string cardId);
        
        /// <summary>
        /// Used to query the scoreboard (top 100 players).
        /// Returns a list of simplified <c>StatsSchema</c>s.
        /// </summary>
        /// <returns>
        /// List of <c>StatsSchema</c>s
        /// </returns>
        List<StatsSchema> GetScoreboard();
        
        /// <summary>
        /// Used to query all available trading deals.
        /// </summary>
        /// <returns>
        /// List of <c>StoreSchema</c>s
        /// </returns>
        List<StoreSchema> GetTradingDeals();
        
        /// <summary>
        /// Used to query a specific trading deal.
        /// Returns a <c>Trade</c> which contains the trading deal
        /// (<c>StoreSchema</c>) and the associated cart to trade
        /// (<c>CardSchema</c>.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// <c>Trade</c> when trading deal exists, else null
        /// </returns>
        Trade? GetTradingDeal(string id);
        
        /// <summary>
        /// Used to add a new trading deal.
        /// </summary>
        /// <param name="username">Name of the trader</param>
        /// <param name="deal">Information of the trading deal</param>
        /// <returns>
        /// True if operation succeeded, else false
        /// </returns>
        bool AddTradingDeal(string username, StoreSchema deal);
        
        /// <summary>
        /// Used to delete an existing trading deal.
        /// User must be associated with the trading deal in order to work.
        /// </summary>
        /// <param name="username">Name of the trader</param>
        /// <param name="id">Id of the trading deal</param>
        /// <returns>
        /// True if operation succeeded, else false
        /// </returns>
        bool DeleteTradingDeal(string username, string id);
        
        /// <summary>
        /// Used to perform a trading deal.
        /// </summary>
        /// <param name="cardUser">User who want to accept a trading deal</param>
        /// <param name="cardOffer">Card that is offered for a trading deal</param>
        /// <param name="storeUser">User who created the initial trading deal</param>
        /// <param name="cardToTrade">Cart that is associated with the trading deal</param>
        /// <param name="storeId">Id of the trading deal</param>
        /// <returns>
        /// True if operation succeeded, else false
        /// </returns>
        bool Trade(
            string cardUser, string cardOffer,
            string storeUser, string cardToTrade, string storeId
        );
        
        /// <summary>
        /// Used to modify user stats after a battle.
        /// Adds coins for the winner or both it its a draw.
        /// Also adds the battle result to a battle history (<c>BattleSchema</c>).
        /// </summary>
        /// <param name="playerA">Name of one player</param>
        /// <param name="playerB">Name of the other player</param>
        /// <param name="log">Battle log as string</param>
        /// <param name="draw">Did battle end in a draw?</param>
        /// <param name="winner">(Optional) Name of the winner</param>
        /// <param name="looser">(Optional) Name of the looser</param>
        /// <param name="eloWin">(Optional) Elo change for the winner</param>
        /// <param name="eloLoose">(Optional) Elo change for the looser</param>
        /// <param name="coinsWin">(Optional) Coin win for the winner</param>
        /// <param name="coinsDraw">(Optional) Coin win if draw</param>
        /// <returns>
        /// True if operation succeeded, else false
        /// </returns>
        bool AddBattleResultModifyEloAndGiveCoins(
            string playerA, string playerB, string log, bool draw, 
            string winner = "", string looser = "", 
            int eloWin = 30, int eloLoose = 50,
            int coinsWin = 2, int coinsDraw = 1
        );
        
        /// <summary>
        /// Query battle history of played games without their logs.
        /// A maximum total of 100 games are returned.
        /// </summary>
        /// <param name="page">Offset: which 100 games should be returned (0 = first, 1 = second, ..)</param>
        /// <returns>
        /// List of <c>BattleSchema</c>s
        /// </returns>
        List<BattleSchema> GetBattleHistory(int page);
        
        /// <summary>
        /// Get the battle log of a specific battle.
        /// </summary>
        /// <param name="battleId"></param>
        /// <returns>
        /// Battle log as string if the battle exists, else null
        /// </returns>
        string? GetBattleLog(int battleId);
    }
}