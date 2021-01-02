using System.Collections.Generic;
using MTCG.Cards.Effects;

namespace MTCG.Cards.Basis.Monster
{
    /// <summary>
    /// Interface that marks a monster card.
    /// </summary>
    public interface IMonsterCard
    {
        MonsterType MonsterType { get; }
        IEnumerable<IEffect> Effects { get; }
        
        /// <summary>
        /// Apply <c>IEffect</c>s when a monster card wins a fight.
        /// </summary>
        void ApplyEffects();
        
        /// <summary>
        /// Drop <c>IEffect</c>s when a monster card is removed from the deck
        /// and given to another.
        /// </summary>
        void DropEffects();
    }
}