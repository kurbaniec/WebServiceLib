using System;
using System.Globalization;

namespace MTCG.Cards.DamageUtil
{
    /// <summary>
    /// Represents a damage value in the Monster Trading Cards Game.
    /// Concrete implementation of <c>IDamage</c>.
    /// </summary>
    public class Damage : IDamage
    {
        private bool infty;
        private decimal value;
        public bool IsInfty => infty;
        public decimal Value => value;

        private Damage(bool infty, decimal value)
        {
            this.infty = infty;
            this.value = value;
        }

        /// <summary>
        /// Build a Damage object with a concrete base damage.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>
        /// Damage object with a concrete base damage as <c>IDamage</c>
        /// </returns>
        public static IDamage Normal(decimal value) => new Damage(false, value);
        
        /// <summary>
        /// Build a Damage object with the infinite damage modifier.
        /// </summary>
        /// <returns>
        /// Build a Damage object with the infinite damage modifier as <c>IDamage</c>
        /// </returns>
        public static IDamage Infty() => new Damage(true, 0);

        /// <summary>
        /// Add damage to the base damage.
        /// </summary>
        /// <param name="addition"></param>
        public void Add(decimal addition)
        {
            value += addition;
        }
        
        /// <summary>
        /// Multiply base damage.
        /// </summary>
        /// <param name="multiplier"></param>
        public void Multiply(decimal multiplier)
        {
            value *= multiplier;
        }
        
        /// <summary>
        /// Divide base damage.
        /// </summary>
        /// <param name="divider"></param>
        public void Divide(decimal divider)
        {
            value /= divider;
        }

        /// <summary>
        /// Activate the infinite damage modifier.
        /// </summary>
        public void SetInfty()
        {
            if (!infty) infty = true;
        }

        /// <summary>
        /// Disable the infinite damage modifier when set and
        /// set the base damage to zero.
        /// </summary>
        public void SetNoDamage()
        {
            if (infty) infty = false;
            value = 0;
        }
        
        public int CompareTo(IDamage? other)
        {
            if (other == null) return 1;
            if ((this.IsInfty && other.IsInfty) || (this.Value == other.Value)) return 0;
            if ((this.IsInfty && !other.IsInfty) ||
                ((!this.IsInfty && !other.IsInfty) && (this.Value > other.Value))) return 1;
            return -1;
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            return IsInfty ? "Infinity" : value.ToString(CultureInfo.InvariantCulture);
        }

        public override string ToString()
        {
            return ToString(null, null);
        }
    }
}