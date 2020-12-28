using System.Collections.Generic;
using System.Linq;
using Moq;
using MTCG.Battles.Basis;
using MTCG.Battles.Logging;
using MTCG.Battles.Player;
using MTCG.Cards.Basis;
using MTCG.Cards.Basis.Monster;
using MTCG.Cards.Basis.Spell;
using MTCG.Cards.DamageUtil;
using MTCG.Cards.Factory;
using MTCG.Cards.Specialities.Types.Miss;
using NUnit.Framework;

namespace MTCG_Test.Unit
{
    public class BattleTest
    {
        private IPlayerLog playerLogA = null!;
        private IPlayerLog playerLogB = null!;
        private IBattleLog battleLog = null!;

        [OneTimeSetUp]
        public void Setup()
        {
            var playerLogAMock = new Mock<IPlayerLog>();
            // Setup property without setter
            // See: https://stackoverflow.com/a/4662991/12347616
            playerLogAMock.SetupGet(log => log.Log).Returns(new List<string>());
            playerLogA = playerLogAMock.Object;
            var playerLogBMock = new Mock<IPlayerLog>();
            playerLogBMock.SetupGet(log => log.Log).Returns(new List<string>());
            playerLogB = playerLogBMock.Object;
            var battleLogMock = new Mock<IBattleLog>();
            battleLogMock.Setup(log => log.GetLog()).Returns(new Dictionary<string, object>());
            battleLog = battleLogMock.Object;
        }
        
        [Test, TestCase(TestName = "Test Battle where Player A wins", Description =
             "Test Battle where Player A wins"
         )]
        public void BattlePlayerAWin()
        {
            var tmpGoblinMock = new Mock<IMonsterCard>();
            tmpGoblinMock.Setup(card => card.MonsterType).Returns(MonsterType.Goblin);
            var goblinMock = tmpGoblinMock.As<ICard>();
            goblinMock.SetupProperty(card => card.Log, playerLogA);
            goblinMock.SetupProperty(card => card.Damage, 100);
            goblinMock.Setup(card => card.CalculateDamage(It.IsAny<ICard>())).Returns(Damage.Normal(100));
            var goblin = goblinMock.Object;
            
            var tmpTrollMock = new Mock<IMonsterCard>();
            tmpTrollMock.Setup(card => card.MonsterType).Returns(MonsterType.Troll);
            var trollMock = tmpTrollMock.As<ICard>();
            trollMock.SetupProperty(card => card.Log, playerLogB);
            trollMock.SetupProperty(card => card.Damage, 10);
            trollMock.Setup(card => card.CalculateDamage(It.IsAny<ICard>())).Returns(Damage.Normal(10));
            var troll = trollMock.Object;
            
            var playerAMock = new Mock<IPlayer>();
            playerAMock.SetupGet(player => player.Username).Returns("A");
            playerAMock.SetupGet(player => player.CardCount).Returns(1);
            playerAMock.Setup(player => player.GetRandomCard()).Returns(goblin);
            var playerA = playerAMock.Object;
            
            var playerBMock = new Mock<IPlayer>();
            playerBMock.SetupGet(player => player.Username).Returns("B");
            playerBMock.SetupGet(player => player.CardCount).Returns(0);
            playerBMock.Setup(player => player.GetRandomCard()).Returns(troll);
            var playerB = playerBMock.Object;
            
            var battle = new Battle(playerA, playerB, battleLog);
            var result = battle.ProcessBattle();
            
            Assert.IsFalse(result.Draw);
            Assert.AreEqual(playerA.Username, result.Winner);
            Assert.AreEqual(playerB.Username, result.Looser);
        }
        
