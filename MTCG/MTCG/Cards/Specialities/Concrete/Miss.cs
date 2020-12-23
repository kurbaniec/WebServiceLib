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
                case ISpellCard selfSpell 
                    when other is IMonsterCard otherMonster && otherMonster.MonsterType == MonsterType.Kraken:
                // Used for: "The FireElves know Dragons since they were little and can evade their attacks"
                case IMonsterCard selfMonster 
                    when selfMonster.MonsterType == MonsterType.Dragon && other is IMonsterCard otherMonster2 && 
                         otherMonster2.MonsterType == MonsterType.Elf && other.Type == DamageType.Fire:
                    damage.SetNoDamage();
                    break;
            }
        }
    }
}