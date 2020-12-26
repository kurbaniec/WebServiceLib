using System.Collections.Generic;
using System.Linq;
using Moq;
using MTCG.Battles;
using MTCG.Cards.Basis;
using MTCG.Cards.Basis.Monster;
using MTCG.Cards.Basis.Spell;
using MTCG.Cards.DamageUtil;
using MTCG.Cards.Effects;
using MTCG.Cards.Factory;
using MTCG.Cards.Specialities;
using MTCG.Cards.Specialities.Types.Destruction;
using MTCG.Cards.Specialities.Types.Miss;
using NUnit.Framework;

namespace MTCG_Test.Unit
{
    public class CardFactoryTest
    {
        private IBattleLog log = null!;

        [OneTimeSetUp]
        public void Setup() => log = new Mock<IBattleLog>().Object;
        
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