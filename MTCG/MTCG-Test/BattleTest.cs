using MTCG.Battles;
using MTCG.Cards;
using MTCG.Cards.Monsters;
using MTCG.Users;
using NUnit.Framework;

namespace MTCG_Test
{
    public class BattleTest
    {
#pragma warning disable CS8618
        private User playerA;
#pragma warning restore CS8618
        private User playerB;

        [SetUp]
        public void Setup()
        {
            playerA = new User();
            playerB = new User();
        }

        /*+
        [Test]
        public void TestBattleWithPlayerAWin()
        {
            // Set Cards TODO to player
            var battle = new Battle(playerA, playerB);
            // Start battle
            var result = battle.RunGame();
            // Check result
            Assert.AreEqual(playerA, result);
        }


        [Test]
        public void TestBattleWithPlayerBWin()
        {
            // Set Cards TODO to player
            var battle = new Battle(playerA, playerB);
            // Start battle
            var result = battle.RunGame();
            // Check result
            Assert.AreEqual(playerB, result);

        }

        [Test]
        public void TestBattleWithDraw()
        {
            // TODO fix
            // Set Cards TODO to player
            var battle = new Battle(playerA, playerB);
            // Start battle
            var result = battle.RunGame();
            // Check result
            Assert.AreEqual(null, result);

        }*/
    }
}