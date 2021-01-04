using MTCG.Cards.Basis;
using MTCG.Cards.Basis.Monster;
using MTCG.Cards.DamageUtil;

namespace MTCG.Cards.Specialities.Types.Miss
{
    /// <summary>
    /// Concrete <c>ISpeciality</c> and <c>IMiss</c>
    /// used to miss attacks at Fire Elves.
    /// </summary>
    public class MissFireElfBecauseOfEvasion : ISpeciality, IMiss
    {
        /// <summary>
        /// Check speciality between cards and modify damage of
        /// self's card when needed.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        /// <param name="damage"></param>
        public void Apply(ICard self, ICard other, IDamage damage)
        {
            // Used for: "The FireElves know Dragons since they were little and can evade their attacks"
            if (other is IMonsterCard otherMonster && otherMonster.MonsterType == MonsterType.Elf &&
                other.Type == DamageType.Fire)
            {
                (this as IMiss).Miss(damage);
                self.Log.AddSpecialityInfo($"FireElf evaded {self}'s attack!");
            }
        }
    }
}