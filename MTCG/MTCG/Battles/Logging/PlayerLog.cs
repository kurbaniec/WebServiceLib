using System.Collections.Generic;

namespace MTCG.Battles.Logging
{
    /// <summary>
    /// Concrete implementation of <c>IPlayerLog</c>.
    /// Used to log card events.
    /// </summary>
    public class PlayerLog : IPlayerLog
    {
        public string Username { get; private set; }
        public string CardName { get; private set; }
        public List<string> Log { get; }
        public string? EffectLog { get; private set; }
        
        public PlayerLog(string username)
        {
            Username = username;
            CardName = string.Empty;
            Log = new List<string>();
            EffectLog = null;
        }
        
        /// <summary>
        /// Used to activate card logging.
        /// </summary>
        /// <param name="cardname"></param>
        public void AddNewCardLog(string cardname)
        {
            CardName = cardname;
        }

        /// <summary>
        /// Log base damage of the card.
        /// </summary>
        /// <param name="damage"></param>
        public void AddBaseDamageInfo(decimal damage)
        {
            Log.Add($"{Username} summons a {CardName} with base damage {damage}");
        }

        /// <summary>
        /// Log speciality damage (Weapon-Triangle) of the card.
        /// </summary>
        /// <param name="damage"></param>
        public void AddWeaponTriangleInfo(decimal damage)
        {
            Log.Add($"{CardName} has an effective damage of {damage} in this battle");
        }

        /// <summary>
        /// Log in game card-speciality info. 
        /// </summary>
        /// <param name="info"></param>
        public void AddSpecialityInfo(string info)
        {
            Log.Add(info);
        }

        /// <summary>
        /// Log in game card-effect info.
        /// </summary>
        /// <param name="info"></param>
        public void AddEffectInfo(string info)
        {
            EffectLog = info;
        }

        /// <summary>
        /// Clear internal data in order to log new card. 
        /// </summary>
        public void Clear()
        {
            Log.Clear();
        }
    }
}