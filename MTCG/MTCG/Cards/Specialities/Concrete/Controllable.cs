using MTCG.Cards.Basis;
using MTCG.Cards.Basis.Monster;
using MTCG.Cards.DamageUtil;

namespace MTCG.Cards.Specialities.Concrete
{
    public class Controllable : ISpeciality
    {
        public void Apply(ICard self, ICard other, IDamage damage)
        {
            // Used for: "Wizzard can control Orks so they are not able to damage them"
            if (self is IMonsterCard selfMonster && selfMonster.MonsterType == MonsterType.Ork &&
                other is IMonsterCard otherMonster && otherMonster.MonsterType == MonsterType.Wizard)
            {
                damage.SetNoDamage();
            }
        }
    }
}