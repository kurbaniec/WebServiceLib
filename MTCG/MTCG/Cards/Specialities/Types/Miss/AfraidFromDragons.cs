using MTCG.Cards.Basis;
using MTCG.Cards.Basis.Monster;
using MTCG.Cards.DamageUtil;

namespace MTCG.Cards.Specialities.Types.Miss
{
    /// <summary>
    /// Concrete <c>ISpeciality</c> and <c>IMiss</c>
    /// used to miss attacks at Dragons.
    /// </summary>
    public class AfraidFromDragons : ISpeciality, IMiss
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
            // Used for: "Goblins are too afraid of Dragons to attack"
            if (other is IMonsterCard otherMonster && otherMonster.MonsterType == MonsterType.Dragon)
            {
                (this as IMiss).Miss(damage);
                self.Log.AddSpecialityInfo($"{self} is too afraid to attack a Dragon!");
            }
        }
    }
}