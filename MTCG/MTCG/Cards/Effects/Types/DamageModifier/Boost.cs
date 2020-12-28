using MTCG.Cards.Basis;

namespace MTCG.Cards.Effects.Types.DamageModifier
{
    public class Boost : IEffect, IDamageModifier
    {
        private long damageAdded = 0;
        
        public void Apply(ICard self)
        {
            damageAdded++;
            (this as IDamageModifier).AddDamage(self, 1);
            self.Log.AddEffectInfo($"{self}'s BOOST activated, base damage increased by 1");
        }

        public void Drop(ICard self)
        {
            (this as IDamageModifier).AddDamage(self, -damageAdded);
        }
    }
}