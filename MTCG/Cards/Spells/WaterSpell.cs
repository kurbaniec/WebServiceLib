using System.Collections.Generic;

namespace MTCG.Cards.Spells
{
    public class WaterSpell : Spell
    {
        public WaterSpell(string name, ElementType type, uint elementDamage) : base(name, type, elementDamage)
        {
        }

        public override uint CalculateDamage(List<Card> enemyCards)
        {
            throw new System.NotImplementedException();
        }
    }
}