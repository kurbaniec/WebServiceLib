using System;
using System.Collections.Generic;
using MTCG.Cards.Basis;

namespace MTCG.Battles.Player
{
    /// <summary>
    /// Concrete implementation of <c>IPlayer</c>.
    /// Represents a player of a <c>Battle</c>.
    /// </summary>
    public class Player : IPlayer
    {
        public string Username { get; }
        private readonly List<ICard> deck;
        public int CardCount => deck.Count;
        public ICard LastPlayed { get; private set; } = null!;
        public Dictionary<string, object>? BattleResult { get; set; }
        private readonly Random rng;

        public Player(string username, List<ICard> deck)
        {
            Username = username;
            this.deck = deck;
            rng = new Random();
        }

        public Player(string username)
        {
            Username = username;
            deck = new List<ICard>();
            rng = new Random();
        }

        /// <summary>
        /// Add a deck of cards that will be used for the battle.
        /// </summary>
        /// <param name="cards"></param>
        public void AddDeck(List<ICard> cards)
        {
            deck.AddRange(cards);
        }

        /// <summary>
        /// Add a single card to the existing deck.
        /// </summary>
        /// <param name="card"></param>
        public void AddToDeck(ICard card)
        {
            deck.Add(card);
        }

        /// <summary>
        /// Remove a concrete card from the deck.
        /// </summary>
        /// <param name="card"></param>
        public void RemoveFromDeck(ICard card)
        {
            deck.Remove(card);
        }

        /// <summary>
        /// Get a random card from the deck.
        /// </summary>
        /// <returns>
        /// Returns a random card that is a member of the current deck.
        /// </returns>
        public ICard GetRandomCard()
        {
            // Random list access
            // See: https://stackoverflow.com/a/2019432/12347616
            var index = rng.Next(deck.Count);
            LastPlayed = deck[index];
            return LastPlayed;
        }
    }
}