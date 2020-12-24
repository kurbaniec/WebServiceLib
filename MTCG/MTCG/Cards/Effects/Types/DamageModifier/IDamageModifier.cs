using MTCG.Cards.Basis;

namespace MTCG.Cards.Effects.Types.DamageModifier
{
    public interface IDamageModifier
    {
        void AddDamage(ICard self, long value)
        {
            if (self.Damage + value > 0) self.Damage += value;
            else self.Damage = 0;
        }
    }
}