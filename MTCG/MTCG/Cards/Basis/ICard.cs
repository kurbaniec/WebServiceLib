using System;
using System.Collections.Generic;
using MTCG.Battles;
using MTCG.Cards.Basis.Spell;
using MTCG.Cards.DamageUtil;
using MTCG.Cards.Specialities;

namespace MTCG.Cards.Basis
{
    public interface ICard
    {
        decimal Damage { get; set; }
        DamageType Type { get; }
        IEnumerable<ISpeciality> Specialities { get; }
        IBattleLog Log { get;  }

        IDamage CalculateDamage(ICard other)
        {
            var roundDamage = DamageUtil.Damage.Normal(Damage);
            if (this is ISpellCard || other is ISpellCard)
                WeaponTriangle.EffectiveDamage(Type, other.Type, roundDamage);
            foreach (var speciality in Specialities) speciality?.Apply(this, other, roundDamage);
            return roundDamage;
        }
    }
}