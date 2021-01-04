using System.Linq;
using Moq;
using MTCG.Battles.Logging;
using MTCG.Cards.Basis.Monster;
using MTCG.Cards.Basis.Spell;
using MTCG.Cards.DamageUtil;
using MTCG.Cards.Effects.Types.DamageModifier;
using MTCG.Cards.Factory;
using MTCG.Cards.Specialities.Types.Destruction;
using MTCG.Cards.Specialities.Types.Miss;
using NUnit.Framework;

namespace MTCG_Test.Unit
{
    public class CardFactoryTest
    {
        private IPlayerLog log = null!;

        [OneTimeSetUp]
        public void Setup() => log = new Mock<IPlayerLog>().Object;
        
        [Test, TestCase(TestName = "Print RegularSpell", Description =
             "Print RegularSpell which does not pierce Krakens"
         )]
        public void PrintRegularSpell()
        {
            var regularSpell = CardFactory.Print("RegularSpell", 10, log);
            
            Assert.NotNull(regularSpell);
            Assert.IsTrue(regularSpell!.Type == DamageType.Normal);
            Assert.IsTrue(regularSpell is ISpellCard);
            // Check speciality in IEnumerable
            // See: https://stackoverflow.com/a/8216904/12347616
            Assert.IsTrue(regularSpell.Specialities.OfType<MissKrakenBecauseOfImmunity>().Any());
        }
        
        [Test, TestCase(TestName = "Print FireSpell", Description =
             "Print FireSpell which does not pierce Krakens"
         )]
        public void PrintFireSpell()
        {
            var fireSpell = CardFactory.Print("FireSpell", 10, log);
            
            Assert.NotNull(fireSpell);
            Assert.IsTrue(fireSpell!.Type == DamageType.Fire);
            Assert.IsTrue(fireSpell is ISpellCard);
            Assert.IsTrue(fireSpell.Specialities.OfType<MissKrakenBecauseOfImmunity>().Any());
        }
        
        [Test, TestCase(TestName = "Print WaterSpell", Description =
             "Print WaterSpell which does not pierce Krakens but drowns Knights"
         )]
        public void PrintWaterSpell()
        {
            var waterSpell = CardFactory.Print("WaterSpell", 10, log);
            
            Assert.NotNull(waterSpell);
            Assert.IsTrue(waterSpell!.Type == DamageType.Water);
            Assert.IsTrue(waterSpell is ISpellCard);
            Assert.IsTrue(waterSpell.Specialities.OfType<MissKrakenBecauseOfImmunity>().Any());
            Assert.IsTrue(waterSpell.Specialities.OfType<DrownKnight>().Any());
        }
        
        
        [Test, TestCase(TestName = "Print RegularGoblin", Description =
             "Print RegularGoblin which is afraid of Dragons"
         )]
        public void PrintRegularGoblin()
        {
            var regularGoblin = CardFactory.Print("RegularGoblin", 10, log);
            
            Assert.NotNull(regularGoblin);
            Assert.IsTrue(regularGoblin!.Type == DamageType.Normal);
            Assert.IsTrue(regularGoblin is IMonsterCard);
            Assert.IsTrue((regularGoblin as IMonsterCard)!.MonsterType == MonsterType.Goblin);
            Assert.IsTrue(regularGoblin.Specialities.OfType<AfraidFromDragons>().Any());
        }
        
        [Test, TestCase(TestName = "Print FireGoblin", Description =
             "Print FireGoblin which is afraid of Dragons"
         )]
        public void PrintFireGoblin()
        {
            var fireGoblin = CardFactory.Print("FireGoblin", 10, log);
            
            Assert.NotNull(fireGoblin);
            Assert.IsTrue(fireGoblin!.Type == DamageType.Fire);
            Assert.IsTrue(fireGoblin is IMonsterCard);
            Assert.IsTrue((fireGoblin as IMonsterCard)!.MonsterType == MonsterType.Goblin);
            Assert.IsTrue(fireGoblin.Specialities.OfType<AfraidFromDragons>().Any());
        }
        
