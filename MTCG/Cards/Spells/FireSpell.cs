using System.Collections.Generic;

namespace MTCG.Cards.Spells
{
    public class FireSpell : Spell
    {
        public FireSpell(string name, ElementType type, uint elementDamage) : base(name, type, elementDamage)
        {
        }

        public override uint CalculateDamage(List<Card> enemyCards)
        {
            throw new System.NotImplementedException();
        }
    }
}