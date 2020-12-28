using System;
using System.Collections.Generic;
using MTCG.Battles;
using MTCG.Battles.Logging;
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
        IPlayerLog Log { get; set;  }

        IDamage CalculateDamage(ICard other)
        {
            Log.AddNewCardLog(this.ToString()!);
            var roundDamage = DamageUtil.Damage.Normal(Damage);
            Log.AddBaseDamageInfo(roundDamage.Value);
            if (this is ISpellCard || other is ISpellCard)
            {
                WeaponTriangle.EffectiveDamage(Type, other.Type, roundDamage);
                Log.AddWeaponTriangleInfo(roundDamage.Value);
            }
            foreach (var speciality in Specialities) speciality?.Apply(this, other, roundDamage);
            return roundDamage;
        }
    }
}