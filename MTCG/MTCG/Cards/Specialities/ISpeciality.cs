using MTCG.Battles;
using MTCG.Cards.Basis;
using MTCG.Cards.DamageUtil;

namespace MTCG.Cards.Specialities
{
    public interface ISpeciality
    {
        Damage Apply(ICard self, ICard other, IDamage damage);
    }
}