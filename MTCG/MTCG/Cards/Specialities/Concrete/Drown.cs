using MTCG.Cards.Basis;
using MTCG.Cards.Basis.Monster;
using MTCG.Cards.Basis.Spell;
using MTCG.Cards.DamageUtil;

namespace MTCG.Cards.Specialities.Concrete
{
    public class Drown : ISpeciality
    {
        public void Apply(ICard self, ICard other, IDamage damage)
        {
            // Used for: "The armor of Knights is so heavy that WaterSpells make them drown them instantly."
            if (self is SpellCard selfSpell && selfSpell.Type == DamageType.Water &&
                other is MonsterCard otherMonster && otherMonster.MonsterType == MonsterType.Knight)
            {
                damage.SetInfty();
            }
        }
    }
}