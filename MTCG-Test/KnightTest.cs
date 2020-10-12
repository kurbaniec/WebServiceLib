using MTCG.Cards;
using MTCG.Cards.Monsters;
using MTCG.Cards.Spells;
using NUnit.Framework;

namespace MTCG_Test
{
    public class KnightTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestWeaknessAgainstWaterSpell()
        {
            var knight = new Knight("test", 0, ElementType.Normal, 0);
            var waterSpell = new WaterSpell("test", ElementType.Water, 0);
            var check = knight.IsWeak(waterSpell);
            Assert.IsTrue(check);
        }

        [Test]
        public void TestWeaknessAgainstNonWaterSpell()
        {
            var knight = new Knight("test", 0, ElementType.Normal, 0);
            var notWaterSpell = new FireSpell("test", ElementType.Water, 0);
            var check = knight.IsWeak(notWaterSpell);
            Assert.IsFalse(check);
        }

    }
}