using MTCG.Cards.Basis;

namespace MTCG.Cards.Effects
{
    /// <summary>
    /// Bonus addition to the Monster Trading Cards Game: Effects.
    /// An Effect is a extra speciality on monster cards that is applied
    /// when a monster card wins a round. The effect stays activated on the card
    /// until the battle is over or the card was defeated and
    /// swapped to the opponents deck.
    /// </summary>
    public interface IEffect
    {
        /// <summary>
        /// Apply Effect on monster card.
        /// </summary>
        /// <param name="self"></param>
        void Apply(ICard self);
        
        /// <summary>
        /// Drop Effect from monster card.
        /// </summary>
        /// <param name="self"></param>
        void Drop(ICard self);
    }
}