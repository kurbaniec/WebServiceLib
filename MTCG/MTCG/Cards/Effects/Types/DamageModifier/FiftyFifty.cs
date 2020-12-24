using System;
using MTCG.Cards.Basis;

namespace MTCG.Cards.Effects.Types.DamageModifier
{
    public class FiftyFifty : IEffect, IDamageModifier
    {
        private long damageAdded = 0;
        private readonly Random rng = new Random();
        
        public void Apply(ICard self)
        {
            // Get random value
            // See: https://stackoverflow.com/a/15325580/12347616
            var value = (rng.Next(0, 2) > 0) ? 2 : -2;
            if (self.Damage + value > 0) damageAdded += value;
            else damageAdded = 0;
            (this as IDamageModifier).AddDamage(self, value);
        }

        public void Drop(ICard self)
        {
            (this as IDamageModifier).AddDamage(self, -damageAdded);
        }
    }
}