        [Test, TestCase(TestName = "Print WaterGoblin", Description =
             "Print WaterGoblin which is afraid of Dragons"
         )]
        public void PrintWaterGoblin()
        {
            var waterGoblin = CardFactory.Print("WaterGoblin", 10, log);
            
            Assert.NotNull(waterGoblin);
            Assert.IsTrue(waterGoblin!.Type == DamageType.Water);
            Assert.IsTrue(waterGoblin is IMonsterCard);
            Assert.IsTrue((waterGoblin as IMonsterCard)!.MonsterType == MonsterType.Goblin);
            Assert.IsTrue(waterGoblin.Specialities.OfType<AfraidFromDragons>().Any());
        }
        
        [Test, TestCase(TestName = "Print RegularTroll", Description =
             "Print RegularTroll"
         )]
        public void PrintRegularTroll()
        {
            var regularTroll = CardFactory.Print("RegularTroll", 10, log);
            
            Assert.NotNull(regularTroll);
            Assert.IsTrue(regularTroll!.Type == DamageType.Normal);
            Assert.IsTrue(regularTroll is IMonsterCard);
            Assert.IsTrue((regularTroll as IMonsterCard)!.MonsterType == MonsterType.Troll);
        }
        
        [Test, TestCase(TestName = "Print FireTroll", Description =
             "Print FireTroll"
         )]
        public void PrintFireTroll()
        {
            var fireTroll = CardFactory.Print("FireTroll", 10, log);
            
            Assert.NotNull(fireTroll);
            Assert.IsTrue(fireTroll!.Type == DamageType.Fire);
            Assert.IsTrue(fireTroll is IMonsterCard);
            Assert.IsTrue((fireTroll as IMonsterCard)!.MonsterType == MonsterType.Troll);
        }
        
        [Test, TestCase(TestName = "Print WaterTroll", Description =
             "Print WaterTroll"
         )]
        public void PrintWaterTroll()
        {
            var waterTroll = CardFactory.Print("WaterTroll", 10, log);
            
            Assert.NotNull(waterTroll);
            Assert.IsTrue(waterTroll!.Type == DamageType.Water);
            Assert.IsTrue(waterTroll is IMonsterCard);
            Assert.IsTrue((waterTroll as IMonsterCard)!.MonsterType == MonsterType.Troll);
        }
        
        [Test, TestCase(TestName = "Print RegularDragon", Description =
             "Print RegularDragon which misses FireElves"
         )]
        public void PrintRegularDragon()
        {
            var regularDragon = CardFactory.Print("RegularDragon", 10, log);
            
            Assert.NotNull(regularDragon);
            Assert.IsTrue(regularDragon!.Type == DamageType.Normal);
            Assert.IsTrue(regularDragon is IMonsterCard);
            Assert.IsTrue((regularDragon as IMonsterCard)!.MonsterType == MonsterType.Dragon);
            Assert.IsTrue(regularDragon.Specialities.OfType<MissFireElfBecauseOfEvasion>().Any());
        }
        
        [Test, TestCase(TestName = "Print FireDragon", Description =
             "Print FireDragon which misses FireElves"
         )]
        public void PrintFireDragon()
        {
            var fireDragon = CardFactory.Print("FireDragon", 10, log);
            
            Assert.NotNull(fireDragon);
            Assert.IsTrue(fireDragon!.Type == DamageType.Fire);
            Assert.IsTrue(fireDragon is IMonsterCard);
            Assert.IsTrue((fireDragon as IMonsterCard)!.MonsterType == MonsterType.Dragon);
            Assert.IsTrue(fireDragon.Specialities.OfType<MissFireElfBecauseOfEvasion>().Any());
        }
        
        [Test, TestCase(TestName = "Print WaterDragon", Description =
             "Print WaterDragon which misses FireElves"
         )]
        public void PrintWaterDragon()
        {
            var waterDragon = CardFactory.Print("WaterDragon", 10, log);
            
            Assert.NotNull(waterDragon);
            Assert.IsTrue(waterDragon!.Type == DamageType.Water);
            Assert.IsTrue(waterDragon is IMonsterCard);
            Assert.IsTrue((waterDragon as IMonsterCard)!.MonsterType == MonsterType.Dragon);
            Assert.IsTrue(waterDragon.Specialities.OfType<MissFireElfBecauseOfEvasion>().Any());
        }

        [Test, TestCase(TestName = "Print RegularKnight", Description =
             "Print RegularKnight"
         )]
        public void PrintRegularKnight()
        {
            var regularKnight = CardFactory.Print("RegularKnight", 10, log);
            
            Assert.NotNull(regularKnight);
            Assert.IsTrue(regularKnight!.Type == DamageType.Normal);
            Assert.IsTrue(regularKnight is IMonsterCard);
            Assert.IsTrue((regularKnight as IMonsterCard)!.MonsterType == MonsterType.Knight);
        }
        
