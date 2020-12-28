using System;
using System.Collections.Generic;
using MTCG.Battles;
using MTCG.Battles.Logging;
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
        public IPlayerLog Log { get; set; }

        public MonsterCard(
            double damage, DamageType damageType, MonsterType monsterType,
            IEnumerable<ISpeciality> specialities, IEnumerable<IEffect> effects, IPlayerLog log
        )
        {
            // Convert to decimal (easier calculations)
            // See: https://docs.microsoft.com/en-us/dotnet/api/system.convert.todecimal?view=net-5.0
            Damage = Convert.ToDecimal(damage);
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
            return $"{Type.GetString()} {MonsterType.GetString()}";
        }
    }
}