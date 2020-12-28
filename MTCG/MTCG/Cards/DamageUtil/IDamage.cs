using System;

namespace MTCG.Cards.DamageUtil
{
    // Force toString
    // See: https://stackoverflow.com/a/50930292/12347616
    public interface IDamage : IComparable<IDamage>, IFormattable
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