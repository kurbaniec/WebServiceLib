using MTCG.Cards;
using MTCG.Cards.Monsters;
using NUnit.Framework;

namespace MTCG_Test
{
    public class FireElveTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestEvadeWithDragon()
        {
            var fireElve = new FireElve("test", 0, ElementType.Normal, 0);
            var dragon = new Dragon("test", 0, ElementType.Normal, 0);
            var check = fireElve.CanEvade(dragon);
            Assert.IsTrue(check);
        }

        [Test]
        public void TestEvadeWithNonDragonCard()
        {
            var fireElve = new FireElve("test", 0, ElementType.Normal, 0);
            var notDragon = new Ork("test", 0, ElementType.Normal, 0);
            var check = fireElve.CanEvade(notDragon);
            Assert.IsFalse(check);
        }
    }
}