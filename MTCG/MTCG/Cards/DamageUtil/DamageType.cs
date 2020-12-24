namespace MTCG.Cards.DamageUtil
{
    public enum DamageType
    {
        Normal,
        Fire,
        Water
    }
    
    static class MonsterTypeMethods
    {
        public static string GetString(this DamageType dt)
        {
            var name = nameof(dt);
            if (name == "Normal") name = "Regular";
            return name;
        }
    }
}