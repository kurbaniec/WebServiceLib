namespace MTCG.Cards.DamageUtil
{
    public class Damage : IDamage
    {
        private bool infty;
        private uint value;
        public bool IsInfty => infty;
        public uint Value => value;

        private Damage(bool infty, uint value)
        {
            this.infty = infty;
            this.value = value;
        }

        public static IDamage Normal(uint value) => new Damage(false, value);
        
        public static IDamage Infty() => new Damage(true, 0);

        public void Add(uint damage)
        {
            value += damage;
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
    }
}