using System.Collections.Generic;

namespace MTCG.Cards.Monsters
{
    public class FireElve : Monster
    {
        public FireElve(string name, uint damage, ElementType type, uint elementDamage) :
            base(name, damage, type, elementDamage)
        {

        }

        public override bool IsResistant(Card enemyCard)
        {
            throw new System.NotImplementedException();
        }

        public override bool IsWeak(Card enemyCard)
        {
            throw new System.NotImplementedException();
        }

        public override bool CanEvade(Card enemyCard)
        {
            return enemyCard is Dragon;
        }

        public override uint CalculateDamage(List<Card> enemyCards)
        {
            throw new System.NotImplementedException();
        }
    }
}