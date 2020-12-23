using MTCG.Cards;
using MTCG.Cards.Monsters;
using MTCG.Cards.Spells;
using NUnit.Framework;

namespace MTCG_Test
{
    public class KrakenTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestResistanceAgainstSpells()
        {
            var kraken = new Kraken("test", 0, ElementType.Normal, 0);
            var waterSpell = new WaterSpell("test", ElementType.Water, 0);
            var fireSpell = new FireSpell("test", ElementType.Fire, 0);
            var normalSpell = new NormalSpell("test", ElementType.Normal, 0);
            var checkWater = kraken.IsResistant(waterSpell);
            var checkFire = kraken.IsResistant(fireSpell);
            var checkNormal = kraken.IsResistant(normalSpell);
            Assert.IsTrue(checkWater);
            Assert.IsTrue(checkFire);
            Assert.IsTrue(checkNormal);
        }

        [Test]
        public void TestResistanceAgainstNotSpells()
        {
            var kraken = new Kraken("test", 0, ElementType.Normal, 0);
            var notSpell = new Dragon("test", 0, ElementType.Normal, 0);
            var check = kraken.IsResistant(notSpell);
            Assert.IsFalse(check);
        }

    }
}