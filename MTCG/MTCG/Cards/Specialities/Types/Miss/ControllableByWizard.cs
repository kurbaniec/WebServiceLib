using MTCG.Cards.Basis;
using MTCG.Cards.Basis.Monster;
using MTCG.Cards.DamageUtil;
using MTCG.Cards.Specialities.Types.Miss;

namespace MTCG.Cards.Specialities.Types.Miss
{
    public class ControllableByWizard : ISpeciality, IMiss
    {
        public void Apply(ICard self, ICard other, IDamage damage)
        {
            // Used for: "Wizzard can control Orks so they are not able to damage them"
            if (other is IMonsterCard otherMonster && otherMonster.MonsterType == MonsterType.Wizard)
            {
                (this as IMiss).Miss(damage);
                self.Log.AddSpecialityInfo($"{self} is being controlled by a Wizard and can't attack!");
            }
        }
    }
}