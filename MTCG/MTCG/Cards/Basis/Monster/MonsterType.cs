using System;
using System.Text.RegularExpressions;
using MTCG.Cards.DamageUtil;

namespace MTCG.Cards.Basis.Monster
{
    /// <summary>
    /// Enum that lists all possible monster types.
    /// </summary>
    public enum MonsterType
    {
        Goblin,
        Troll,
        Dragon,
        Wizard,
        Ork,
        Knight,
        Kraken,
        Elf,
        SpaceMarine,
    }
    
    /// <summary>
    /// Utility class for <c>MonsterType</c>.
    /// </summary>
    // Extension class for enum
    // See: https://stackoverflow.com/a/5985710/12347616
    static class MonsterTypeMethods
    {
        /// <summary>
        /// Get prettified string representation of a <c>MonsterType</c>.
        /// </summary>
        /// <param name="mt"></param>
        /// <returns>
        /// String representation of a <c>MonsterType</c>.
        /// </returns>
        public static string GetString(this MonsterType mt)
        {
            // Get name of MonsterType as string
            // See: https://stackoverflow.com/a/31742218/12347616
            // And: https://stackoverflow.com/a/309339/12347616
            var name = mt.ToString();
            // Prettify it (e.g. SpaceMarine => Space Marine
            // See: https://stackoverflow.com/a/36147193/12347616
            // And: https://stackoverflow.com/a/37262742/12347616
            var prettified = string.Join(" ", Regex.Split(name, @"(?<!^)(?=[A-Z])"));
            return prettified ?? name;
        }

        /// <summary>
        /// Get default <c>DamageType</c> of a <c>MonsterType</c>.
        /// </summary>
        /// <param name="mt"></param>
        /// <returns>
        /// Default <c>DamageType</c> of a <c>MonsterType</c> as string
        /// </returns>
        public static string GetDefaultDamageType(this MonsterType mt)
        {
            string damageType;
            switch (mt)
            {
                case MonsterType.Goblin:
                case MonsterType.Troll:
                case MonsterType.Elf:
                case MonsterType.Wizard:
                case MonsterType.Ork:
                case MonsterType.Knight:
                case MonsterType.SpaceMarine:
                    damageType = DamageType.Normal.ToString();
                    break;
                case MonsterType.Dragon:
                    damageType = DamageType.Fire.ToString();
                    break;
                case MonsterType.Kraken:
                    damageType = DamageType.Water.ToString();
                    break;
                default:
                    damageType = DamageType.Normal.ToString();
                    break;
            }

            return damageType;
        }
        
        /// <summary>
        /// Get <c>MonsterType</c> from a given string.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>
        /// Concrete <c>MonsterType</c> enum value or null.
        /// </returns>
        public static MonsterType? GetType(string type)
        {
            if (Enum.TryParse(type, out MonsterType enumType)) return enumType;
            return null;
        }
    }
}