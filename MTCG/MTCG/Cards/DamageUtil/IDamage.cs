using System;

namespace MTCG.Cards.DamageUtil
{
    public interface IDamage : IComparable<IDamage>
    {
        bool IsInfty { get; }
        uint Value { get; }
        void Add(uint damage);
        void SetInfty();
        void SetNoDamage();
    }
}