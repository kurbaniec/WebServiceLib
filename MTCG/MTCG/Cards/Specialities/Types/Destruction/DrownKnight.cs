using MTCG.Cards.Basis;
using MTCG.Cards.Basis.Monster;
using MTCG.Cards.DamageUtil;

namespace MTCG.Cards.Specialities.Types.Destruction
{
    /// <summary>
    /// Concrete <c>ISpeciality</c> and <c>IDestruction</c>
    /// used to drown Knights.
    /// </summary>
    public class DrownKnight : ISpeciality, IDestruction
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
            // Used for: "The armor of Knights is so heavy that WaterSpells make them drown them instantly."
            if (other is IMonsterCard otherMonster && otherMonster.MonsterType == MonsterType.Knight)
            {
                (this as IDestruction).Destroy(damage);
                self.Log.AddSpecialityInfo($"{self} drowns Knights instantly");
            }
        }
    }
}