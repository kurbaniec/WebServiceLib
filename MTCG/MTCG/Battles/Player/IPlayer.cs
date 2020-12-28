using System.Collections.Generic;
using MTCG.Cards.Basis;
using WebService_Lib.Attributes.Rest;

namespace MTCG.Battles.Player
{
    public interface IPlayer
    {
        string Username { get; }
        Dictionary<string, object>? BattleResult { get; set; }
        ICard LastPlayed { get; }
        int CardCount { get; }

        void AddDeck(List<ICard> cards);
        void AddToDeck(ICard card);
        void RemoveFromDeck(ICard card);
        ICard GetRandomCard();
    }
}