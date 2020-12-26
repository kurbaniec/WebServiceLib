using System;

namespace MTCG.Cards.DamageUtil
{
    public enum DamageType
    {
        Normal,
        Fire,
        Water
    }
    
    static class DamageTypeMethods
    {
        public static string GetString(this DamageType dt)
        {
            var name = nameof(dt);
            if (name == "Normal") name = "Regular";
            return name;
        }

        public static DamageType? GetType(string type)
        {
            if (type == "Regular") return DamageType.Normal;
            if (Enum.TryParse(type, out DamageType enumType)) return enumType;
            return null;
        }
    }
}