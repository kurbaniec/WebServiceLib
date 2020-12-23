using System;

namespace MTCG.Cards.DamageUtil
{
    public interface IDamage : IComparable<IDamage>
    {
        bool IsInfty { get; }
        Decimal Value { get; }
        void Add(Decimal addition);
        void Multiply(Decimal multiplier);
        void Divide(Decimal divider);
        void SetInfty();
        void SetNoDamage();
    }
}