using System.Collections.Generic;
using MTCG.Battles;
using MTCG.Cards.Basis.Monster;
using MTCG.Cards.DamageUtil;
using MTCG.Cards.Effects;
using MTCG.Cards.Specialities;

namespace MTCG.Cards.Basis.Spell
{
    public class SpellCard : ICard, ISpellCard
    {
        public decimal Damage { get; set; }
        public DamageType Type { get; }
        public IEnumerable<ISpeciality> Specialities { get; }
        public BattleLog Log { get; }

        public SpellCard(
            uint damage, DamageType damageType, IEnumerable<ISpeciality> specialities,  
            BattleLog log
        )
        {
            Damage = damage;
            Type = damageType;
            Specialities = specialities;
            Log = log;
        }

        public override string ToString()
        {
            return $"{nameof(Type)} Spell";
        }
    }
}