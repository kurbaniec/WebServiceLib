using MTCG.Cards.Basis;
using MTCG.Cards.Basis.Monster;
using MTCG.Cards.DamageUtil;

namespace MTCG.Cards.Specialities.Types.Miss
{
    public class MissKrakenBecauseOfImmunity : ISpeciality, IMiss
    {
        public void Apply(ICard other, IDamage damage)
        {
            // Used for: "The Kraken is immune against spells"
            if (other is IMonsterCard otherMonster && otherMonster.MonsterType == MonsterType.Kraken)
            {
                (this as IMiss).Miss(damage);
            }
        }
    }
}