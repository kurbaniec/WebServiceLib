using MTCG.Cards.Basis;
using MTCG.Cards.Basis.Monster;
using MTCG.Cards.DamageUtil;
using MTCG.Cards.Specialities.Types.Miss;

namespace MTCG.Cards.Specialities.Types.Miss
{
    public class ControllableByWizzard : ISpeciality, IMiss
    {
        public void Apply(ICard other, IDamage damage)
        {
            // Used for: "Wizzard can control Orks so they are not able to damage them"
            if (other is IMonsterCard otherMonster && otherMonster.MonsterType == MonsterType.Wizard)
            {
                (this as IMiss).Miss(damage);
            }
        }
    }
}