        [Test, TestCase(TestName = "Print FireKnight", Description =
             "Print FireKnight"
         )]
        public void PrintFireKnight()
        {
            var fireKnight = CardFactory.Print("FireKnight", 10, log);
            
            Assert.NotNull(fireKnight);
            Assert.IsTrue(fireKnight!.Type == DamageType.Fire);
            Assert.IsTrue(fireKnight is IMonsterCard);
            Assert.IsTrue((fireKnight as IMonsterCard)!.MonsterType == MonsterType.Knight);
        }
        
        [Test, TestCase(TestName = "Print WaterKnight", Description =
             "Print WaterKnight"
         )]
        public void PrintWaterKnight()
        {
            var waterKnight = CardFactory.Print("WaterKnight", 10, log);
            
            Assert.NotNull(waterKnight);
            Assert.IsTrue(waterKnight!.Type == DamageType.Water);
            Assert.IsTrue(waterKnight is IMonsterCard);
            Assert.IsTrue((waterKnight as IMonsterCard)!.MonsterType == MonsterType.Knight);
        }

        [Test, TestCase(TestName = "Print RegularKraken", Description =
             "Print RegularKraken"
         )]
        public void PrintRegularKraken()
        {
            var regularKraken = CardFactory.Print("RegularKraken", 10, log);
            
            Assert.NotNull(regularKraken);
            Assert.IsTrue(regularKraken!.Type == DamageType.Normal);
            Assert.IsTrue(regularKraken is IMonsterCard);
            Assert.IsTrue((regularKraken as IMonsterCard)!.MonsterType == MonsterType.Kraken);
        }
        
        [Test, TestCase(TestName = "Print FireKraken", Description =
             "Print FireKraken"
         )]
        public void PrintFireKraken()
        {
            var fireKraken = CardFactory.Print("FireKraken", 10, log);
            
            Assert.NotNull(fireKraken);
            Assert.IsTrue(fireKraken!.Type == DamageType.Fire);
            Assert.IsTrue(fireKraken is IMonsterCard);
            Assert.IsTrue((fireKraken as IMonsterCard)!.MonsterType == MonsterType.Kraken);
        }
        
        [Test, TestCase(TestName = "Print WaterKraken", Description =
             "Print WaterKraken"
         )]
        public void PrintWaterKraken()
        {
            var waterKraken = CardFactory.Print("WaterKraken", 10, log);
            
            Assert.NotNull(waterKraken);
            Assert.IsTrue(waterKraken!.Type == DamageType.Water);
            Assert.IsTrue(waterKraken is IMonsterCard);
            Assert.IsTrue((waterKraken as IMonsterCard)!.MonsterType == MonsterType.Kraken);
        }
        
        [Test, TestCase(TestName = "Print RegularElf", Description =
             "Print RegularElf"
         )]
        public void PrintRegularElf()
        {
            var regularElf = CardFactory.Print("RegularElf", 10, log);
            
            Assert.NotNull(regularElf);
            Assert.IsTrue(regularElf!.Type == DamageType.Normal);
            Assert.IsTrue(regularElf is IMonsterCard);
            Assert.IsTrue((regularElf as IMonsterCard)!.MonsterType == MonsterType.Elf);
        }
        
        [Test, TestCase(TestName = "Print FireElf", Description =
             "Print FireElf"
         )]
        public void PrintFireElf()
        {
            var fireElf = CardFactory.Print("FireElf", 10, log);
            
            Assert.NotNull(fireElf);
            Assert.IsTrue(fireElf!.Type == DamageType.Fire);
            Assert.IsTrue(fireElf is IMonsterCard);
            Assert.IsTrue((fireElf as IMonsterCard)!.MonsterType == MonsterType.Elf);
        }
        
        [Test, TestCase(TestName = "Print WaterElf", Description =
             "Print WaterElf"
         )]
        public void PrintWaterElf()
        {
            var waterElf = CardFactory.Print("WaterElf", 10, log);
            
            Assert.NotNull(waterElf);
            Assert.IsTrue(waterElf!.Type == DamageType.Water);
            Assert.IsTrue(waterElf is IMonsterCard);
            Assert.IsTrue((waterElf as IMonsterCard)!.MonsterType == MonsterType.Elf);
        }
        
