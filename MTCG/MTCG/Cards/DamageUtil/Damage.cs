using System;
using System.Globalization;

namespace MTCG.Cards.DamageUtil
{
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

        public static IDamage Normal(decimal value) => new Damage(false, value);
        
        public static IDamage Infty() => new Damage(true, 0);

        public void Add(decimal addition)
        {
            value += addition;
        }
        
        public void Multiply(decimal multiplier)
        {
            value *= multiplier;
        }
        
        public void Divide(decimal divider)
        {
            value /= divider;
        }

        public void SetInfty()
        {
            if (!infty) infty = true;
        }

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
            return IsInfty ? "∞" : value.ToString(CultureInfo.InvariantCulture);
        }

        public override string ToString()
        {
            return ToString(null, null);
        }
    }
}