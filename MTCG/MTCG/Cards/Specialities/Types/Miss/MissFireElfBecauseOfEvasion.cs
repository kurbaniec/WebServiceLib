using MTCG.Cards.Basis;
using MTCG.Cards.Basis.Monster;
using MTCG.Cards.DamageUtil;

namespace MTCG.Cards.Specialities.Types.Miss
{
    public class MissFireElfBecauseOfEvasion : ISpeciality, IMiss
    {
        public void Apply(ICard other, IDamage damage)
        {
            // Used for: "The FireElves know Dragons since they were little and can evade their attacks"
            if (other is IMonsterCard otherMonster && otherMonster.MonsterType == MonsterType.Elf &&
                other.Type == DamageType.Fire)
            {
                (this as IMiss).Miss(damage);
            }
        }
    }
}