using MTCG.Cards.Basis;

namespace MTCG.Cards.Effects.Types.DamageModifier
{
    /// <summary>
    /// Interface that describes a type of Effect that modifies base damage.
    /// </summary>
    public interface IDamageModifier
    {
        /// <summary>
        /// Add damage value to the base damage of a card.
        /// Note that the base damage cannot be less than 0.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        void AddDamage(ICard self, long value)
        {
            if (self.Damage + value > 0) self.Damage += value;
            else self.Damage = 0;
        }
    }
}