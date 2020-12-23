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
        public MonsterType MonsterType { get; }
        public IEnumerable<ISpeciality> Specialities { get; }
        public IEnumerable<IEffect> Effects { get; }
        public IBattleLog Log { get; }

        public MonsterCard(
            uint damage, DamageType damageType, MonsterType monsterType,
            IEnumerable<ISpeciality> specialities, IEnumerable<IEffect> effects, IBattleLog log
        )
        {
            Damage = damage;
            Type = damageType;
            MonsterType = monsterType;
            Specialities = specialities;
            Effects = effects;
            Log = log;
        }

        public void ApplyEffects()
        {
            foreach (var effect in Effects) effect?.Apply(this);
        }

        public void DropEffects()
        {
            foreach (var effect in Effects) effect?.Drop(this);
        }

        public override string ToString()
        {
            // TODO nameof Normal => Regular or skip it entirely
            return $"{nameof(Type)} {MonsterType.GetString()}";
        }
    }
}