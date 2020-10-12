namespace MTCG.Cards
{
    public abstract class Monster : Card
    {
        protected Monster(string name, uint damage, ElementType type, uint elementDamage)
        {
            this._name = name;
            this._damage = damage;
            this._type = type;
            this._elementDamage = elementDamage;
        }

        public abstract bool IsResistant(Card enemyCard);
        public abstract bool IsWeak(Card enemyCard);
        public abstract bool CanEvade(Card enemyCard);
        public abstract bool UseElementDamage(Card enemyCard);
    }
}