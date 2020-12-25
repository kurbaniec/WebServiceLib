using System;
using System.Text.RegularExpressions;

namespace MTCG.Cards.Basis.Monster
{
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

    // Extension class for enum
    // See: https://stackoverflow.com/a/5985710/12347616
    static class MonsterTypeMethods
    {
        public static string GetString(this MonsterType mt)
        {
            // Get name of MonsterType as string
            // See: https://stackoverflow.com/a/31742218/12347616
            var name = nameof(mt);
            // Prettify it (e.g. SpaceMarine => Space Marine
            // See: https://stackoverflow.com/a/36147193/12347616
            var prettified = Regex.Split(name, @"(?<!^)(?=[A-Z])").ToString();
            return prettified ?? name;
        }
        
        public static MonsterType? GetType(string type)
        {
            if (Enum.TryParse(type, out MonsterType enumType)) return enumType;
            return null;
        }
    }
}