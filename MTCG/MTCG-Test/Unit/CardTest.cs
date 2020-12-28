using System.Collections.Generic;
using Moq;
using MTCG.Battles;
using MTCG.Battles.Logging;
using MTCG.Cards.Basis;
using MTCG.Cards.Basis.Monster;
using MTCG.Cards.Basis.Spell;
using MTCG.Cards.DamageUtil;
using MTCG.Cards.Effects;
using MTCG.Cards.Specialities;
using NUnit.Framework;

namespace MTCG_Test.Unit
{
    public class CardTest
    {
        
        private IPlayerLog log = null!;

        [OneTimeSetUp]
        public void Setup() => log = new Mock<IPlayerLog>().Object;
        
        
        [Test, TestCase(TestName = "Card Damage calculation in Monster fight", Description =
             "Calculate Damage for a round, in which both cards are Monsters. " +
             "No Specialities or Effects are in use."
        )]
        public void CalculateDamageMonsters()
        {
            ICard waterGoblin = new MonsterCard(
                10, DamageType.Water, MonsterType.Goblin, new List<ISpeciality>(),
                new List<IEffect>(), log 
            );
            ICard fireTroll = new MonsterCard(
                15, DamageType.Fire, MonsterType.Troll, new List<ISpeciality>(),
                new List<IEffect>(), log 
            );

            var waterGoblinResult = waterGoblin.CalculateDamage(fireTroll).Value;
            var fireTrollResult = fireTroll.CalculateDamage(waterGoblin).Value;
            
            Assert.AreEqual(10, waterGoblinResult);
            Assert.AreEqual(15, fireTrollResult);
        }

        [Test, TestCase(TestName = "Card Damage calculation in Spell fight", Description =
             "Calculate Damage for a round, in which both cards are Spells. " +
             "No Specialities are in use."
         )]
        public void CalculateDamageSpellsA()
        {
            ICard fireSpell = new SpellCard(
                10, DamageType.Fire, new List<ISpeciality>(), log
            );
            ICard waterSpell = new SpellCard(
                20, DamageType.Water, new List<ISpeciality>(), log
            );

            var fireSpellResult = fireSpell.CalculateDamage(waterSpell).Value;
            var waterSpellResult = waterSpell.CalculateDamage(fireSpell).Value;
            
            Assert.AreEqual(5, fireSpellResult);
            Assert.AreEqual(40, waterSpellResult);
        }
        
        [Test, TestCase(TestName = "Alternative Card Damage calculation in Spell fight", Description =
             "Calculate Damage for a round, in which both cards are Spells. " +
             "No Specialities are in use."
         )]
        public void CalculateDamageSpellsB()
        {
            ICard fireSpell = new SpellCard(
                20, DamageType.Fire, new List<ISpeciality>(), log
            );
            ICard waterSpell = new SpellCard(
                5, DamageType.Water, new List<ISpeciality>(), log
            );

            var fireSpellResult = fireSpell.CalculateDamage(waterSpell).Value;
            var waterSpellResult = waterSpell.CalculateDamage(fireSpell).Value;
            
            Assert.AreEqual(10, fireSpellResult);
            Assert.AreEqual(10, waterSpellResult);
        }
        
        [Test, TestCase(TestName = "Alternative Card Damage calculation in Spell fight", Description =
             "Calculate Damage for a round, in which both cards are Spells. " +
             "No Specialities are in use."
         )]
        public void CalculateDamageSpellsC()
        {
            ICard fireSpell = new SpellCard(
                90, DamageType.Fire, new List<ISpeciality>(), log
            );
            ICard waterSpell = new SpellCard(
                5, DamageType.Water, new List<ISpeciality>(), log
            );

            var fireSpellResult = fireSpell.CalculateDamage(waterSpell).Value;
            var waterSpellResult = waterSpell.CalculateDamage(fireSpell).Value;
            
            Assert.AreEqual(45, fireSpellResult);
            Assert.AreEqual(10, waterSpellResult);
        }
        
        [Test, TestCase(TestName = "Card Damage calculation in Mixed fight", Description =
             "Calculate Damage for a round, in which one Card is a Monster, one a Spell " +
             "No Specialities or Effects are in use."
         )]
        public void CalculateDamageMixedA()
        {
            ICard fireSpell = new SpellCard(
                10, DamageType.Fire, new List<ISpeciality>(), log
            );
            ICard waterGoblin = new MonsterCard(
                10, DamageType.Water, MonsterType.Goblin, new List<ISpeciality>(),
                new List<IEffect>(), log 
            );

            var fireSpellResult = fireSpell.CalculateDamage(waterGoblin).Value;
            var waterGoblinResult = waterGoblin.CalculateDamage(fireSpell).Value;
            
            Assert.AreEqual(5, fireSpellResult);
            Assert.AreEqual(20, waterGoblinResult);
        }
        
        [Test, TestCase(TestName = "Alternative Card Damage calculation in Mixed fight", Description =
             "Calculate Damage for a round, in which one Card is a Monster, one a Spell " +
             "No Specialities or Effects are in use."
         )]
        public void CalculateDamageMixedB()
        {
            ICard waterSpell = new SpellCard(
                10, DamageType.Water, new List<ISpeciality>(), log
            );
            ICard waterGoblin = new MonsterCard(
                10, DamageType.Water, MonsterType.Goblin, new List<ISpeciality>(),
                new List<IEffect>(), log 
            );

            var waterSpellResult = waterSpell.CalculateDamage(waterGoblin).Value;
            var waterGoblinResult = waterGoblin.CalculateDamage(waterSpell).Value;
            
            Assert.AreEqual(10, waterSpellResult);
            Assert.AreEqual(10, waterGoblinResult);
        }
        
        [Test, TestCase(TestName = "Alternative Card Damage calculation in Mixed fight", Description =
             "Calculate Damage for a round, in which one Card is a Monster, one a Spell " +
             "No Specialities or Effects are in use."
         )]
        public void CalculateDamageMixedC()
        {
            ICard regularSpell = new SpellCard(
                10, DamageType.Normal, new List<ISpeciality>(), log
            );
            ICard waterGoblin = new MonsterCard(
                10, DamageType.Water, MonsterType.Goblin, new List<ISpeciality>(),
                new List<IEffect>(), log 
            );

            var regularSpellResult = regularSpell.CalculateDamage(waterGoblin).Value;
            var waterGoblinResult = waterGoblin.CalculateDamage(regularSpell).Value;
            
            Assert.AreEqual(20, regularSpellResult);
            Assert.AreEqual(5, waterGoblinResult);
        }
        
        [Test, TestCase(TestName = "Alternative Card Damage calculation in Mixed fight", Description =
             "Calculate Damage for a round, in which one Card is a Monster, one a Spell " +
             "No Specialities or Effects are in use."
         )]
        public void CalculateDamageMixedD()
        {
            ICard regularSpell = new SpellCard(
                10, DamageType.Normal, new List<ISpeciality>(), log
            );
            ICard regularKnight= new MonsterCard(
                15, DamageType.Normal, MonsterType.Goblin, new List<ISpeciality>(),
                new List<IEffect>(), log 
            );

            var regularSpellResult = regularSpell.CalculateDamage(regularKnight).Value;
            var regularKnightResult = regularKnight.CalculateDamage(regularSpell).Value;
            
            Assert.AreEqual(10, regularSpellResult);
            Assert.AreEqual(15, regularKnightResult);
        }
        
    }
}