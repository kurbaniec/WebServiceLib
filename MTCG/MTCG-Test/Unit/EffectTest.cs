using Moq;
using MTCG.Battles;
using MTCG.Cards.Basis;
using MTCG.Cards.Basis.Monster;
using MTCG.Cards.Effects.Types.DamageModifier;
using NUnit.Framework;

namespace MTCG_Test.Unit
{
    public class EffectTest
    {
        private IBattleLog log = null!;

        [OneTimeSetUp]
        public void Setup() => log = new Mock<IBattleLog>().Object;
        
        [Test, TestCase(TestName = "Apply Boost Effect on Space Marine", Description =
             "Apply Boost Effect on Space Marine Monster-Type with the help of mocks"
         )]
        public void SpaceMarineBoostApply()
        {
            var tmpSpaceMarineMock = new Mock<IMonsterCard>();
            tmpSpaceMarineMock.Setup(card => card.MonsterType).Returns(MonsterType.SpaceMarine);
            var spaceMarineMock = tmpSpaceMarineMock.As<ICard>();
            spaceMarineMock.SetupProperty(card => card.Damage);
            var spaceMarine = spaceMarineMock.Object;
            var boostEffect = new Boost();
            
            boostEffect.Apply(spaceMarine);
            var result = spaceMarine.Damage;
            
            Assert.AreEqual(1, result);
        }
        
        [Test, TestCase(TestName = "Drop Boost Effect on Space Marine", Description =
             "Apply and Drop Boost Effect on Space Marine Monster-Type with the help of mocks"
         )]
        public void SpaceMarineBoostDrop()
        {
            var tmpSpaceMarineMock = new Mock<IMonsterCard>();
            tmpSpaceMarineMock.Setup(card => card.MonsterType).Returns(MonsterType.SpaceMarine);
            var spaceMarineMock = tmpSpaceMarineMock.As<ICard>();
            spaceMarineMock.SetupProperty(card => card.Damage);
            var spaceMarine = spaceMarineMock.Object;
            var boostEffect = new Boost();
            
            boostEffect.Apply(spaceMarine);
            boostEffect.Drop(spaceMarine);
            var result = spaceMarine.Damage;
            
            Assert.AreEqual(0, result);
        }
        
        [Test, TestCase(TestName = "Apply FiftyFifty Effect on Ork", Description =
             "Apply FiftyFifty Effect on Ork Monster-Type with the help of mocks"
         )]
        public void OrkFiftyFiftyApply()
        {
            var tmpOrkMock = new Mock<IMonsterCard>();
            tmpOrkMock.Setup(card => card.MonsterType).Returns(MonsterType.SpaceMarine);
            var orkMock = tmpOrkMock.As<ICard>();
            orkMock.SetupProperty(card => card.Damage, 10);
            var ork = orkMock.Object;
            var boostEffect = new FiftyFifty();
            
            boostEffect.Apply(ork);
            var result = ork.Damage;
            
            Assert.IsTrue(result == 12 || result == 8);
        }
        
        [Test, TestCase(TestName = "Drop FiftyFifty Effect on Ork", Description =
             "Apply and Drop FiftyFifty Effect on Ork Monster-Type with the help of mocks"
         )]
        public void OrkFiftyFiftyDrop()
        {
            var tmpOrkMock = new Mock<IMonsterCard>();
            tmpOrkMock.Setup(card => card.MonsterType).Returns(MonsterType.SpaceMarine);
            var orkMock = tmpOrkMock.As<ICard>();
            orkMock.SetupProperty(card => card.Damage, 10);
            var ork = orkMock.Object;
            var boostEffect = new FiftyFifty();
            
            boostEffect.Apply(ork);
            boostEffect.Drop(ork);
            var result = ork.Damage;
            
            Assert.AreEqual(10, result);
        }
        
        [Test, TestCase(TestName = "Apply FiftyFifty Effect on Ork on edge case", Description =
             "Apply FiftyFifty Effect on Ork Monster-Type on edge case with the help of mocks"
         )]
        public void OrkFiftyFiftyApplyEdgeCase()
        {
            var tmpOrkMock = new Mock<IMonsterCard>();
            tmpOrkMock.Setup(card => card.MonsterType).Returns(MonsterType.SpaceMarine);
            var orkMock = tmpOrkMock.As<ICard>();
            orkMock.SetupProperty(card => card.Damage, 0);
            var ork = orkMock.Object;
            var boostEffect = new FiftyFifty();
            
            boostEffect.Apply(ork);
            var result = ork.Damage;
            
            Assert.IsTrue(result == 0 || result == 2);
        }
        
        [Test, TestCase(TestName = "Drop FiftyFifty Effect on Ork on edge case", Description =
             "Apply and Drop FiftyFifty Effect on Ork Monster-Type on edge case with the help of mocks"
         )]
        public void OrkFiftyFiftyDropEdgeCase()
        {
            var tmpOrkMock = new Mock<IMonsterCard>();
            tmpOrkMock.Setup(card => card.MonsterType).Returns(MonsterType.SpaceMarine);
            var orkMock = tmpOrkMock.As<ICard>();
            orkMock.SetupProperty(card => card.Damage, 0);
            var ork = orkMock.Object;
            var boostEffect = new FiftyFifty();
            
            boostEffect.Apply(ork);
            boostEffect.Drop(ork);
            var result = ork.Damage;
            
            Assert.AreEqual(0, result);
        }
    }
}