using System.Collections.Generic;
using MTCG.Battles;
using MTCG.Battles.Logging;
using MTCG.Cards.Basis;
using MTCG.Cards.Basis.Monster;
using MTCG.Cards.Basis.Spell;
using MTCG.Cards.DamageUtil;
using MTCG.Cards.Effects;
using MTCG.Cards.Effects.Types.DamageModifier;
using MTCG.Cards.Specialities;
using MTCG.Cards.Specialities.Types.Destruction;
using MTCG.Cards.Specialities.Types.Miss;

namespace MTCG.Cards.Factory
{
    public class CardFactory
    {
        public static ICard? Print(string fullCardName, double damage, IPlayerLog log)
        {
            fullCardName = InferType(fullCardName);
            var processedName = Split(fullCardName);
            if (processedName == null) return null;
            var procType = processedName.Value.Item1;
            var procName = processedName.Value.Item2;

            var type = DamageTypeMethods.GetType(procType);
            if (type == null) return null;

            // Build Spell Card
            if (procName.EndsWith("Spell"))
            {
                return PrintSpell(damage, (DamageType) type, log);
            }
            // Build Monster Card
            var monsterType = MonsterTypeMethods.GetType(procName);
            if (monsterType == null) return null;
            return PrintMonster(damage, (DamageType) type, (MonsterType) monsterType, log);
        }

        private static ICard PrintSpell(double damage, DamageType damageType, IPlayerLog log)
        {
            // All spells can't pierce Krakens
            var specialities = new List<ISpeciality>() { new MissKrakenBecauseOfImmunity() };
            switch (damageType)
            {
                case DamageType.Water:
                    // WaterSpells drown Knights
                    specialities.Add(new DrownKnight());
                    break;
                case DamageType.Normal:
                    break;
                case DamageType.Fire:
                    break;
            }
            ICard card = new SpellCard(damage, damageType, specialities, log);
            return card;
        }

        private static ICard PrintMonster(
            double damage, DamageType damageType, MonsterType monsterType, IPlayerLog log
        )
        {
            var specialities = new List<ISpeciality>();
            var effects = new List<IEffect>();
            switch (monsterType)
            {
                case MonsterType.Goblin:
                    specialities.Add(new AfraidFromDragons());
                    break;
                case MonsterType.Troll:
                    break;
                case MonsterType.Dragon:
                    specialities.Add(new MissFireElfBecauseOfEvasion());
                    break;
                case MonsterType.Wizard:
                    break;
                case MonsterType.Ork:
                    specialities.Add(new ControllableByWizard());
                    effects.Add(new FiftyFifty());
                    break;
                case MonsterType.Knight:
                    break;
                case MonsterType.Kraken:
                    break;
                case MonsterType.Elf:
                    break;
                case MonsterType.SpaceMarine:
                    effects.Add(new Boost());
                    break;
            }
            ICard card = new MonsterCard(damage, damageType, monsterType, specialities, effects, log);
            return card;
        }

        private static string InferType(string fullCardName)
        {
            if (fullCardName.StartsWith("Normal") || fullCardName.StartsWith("Regular") ||
                fullCardName.StartsWith("Fire") || fullCardName.StartsWith("Water")) return fullCardName;
            if (fullCardName.EndsWith("Spell")) fullCardName = "Regular" + fullCardName;
            // Pattern matching
            // See: https://stackoverflow.com/a/51449576/12347616
            else if (MonsterTypeMethods.GetType(fullCardName) is { } type)
            {
                fullCardName = type.GetDefaultDamageType() + fullCardName;
            }

            return fullCardName;
        }
        

        private static (string, string)? Split(string fullCardName)
        {
            for (var i = 1; i < fullCardName.Length; i++)
            {
                if (!char.IsUpper(fullCardName[i])) continue;
                var type = fullCardName.Substring(0, i);
                var name = fullCardName.Substring(i);
                return (type, name);
            }

            return null;
        }
    }
}