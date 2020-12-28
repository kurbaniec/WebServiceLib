using System;
using System.Collections.Generic;
using MTCG.Cards.Basis;

namespace MTCG.Battles.Player
{
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

        public void AddDeck(List<ICard> cards)
        {
            deck.AddRange(cards);
        }

        public void AddToDeck(ICard card)
        {
            deck.Add(card);
        }

        public void RemoveFromDeck(ICard card)
        {
            deck.Remove(card);
        }

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