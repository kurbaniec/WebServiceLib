using MTCG.Battles;
using MTCG.Cards.Basis;
using MTCG.Cards.DamageUtil;

namespace MTCG.Cards.Effects
{
    public interface IEffect
    {
        void Apply(ICard self);
        void Drop(ICard self);
    }
}