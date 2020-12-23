using MTCG.Cards.DamageUtil;

namespace MTCG.Cards.DamageUtil
{
    public class WeaponTriangle
    {
        /// <summary>
        /// Holy weapon triangle:
        /// • water -> fire
        /// • fire -> normal
        /// • normal -> water
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        /// <param name="damage"></param>
        /// <returns>
        /// Damage based on effectiveness.
        /// </returns>
        public static void EffectiveDamage(DamageType self, DamageType other, IDamage damage)
        {
            if (self == other) return;
            if (self == DamageType.Water)
            {
                if (other == DamageType.Fire) damage.Multiply(2);
                else damage.Divide(2);
            }
            else if (self == DamageType.Fire)
            {
                if (other == DamageType.Normal) damage.Multiply(2);
                else damage.Divide(2);
            }
            else
            {
                if (other == DamageType.Water) damage.Multiply(2);
                else damage.Divide(2);
            }
        }
    }
}