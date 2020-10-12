using System.Collections.Generic;

namespace MTCG.Cards.Monsters
{
    public class Ork : Monster
    {
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
            throw new System.NotImplementedException();
        }

        public override uint CalculateDamage(List<Card> enemyCard)
        {
            throw new System.NotImplementedException();
        }
    }
}