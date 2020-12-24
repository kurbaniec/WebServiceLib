using MTCG.Cards.DamageUtil;

namespace MTCG.Cards.Specialities.Types.Miss
{
    public interface IMiss
    {
        void Miss(IDamage damage)
        {
            damage.SetNoDamage();
        }
    }
}