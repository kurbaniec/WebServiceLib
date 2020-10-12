using System.Collections;
using System.Collections.Generic;

namespace MTCG.Cards
{
    public abstract class Card
    {
        protected string _name;
        protected ElementType _type;
        protected uint _damage;
        protected uint _elementDamage;
        protected bool _inDeck;
        protected bool _inStore;
        public abstract uint CalculateDamage(List<Card> enemyCards);
    }
}