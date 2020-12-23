using MTCG.Cards.Basis;
using MTCG.Cards.Basis.Monster;
using MTCG.Cards.Basis.Spell;
using MTCG.Cards.DamageUtil;

namespace MTCG.Cards.Specialities.Concrete
{
    public class Miss : ISpeciality
    {
        public void Apply(ICard self, ICard other, IDamage damage)
        {
            switch (self)
            {
                // Used for: "The Kraken is immune against spells"
                case SpellCard selfSpell 
                    when other is MonsterCard otherMonster && otherMonster.MonsterType == MonsterType.Kraken:
                // Used for: "The FireElves know Dragons since they were little and can evade their attacks"
                case MonsterCard selfMonster 
                    when selfMonster.MonsterType == MonsterType.Dragon && other is MonsterCard otherMonster2 && 
                         otherMonster2.MonsterType == MonsterType.Elve && otherMonster2.Type == DamageType.Fire:
                    damage.SetNoDamage();
                    break;
            }
        }
    }
}