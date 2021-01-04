using System;
using System.Collections.Generic;
using MTCG.Battles.Logging;
using MTCG.Cards.DamageUtil;
using MTCG.Cards.Specialities;

namespace MTCG.Cards.Basis.Spell
{
    /// <summary>
    /// Represents a concrete spell card.
    /// </summary>
    public class SpellCard : ICard, ISpellCard
    {
        public decimal Damage { get; set; }
        public DamageType Type { get; }
        public IEnumerable<ISpeciality> Specialities { get; }
        public IPlayerLog Log { get; set; }

        public SpellCard(
            double damage, DamageType damageType, IEnumerable<ISpeciality> specialities,  
            IPlayerLog log
        )
        {
            Damage = Convert.ToDecimal(damage);
            Type = damageType;
            Specialities = specialities;
            Log = log;
        }

        public override string ToString()
        {
            return $"{Type.GetString()} Spell";
        }
    }
}