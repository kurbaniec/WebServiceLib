using Moq;
using MTCG.Battles.Logging;
using MTCG.Cards.Basis;
using MTCG.Cards.Basis.Monster;
using MTCG.Cards.Basis.Spell;
using MTCG.Cards.DamageUtil;
using MTCG.Cards.Specialities;
using MTCG.Cards.Specialities.Types.Destruction;
using MTCG.Cards.Specialities.Types.Miss;
using NUnit.Framework;

namespace MTCG_Test.Unit
{
    public class SpecialityTest
    {
        private IPlayerLog log = null!;

        [OneTimeSetUp]
        public void Setup() => log = new Mock<IPlayerLog>().Object;
        
        [Test, TestCase(TestName = "Goblins are too afraid of Dragons to attack", Description =
             "Test speciality \"Goblins are too afraid of Dragons to attack\" " +
             "with the help of mocks"
         )]
        public void GoblinAfraidDragons()
        {
            // Mock of multiple interfaces
            // See: https://stackoverflow.com/a/28145338/12347616
            var tmpGoblinMock = new Mock<IMonsterCard>();
            tmpGoblinMock.Setup(card => card.MonsterType).Returns(MonsterType.Goblin);
            var goblinMock = tmpGoblinMock.As<ICard>();
            goblinMock.SetupProperty(card => card.Log, log);
            var goblin = goblinMock.Object;
            goblin.Log = log;
            var dragonMock = new Mock<IMonsterCard>();
            dragonMock.Setup(card => card.MonsterType).Returns(MonsterType.Dragon);
            var dragon = dragonMock.As<ICard>().Object;
            var damage = Damage.Normal(10);
            ISpeciality speciality = new AfraidFromDragons();
            
            speciality.Apply(goblin, dragon, damage);
            var result = damage.Value;
            
            Assert.AreEqual(0, result);
        }
        
        [Test, TestCase(TestName = "Wizard can control Orks so they are not able to damage them", Description =
             "Test speciality \"Wizard can control Orks so they are not able to damage them\" " +
             "with the help of mocks"
         )]
        public void WizzardControlOrks()
        {
            var tmpOrkMock = new Mock<IMonsterCard>();
            tmpOrkMock.Setup(card => card.MonsterType).Returns(MonsterType.Ork);
            var orkMock = tmpOrkMock.As<ICard>();
            orkMock.SetupProperty(card => card.Log, log);
            var ork = orkMock.Object;
            var wizardMock = new Mock<IMonsterCard>();
            wizardMock.Setup(card => card.MonsterType).Returns(MonsterType.Wizard);
            var wizard = wizardMock.As<ICard>().Object;
            var damage = Damage.Normal(10);
            ISpeciality speciality = new ControllableByWizard();
            
            speciality.Apply(ork, wizard, damage);
            var result = damage.Value;
            
            Assert.AreEqual(0, result);
        }
        
        [Test, TestCase(TestName = "The armor of Knights is so heavy that WaterSpells make them drown them instantly", Description =
             "Test speciality \"The armor of Knights is so heavy that WaterSpells make them drown them instantly\" " +
             "with the help of mocks"
         )]
        public void KnightDrownThroughWaterSpell()
        {
            var tmpWaterSpellMock = new Mock<ISpellCard>();
            var waterSpellMock = tmpWaterSpellMock.As<ICard>();
            waterSpellMock.Setup(card => card.Type).Returns(DamageType.Water);
            waterSpellMock.SetupProperty(card => card.Log, log);
            var waterSpell = waterSpellMock.Object;
            var knightMock = new Mock<IMonsterCard>();
            knightMock.Setup(card => card.MonsterType).Returns(MonsterType.Knight);
            var knight = knightMock.As<ICard>().Object;
            var damage = Damage.Normal(10);
            ISpeciality speciality = new DrownKnight();
            
            speciality.Apply(waterSpell, knight, damage);
            var result = damage.IsInfty;
            
            Assert.IsTrue(result);
        }
        
        [Test, TestCase(TestName = "The Kraken is immune against spells", Description =
             "Test speciality \"The Kraken is immune against spells\" " +
             "with the help of mocks"
         )]
        public void KrakenImmuneToSpells()
        {
            var tmpSpellMock = new Mock<ISpellCard>();
            var spellMock = tmpSpellMock.As<ICard>();
            spellMock.SetupProperty(card => card.Log, log);
            var spell = spellMock.Object;
            var krakenMock = new Mock<IMonsterCard>();
            krakenMock.Setup(card => card.MonsterType).Returns(MonsterType.Kraken);
            var kraken = krakenMock.As<ICard>().Object;
            var damage = Damage.Normal(10);
            ISpeciality speciality = new MissKrakenBecauseOfImmunity();
            
            speciality.Apply(spell, kraken, damage);
            var result = damage.Value;
            
            Assert.AreEqual(0, result);
        }
        
        [Test, TestCase(TestName = "The FireElves know Dragons since they were little and can evade their attacks", Description =
             "Test speciality \"The FireElves know Dragons since they were little and can evade their attacks\" " +
             "with the help of mocks"
         )]
        public void FireElfEvadesDragon()
        {
            var tmpDragonMock = new Mock<IMonsterCard>();
            tmpDragonMock.Setup(card => card.MonsterType).Returns(MonsterType.Dragon);
            var dragonMock = tmpDragonMock.As<ICard>();
            dragonMock.SetupProperty(card => card.Log, log);
            var dragon = dragonMock.Object;
            var tmpFireElfMock = new Mock<IMonsterCard>();
            tmpFireElfMock.Setup(card => card.MonsterType).Returns(MonsterType.Elf);
            var fireElfMock = tmpFireElfMock.As<ICard>();
            fireElfMock.Setup(card => card.Type).Returns(DamageType.Fire);
            var fireElf = fireElfMock.Object;
            var damage = Damage.Normal(10);
            ISpeciality speciality = new MissFireElfBecauseOfEvasion();
            
            speciality.Apply(dragon, fireElf, damage);
            var result = damage.Value;
            
            Assert.AreEqual(0, result);
        }
    }
}