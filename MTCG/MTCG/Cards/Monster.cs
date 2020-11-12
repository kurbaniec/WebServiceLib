using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// Cards resistant to others will take no damage. This means that
        /// the enemy card will output zero damage overall.
        /// </summary>
        /// <param name="enemyCard"></param>
        /// <returns>
        /// True, when this card is resistant to the enemy card, else False.
        /// </returns>

        public abstract bool IsResistant(Card enemyCard);
        /// <summary>
        /// Weak cards are destroyed instantly and output zero damage overall.
        /// </summary>
        /// <param name="enemyCard"></param>
        /// <returns>
        /// True, when this card is weak to the enemy card, else False.
        /// </returns>
        public abstract bool IsWeak(Card enemyCard);

        /// <summary>
        /// Evading cards take no damage. This means that
        /// the enemy card will output zero damage overall.
        /// </summary>
        /// <param name="enemyCard"></param>
        /// <returns></returns>
        public abstract bool CanEvade(Card enemyCard);

        /// <summary>
        /// When the enemy cards contain at least one spell card then this
        /// monster card will use its elemental damage instead of normal damage
        /// as damage output.
        /// </summary>
        /// <param name="enemyCards"></param>
        /// <returns>
        /// True, when the enemy cards contain at least one spell card,
        /// else False.
        /// </returns>
        public virtual bool UseElementDamage(List<Card> enemyCards)
        {
            return enemyCards.OfType<Spell>().Any();
        }
    }
}