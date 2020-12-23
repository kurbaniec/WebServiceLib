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
            if (self is MonsterCard selfMonster && selfMonster.MonsterType == MonsterType.Ork &&
                other is MonsterCard otherMonster && otherMonster.MonsterType == MonsterType.Wizzard)
            {
                damage.SetNoDamage();
            }
        }
    }
}