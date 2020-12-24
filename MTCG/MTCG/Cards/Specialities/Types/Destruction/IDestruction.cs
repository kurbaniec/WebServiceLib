using MTCG.Cards.DamageUtil;

namespace MTCG.Cards.Specialities.Types.Destruction
{
    public interface IDestruction
    {
        void Destroy(IDamage damage)
        {
            damage.SetInfty();
        }
    }
}