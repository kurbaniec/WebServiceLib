using MTCG.Cards.Basis;

namespace MTCG.Cards.Effects.Types.DamageModifier
{
    /// <summary>
    /// Concrete <c>IEffect</c> and <c>IDamageModifier</c> that
    /// boosts damage permanently by 1 when applied.
    /// Used by Space Marines.
    /// </summary>
    public class Boost : IEffect, IDamageModifier
    {
        private long damageAdded = 0;
        
        /// <summary>
        /// Apply Effect on monster card.
        /// </summary>
        /// <param name="self"></param>
        public void Apply(ICard self)
        {
            damageAdded++;
            (this as IDamageModifier).AddDamage(self, 1);
            self.Log.AddEffectInfo($"{self}'s BOOST activated, base damage increased by 1");
        }

        /// <summary>
        /// Drop Effect from monster card.
        /// </summary>
        /// <param name="self"></param>
        public void Drop(ICard self)
        {
            (this as IDamageModifier).AddDamage(self, -damageAdded);
        }
    }
}