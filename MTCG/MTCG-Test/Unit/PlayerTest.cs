using System.Collections.Generic;
using Moq;
using MTCG.Battles.Basis;
using MTCG.Battles.Player;
using MTCG.Cards.Basis;
using MTCG.Cards.Basis.Monster;
using MTCG.Cards.DamageUtil;
using NUnit.Framework;

namespace MTCG_Test.Unit
{
    public class PlayerTest
    {
        [Test, TestCase(TestName = "Test adding a new Deck", Description =
             "Test adding a new Deck"
         )]
        public void PlayerAddDeck()
        {
            var cards = new List<ICard>()
            {
                new Mock<ICard>().Object, new Mock<ICard>().Object
            };
            var player = new Player("player");

            player.AddDeck(cards);
            var count = player.CardCount;

            Assert.AreEqual(2, count);
        }
        
        [Test, TestCase(TestName = "Test Getter for Card Count", Description =
             "Test Getter for Card Count"
         )]
        public void PlayerCardCount()
        {
            var cards = new List<ICard>()
            {
                new Mock<ICard>().Object, new Mock<ICard>().Object
            };
            var player = new Player("player", cards);

            var count = player.CardCount;

            Assert.AreEqual(2, count);
        }
        
        [Test, TestCase(TestName = "Test Card Addition", Description =
             "Test Card Addition"
         )]
        public void PlayerAddToDeck()
        {
            var cards = new List<ICard>()
            {
                new Mock<ICard>().Object, new Mock<ICard>().Object
            };
            var player = new Player("player", cards);

            var initialCount = player.CardCount;
            player.AddToDeck(new Mock<ICard>().Object);
            var additionCount = player.CardCount;

            Assert.AreEqual(2, initialCount);
            Assert.AreEqual(3, additionCount);
        }
        
        [Test, TestCase(TestName = "Test Card Remove", Description =
             "Test Card Remove"
         )]
        public void PlayerRemoveFromDeck()
        {
            var card = new Mock<ICard>().Object;
            var cards = new List<ICard>()
            {
                card, new Mock<ICard>().Object
            };
            var player = new Player("player", cards);

            var initialCount = player.CardCount;
            player.RemoveFromDeck(card);
            var removeCount = player.CardCount;

            Assert.AreEqual(2, initialCount);
            Assert.AreEqual(1, removeCount);
        }
        
        [Test, TestCase(TestName = "Test access for random Card", Description =
             "Test access for random Card"
         )]
        public void PlayerRandomCard()
        {
            var cards = new List<ICard>()
            {
                new Mock<ICard>().Object, new Mock<ICard>().Object
            };
            var player = new Player("player", cards);

            var card = player.GetRandomCard();

            Assert.Contains(card, cards);
        }
        
        
    }
}