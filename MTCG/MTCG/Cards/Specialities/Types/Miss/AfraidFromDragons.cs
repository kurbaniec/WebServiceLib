using MTCG.Cards.Basis;
using MTCG.Cards.Basis.Monster;
using MTCG.Cards.DamageUtil;
using MTCG.Cards.Specialities.Types.Miss;

namespace MTCG.Cards.Specialities.Types.Miss
{
    public class AfraidFromDragons : ISpeciality, IMiss
    {
        public void Apply(ICard other, IDamage damage)
        {
            // Used for: "Goblins are too afraid of Dragons to attack"
            if (other is IMonsterCard otherMonster && otherMonster.MonsterType == MonsterType.Dragon)
            {
                (this as IMiss).Miss(damage);
            }
        }
    }
}