        [Test, TestCase(TestName = "Print RegularWizard", Description =
             "Print RegularWizard"
         )]
        public void PrintRegularWizard()
        {
            var regularWizard = CardFactory.Print("RegularWizard", 10, log);
            
            Assert.NotNull(regularWizard);
            Assert.IsTrue(regularWizard!.Type == DamageType.Normal);
            Assert.IsTrue(regularWizard is IMonsterCard);
            Assert.IsTrue((regularWizard as IMonsterCard)!.MonsterType == MonsterType.Wizard);
        }
        
        [Test, TestCase(TestName = "Print FireWizard", Description =
             "Print FireWizard"
         )]
        public void PrintFireWizard()
        {
            var fireWizard = CardFactory.Print("FireWizard", 10, log);
            
            Assert.NotNull(fireWizard);
            Assert.IsTrue(fireWizard!.Type == DamageType.Fire);
            Assert.IsTrue(fireWizard is IMonsterCard);
            Assert.IsTrue((fireWizard as IMonsterCard)!.MonsterType == MonsterType.Wizard);
        }
        
        [Test, TestCase(TestName = "Print WaterWizard", Description =
             "Print WaterWizard"
         )]
        public void PrintWaterWizard()
        {
            var waterWizard = CardFactory.Print("WaterWizard", 10, log);
            
            Assert.NotNull(waterWizard);
            Assert.IsTrue(waterWizard!.Type == DamageType.Water);
            Assert.IsTrue(waterWizard is IMonsterCard);
            Assert.IsTrue((waterWizard as IMonsterCard)!.MonsterType == MonsterType.Wizard);
        }
        
        [Test, TestCase(TestName = "Print RegularOrk", Description =
             "Print RegularOrk which misses Wizards because of Control and " +
             "has the \"FiftyFifty\" Effect"
         )]
        public void PrintRegularOrk()
        {
            var regularOrk = CardFactory.Print("RegularOrk", 10, log);
            
            Assert.NotNull(regularOrk);
            Assert.IsTrue(regularOrk!.Type == DamageType.Normal);
            Assert.IsTrue(regularOrk is IMonsterCard);
            Assert.IsTrue((regularOrk as IMonsterCard)!.MonsterType == MonsterType.Ork);
            Assert.IsTrue(regularOrk.Specialities.OfType<ControllableByWizard>().Any());
            Assert.IsTrue((regularOrk as IMonsterCard)!.Effects.OfType<FiftyFifty>().Any());
        }
        
        [Test, TestCase(TestName = "Print FireOrk", Description =
             "Print FireOrk which misses Wizards because of Control and " +
             "has the \"FiftyFifty\" Effect"
         )]
        public void PrintFireOrk()
        {
            var fireOrk = CardFactory.Print("FireOrk", 10, log);
            
            Assert.NotNull(fireOrk);
            Assert.IsTrue(fireOrk!.Type == DamageType.Fire);
            Assert.IsTrue(fireOrk is IMonsterCard);
            Assert.IsTrue((fireOrk as IMonsterCard)!.MonsterType == MonsterType.Ork);
            Assert.IsTrue(fireOrk.Specialities.OfType<ControllableByWizard>().Any());
            Assert.IsTrue((fireOrk as IMonsterCard)!.Effects.OfType<FiftyFifty>().Any());
        }
        
        [Test, TestCase(TestName = "Print WaterOrk", Description =
             "Print WaterOrk which misses Wizards because of Control and " +
             "has the \"FiftyFifty\" Effect"
         )]
        public void PrintWaterOrk()
        {
            var waterOrk = CardFactory.Print("WaterOrk", 10, log);
            
            Assert.NotNull(waterOrk);
            Assert.IsTrue(waterOrk!.Type == DamageType.Water);
            Assert.IsTrue(waterOrk is IMonsterCard);
            Assert.IsTrue((waterOrk as IMonsterCard)!.MonsterType == MonsterType.Ork);
            Assert.IsTrue(waterOrk.Specialities.OfType<ControllableByWizard>().Any());
            Assert.IsTrue((waterOrk as IMonsterCard)!.Effects.OfType<FiftyFifty>().Any());
        }
        
