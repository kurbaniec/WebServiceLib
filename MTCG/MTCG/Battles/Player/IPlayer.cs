using System.Collections.Generic;
using MTCG.Cards.Basis;

namespace MTCG.Battles.Player
{
    /// <summary>
    /// Interface that describe a player of a battle.
    /// </summary>
    public interface IPlayer
    {
        string Username { get; }
        Dictionary<string, object>? BattleResult { get; set; }
        ICard LastPlayed { get; }
        int CardCount { get; }

        /// <summary>
        /// Add a deck of cards that will be used for the battle.
        /// </summary>
        /// <param name="cards"></param>
        void AddDeck(List<ICard> cards);
        
        /// <summary>
        /// Add a single card to the existing deck.
        /// </summary>
        /// <param name="card"></param>
        void AddToDeck(ICard card);
        
        /// <summary>
        /// Remove a concrete card from the deck.
        /// </summary>
        /// <param name="card"></param>
        void RemoveFromDeck(ICard card);
        
        /// <summary>
        /// Get a random card from the deck.
        /// </summary>
        /// <returns>
        /// Returns a random card that is a member of the current deck.
        /// </returns>
        ICard GetRandomCard();
    }
}