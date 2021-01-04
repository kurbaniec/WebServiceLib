using System.Collections.Generic;

namespace MTCG.Battles.Logging
{
    /// <summary>
    /// Interfaces that describes the logic of the player logger used internally by <c>ICard</c>'s
    /// to log card events.
    /// </summary>
    public interface IPlayerLog
    {
        string Username { get; }
        string CardName { get; }
        List<string> Log { get; }
        string? EffectLog { get; }

        /// <summary>
        /// Used to activate card logging.
        /// </summary>
        /// <param name="cardname"></param>
        void AddNewCardLog(string cardname);
        
        /// <summary>
        /// Log base damage of the card.
        /// </summary>
        /// <param name="damage"></param>
        void AddBaseDamageInfo(decimal damage);

        /// <summary>
        /// Log speciality damage (Weapon-Triangle) of the card.
        /// </summary>
        /// <param name="damage"></param>
        void AddWeaponTriangleInfo(decimal damage);

        /// <summary>
        /// Log in game card-speciality info. 
        /// </summary>
        /// <param name="info"></param>
        void AddSpecialityInfo(string info);

        /// <summary>
        /// Log in game card-effect info.
        /// </summary>
        /// <param name="info"></param>
        void AddEffectInfo(string info);

        /// <summary>
        /// Clear internal data in order to log new card. 
        /// </summary>
        void Clear();
    }
}