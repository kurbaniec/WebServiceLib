using MTCG.Battles;
using MTCG.Cards.Basis;
using MTCG.Cards.DamageUtil;

namespace MTCG.Cards.Specialities
{
    /// <summary>
    /// Interfaces that is used to determine specialities and relationships
    /// between cards like "A cannot attack B because of C".
    /// </summary>
    public interface ISpeciality
    {
        /// <summary>
        /// Check speciality between cards and modify damage of
        /// self's card when needed.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="other"></param>
        /// <param name="damage"></param>
        void Apply(ICard self, ICard other, IDamage damage);
    }
}