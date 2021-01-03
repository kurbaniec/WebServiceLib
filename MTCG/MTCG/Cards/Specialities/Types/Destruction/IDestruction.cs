using MTCG.Cards.DamageUtil;

namespace MTCG.Cards.Specialities.Types.Destruction
{
    /// <summary>
    /// Interface that describes a type of Speciality that describe instant destruction. 
    /// </summary>
    public interface IDestruction
    {
        /// <summary>
        /// In order to destroy another card, damage must be set to
        /// infinity so it cannot be countered. 
        /// </summary>
        /// <param name="damage"></param>
        void Destroy(IDamage damage)
        {
            damage.SetInfty();
        }
    }
}