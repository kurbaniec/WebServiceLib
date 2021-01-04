using System;

namespace MTCG.Cards.DamageUtil
{
    /// <summary>
    /// Enum that lists all possible element types.
    /// </summary>
    public enum DamageType
    {
        Normal,
        Fire,
        Water
    }
    
    /// <summary>
    /// Utility class for <c>DamageType</c>.
    /// </summary>
    static class DamageTypeMethods
    {
        /// <summary>
        /// Get string representation of a <c>DamageType</c>.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>
        /// String representation of a <c>DamageType</c>.
        /// </returns>
        public static string GetString(this DamageType dt)
        {
            var name = dt.ToString();
            if (name == "Normal") name = "Regular";
            return name;
        }

        /// <summary>
        /// Get <c>DamageType</c> from a given string.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>
        /// Concrete <c>DamageType</c> enum value or null.
        /// </returns>
        public static DamageType? GetType(string type)
        {
            if (type == "Regular") return DamageType.Normal;
            if (Enum.TryParse(type, out DamageType enumType)) return enumType;
            return null;
        }
    }
}