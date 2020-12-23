using MTCG.Battles;
using MTCG.Cards.Basis;
using MTCG.Cards.DamageUtil;

namespace MTCG.Cards.Effects
{
    public interface IEffect
    {
        Damage Apply(ICard self, ICard other, IDamage damage);
        void Drop(ICard self);
    }
}