        [Test, TestCase(TestName = "Test Battle where Player B wins", Description =
             "Test Battle where Player B wins"
         )]
        public void BattlePlayerBWin()
        {
            var tmpGoblinMock = new Mock<IMonsterCard>();
            tmpGoblinMock.Setup(card => card.MonsterType).Returns(MonsterType.Goblin);
            var goblinMock = tmpGoblinMock.As<ICard>();
            goblinMock.SetupProperty(card => card.Log, playerLogA);
            goblinMock.SetupProperty(card => card.Damage, 10);
            goblinMock.Setup(card => card.CalculateDamage(It.IsAny<ICard>())).Returns(Damage.Normal(10));
            var goblin = goblinMock.Object;
            
            var tmpTrollMock = new Mock<IMonsterCard>();
            tmpTrollMock.Setup(card => card.MonsterType).Returns(MonsterType.Troll);
            var trollMock = tmpTrollMock.As<ICard>();
            trollMock.SetupProperty(card => card.Log, playerLogB);
            trollMock.SetupProperty(card => card.Damage, 100);
            trollMock.Setup(card => card.CalculateDamage(It.IsAny<ICard>())).Returns(Damage.Normal(100));
            var troll = trollMock.Object;
            
            var playerAMock = new Mock<IPlayer>();
            playerAMock.SetupGet(player => player.Username).Returns("A");
            playerAMock.SetupGet(player => player.CardCount).Returns(0);
            playerAMock.Setup(player => player.GetRandomCard()).Returns(goblin);
            var playerA = playerAMock.Object;
            
            var playerBMock = new Mock<IPlayer>();
            playerBMock.SetupGet(player => player.Username).Returns("B");
            playerBMock.SetupGet(player => player.CardCount).Returns(1);
            playerBMock.Setup(player => player.GetRandomCard()).Returns(troll);
            var playerB = playerBMock.Object;
            
            var battle = new Battle(playerA, playerB, battleLog);
            var result = battle.ProcessBattle();
            
            Assert.IsFalse(result.Draw);
            Assert.AreEqual(playerB.Username, result.Winner);
            Assert.AreEqual(playerA.Username, result.Looser);
        }
        
        [Test, TestCase(TestName = "Test Battle with Draw", Description =
             "Test Battle for Draw with both players sharing the same card"
         )]
        public void BattleDraw()
        {
            var tmpGoblinMock = new Mock<IMonsterCard>();
            tmpGoblinMock.Setup(card => card.MonsterType).Returns(MonsterType.Goblin);
            var goblinMock = tmpGoblinMock.As<ICard>();
            goblinMock.SetupProperty(card => card.Log, playerLogA);
            goblinMock.SetupProperty(card => card.Damage, 10);
            goblinMock.Setup(card => card.CalculateDamage(It.IsAny<ICard>())).Returns(Damage.Normal(10));
            var goblin = goblinMock.Object;
            
            var tmpTrollMock = new Mock<IMonsterCard>();
            tmpTrollMock.Setup(card => card.MonsterType).Returns(MonsterType.Troll);
            var trollMock = tmpTrollMock.As<ICard>();
            trollMock.SetupProperty(card => card.Log, playerLogB);
            trollMock.SetupProperty(card => card.Damage, 10);
            trollMock.Setup(card => card.CalculateDamage(It.IsAny<ICard>())).Returns(Damage.Normal(10));
            var troll = trollMock.Object;
            
            var playerAMock = new Mock<IPlayer>();
            playerAMock.SetupGet(player => player.Username).Returns("A");
            playerAMock.SetupGet(player => player.CardCount).Returns(1);
            playerAMock.Setup(player => player.GetRandomCard()).Returns(goblin);
            var playerA = playerAMock.Object;
            
            var playerBMock = new Mock<IPlayer>();
            playerBMock.SetupGet(player => player.Username).Returns("B");
            playerBMock.SetupGet(player => player.CardCount).Returns(1);
            playerBMock.Setup(player => player.GetRandomCard()).Returns(troll);
            var playerB = playerBMock.Object;
            
            var battle = new Battle(playerA, playerB, battleLog);
            var result = battle.ProcessBattle();
            
            Assert.IsTrue(result.Draw);
        }
    }
}