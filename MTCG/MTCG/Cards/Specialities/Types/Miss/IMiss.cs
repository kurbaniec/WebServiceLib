using MTCG.Cards.DamageUtil;

namespace MTCG.Cards.Specialities.Types.Miss
{
    /// <summary>
    /// Interface that describes a type of Speciality that describe a missed attack. 
    /// </summary>
    public interface IMiss
    {
        /// <summary>
        /// A missed attack is equal to an attack with zero damage.
        /// </summary>
        /// <param name="damage"></param>
        void Miss(IDamage damage)
        {
            damage.SetNoDamage();
        }
    }
}