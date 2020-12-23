using System;
using System.Collections.Generic;
using MTCG.Battles;
using MTCG.Cards.DamageUtil;
using MTCG.Cards.Specialities;

namespace MTCG.Cards.Basis
{
    public interface ICard
    {
        decimal Damage { get; set; }
        DamageType Type { get; }
        IEnumerable<ISpeciality> Specialities { get; }
        BattleLog Log { get;  }

        IDamage CalculateDamage(ICard other)
        {
            var roundDamage = DamageUtil.Damage.Normal(Damage);
            WeaponTriangle.EffectiveDamage(Type, other.Type, roundDamage);
            foreach (var speciality in Specialities) speciality?.Apply(this, other, roundDamage);
            return roundDamage;
        }
    }
}