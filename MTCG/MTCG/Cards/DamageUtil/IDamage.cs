using System;

namespace MTCG.Cards.DamageUtil
{
    /// <summary>
    /// Interface that represents a damage value.
    /// </summary>
    // Force toString
    // See: https://stackoverflow.com/a/50930292/12347616
    public interface IDamage : IComparable<IDamage>, IFormattable
    {
        bool IsInfty { get; }
        decimal Value { get; }
        
        /// <summary>
        /// Add damage to the base damage.
        /// </summary>
        /// <param name="addition"></param>
        void Add(decimal addition);
        
        /// <summary>
        /// Multiply base damage.
        /// </summary>
        /// <param name="multiplier"></param>
        void Multiply(decimal multiplier);
        
        /// <summary>
        /// Divide base damage.
        /// </summary>
        /// <param name="divider"></param>
        void Divide(decimal divider);
        
        /// <summary>
        /// Activate the infinite damage modifier.
        /// </summary>
        void SetInfty();
        
        /// <summary>
        /// Disable the infinite damage modifier when set and
        /// set the base damage to zero.
        /// </summary>
        void SetNoDamage();
    }
}