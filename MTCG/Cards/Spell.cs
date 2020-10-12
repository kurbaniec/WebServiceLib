namespace MTCG.Cards
{
    public abstract class Spell : Card
    {
        protected Spell(string name, ElementType type, uint elementDamage)
        {
            this._name = name;
            this._type = type;
            this._elementDamage = elementDamage;
        }
    }
}