        [Test, TestCase(TestName = "Print RegularSpaceMarine", Description =
             "Print RegularSpaceMarine which has a \"Boost\" Effect"
         )]
        public void PrintRegularSpaceMarine()
        {
            var regularSpaceMarine = CardFactory.Print("RegularSpaceMarine", 10, log);
            
            Assert.NotNull(regularSpaceMarine);
            Assert.IsTrue(regularSpaceMarine!.Type == DamageType.Normal);
            Assert.IsTrue(regularSpaceMarine is IMonsterCard);
            Assert.IsTrue((regularSpaceMarine as IMonsterCard)!.MonsterType == MonsterType.SpaceMarine);
            Assert.IsTrue((regularSpaceMarine as IMonsterCard)!.Effects.OfType<Boost>().Any());
        }
        
        [Test, TestCase(TestName = "Print FireSpaceMarine", Description =
             "Print FireSpaceMarine which has a \"Boost\" Effect"
         )]
        public void PrintFireSpaceMarine()
        {
            var fireSpaceMarine = CardFactory.Print("FireSpaceMarine", 10, log);
            
            Assert.NotNull(fireSpaceMarine);
            Assert.IsTrue(fireSpaceMarine!.Type == DamageType.Fire);
            Assert.IsTrue(fireSpaceMarine is IMonsterCard);
            Assert.IsTrue((fireSpaceMarine as IMonsterCard)!.MonsterType == MonsterType.SpaceMarine);
            Assert.IsTrue((fireSpaceMarine as IMonsterCard)!.Effects.OfType<Boost>().Any());
        }
        
        [Test, TestCase(TestName = "Print WaterSpaceMarine", Description =
             "Print WaterSpaceMarine which has a \"Boost\" Effect"
         )]
        public void PrintWaterSpaceMarine()
        {
            var waterSpaceMarine = CardFactory.Print("WaterSpaceMarine", 10, log);
            
            Assert.NotNull(waterSpaceMarine);
            Assert.IsTrue(waterSpaceMarine!.Type == DamageType.Water);
            Assert.IsTrue(waterSpaceMarine is IMonsterCard);
            Assert.IsTrue((waterSpaceMarine as IMonsterCard)!.MonsterType == MonsterType.SpaceMarine);
            Assert.IsTrue((waterSpaceMarine as IMonsterCard)!.Effects.OfType<Boost>().Any());
        }

        [Test, TestCase(TestName = "Print default Spell Card", Description =
             "Print default Spell Card when no Damage Type is given"
         )]
        public void PrintDefaultSpell()
        {
            var spell = CardFactory.Print("Spell", 10, log);
            
            Assert.IsTrue(spell!.Type == DamageType.Normal);
        }


        [Test, TestCase(TestName = "Print default Monster Cards", Description =
             "Print default Monster Cards when no Damage Type is given"
         )]
        public void PrintDefaultMonsters()
        {
            // Default to Normal
            var goblin = CardFactory.Print("Goblin", 10, log);
            var troll = CardFactory.Print("Troll", 10, log);
            var elf = CardFactory.Print("Elf", 10, log);
            var wizard = CardFactory.Print("Wizard", 10, log);
            var ork = CardFactory.Print("Ork", 10, log);
            var knight = CardFactory.Print("Knight", 10, log);
            var spaceMarine = CardFactory.Print("SpaceMarine", 10, log);
            // Default NOT to Normal
            var dragon = CardFactory.Print("Dragon", 10, log);
            var kraken = CardFactory.Print("Kraken", 10, log);
            
            Assert.IsTrue(goblin!.Type == DamageType.Normal);
            Assert.IsTrue(troll!.Type == DamageType.Normal);
            Assert.IsTrue(elf!.Type == DamageType.Normal);
            Assert.IsTrue(wizard!.Type == DamageType.Normal);
            Assert.IsTrue(ork!.Type == DamageType.Normal);
            Assert.IsTrue(knight!.Type == DamageType.Normal);
            Assert.IsTrue(spaceMarine!.Type == DamageType.Normal);
            
            Assert.IsTrue(dragon!.Type == DamageType.Fire);
            Assert.IsTrue(kraken!.Type == DamageType.Water);
        }
        

        [Test, TestCase(TestName = "Check invalid Card behaviour", Description =
             "Check invalid Card behaviour with unimplemented Card"
         )]
        public void InvalidCardName()
        {
            var empty = CardFactory.Print("DoesNotExist", 10, log);
            
            Assert.IsNull(empty);
        }
    }
}