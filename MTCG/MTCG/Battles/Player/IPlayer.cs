using MTCG.Cards.Basis;
using WebService_Lib.Attributes.Rest;

namespace MTCG.Battles.Player
{
    public interface IPlayer
    {
        string Username { get; }
        ICard LastPlayed { get; }
        int CardCount { get; }

        void AddToDeck(ICard card);
        void RemoveFromDeck(ICard card);
        ICard GetRandomCard();
    }
}