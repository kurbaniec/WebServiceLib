using System.Collections.Generic;
using MTCG.Battles;
using MTCG.Cards.DamageUtil;
using MTCG.Cards.Effects;
using MTCG.Cards.Specialities;

namespace MTCG.Cards.Basis.Monster
{
    public class MonsterCard : ICard, IMonsterCard
    {
        public decimal Damage { get; set; }
        
        public DamageType Type { get; }
        
        public IEnumerable<ISpeciality> Specialities { get; }
        public BattleLog Log { get; }

        public MonsterType MonsterType { get; }
        public IEnumerable<IEffect> Effects { get; }

        public IDamage CalculateDamage(ICard other)
        {
            var roundDamage = (this as ICard).CalculateDamage(other);

            return roundDamage;
        }

        
        public void ApplyEffect(ICard self)
        {
            throw new System.NotImplementedException();
        }

        public void DropEffect(ICard self)
        {
            throw new System.NotImplementedException();
        }
    }
}