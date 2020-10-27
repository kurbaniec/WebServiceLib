using MTCG.Cards;
using MTCG.Cards.Monsters;
using NUnit.Framework;

namespace MTCG_Test
{
    public class WizzardTess
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestResistanceAgainstOrk()
        {
            var wizzard = new Wizzard("test", 0, ElementType.Normal, 0);
            var ork = new Ork("test", 0, ElementType.Normal, 0);
            var check = wizzard.IsResistant(ork);
            Assert.IsTrue(check);
        }

        [Test]
        public void TestResistanceAgainstNonOrk()
        {
            var wizzard = new Wizzard("test", 0, ElementType.Normal, 0);
            var notOrk = new Dragon("test", 0, ElementType.Normal, 0);
            var check = wizzard.IsResistant(notOrk);
            Assert.IsFalse(check);
        }

    }
}