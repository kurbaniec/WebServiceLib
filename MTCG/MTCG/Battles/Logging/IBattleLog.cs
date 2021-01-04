using System.Collections.Generic;
using MTCG.Cards.DamageUtil;

namespace MTCG.Battles.Logging
{
    /// <summary>
    /// Interface that describes the logic of the main battle logger.
    /// </summary>
    public interface IBattleLog
    {
        /// <summary>
        /// Used to generate <c>IPlayerLog</c>'s that will be used by the user cards.
        /// </summary>
        /// <param name="playerAName"></param>
        /// <param name="playerBName"></param>
        /// <returns>
        /// Pair of <c>IPlayerLog</c>'s corresponding to the given usernames
        /// </returns>
        (IPlayerLog, IPlayerLog) GetPlayerLogs(string playerAName, string playerBName);

        /// <summary>
        /// Used to log a finished round.
        /// </summary>
        /// <param name="playerADamage"></param>
        /// <param name="playerBDamage"></param>
        /// <param name="cardsLeftA"></param>
        /// <param name="cardsLeftB"></param>
        void RoundLog(IDamage playerADamage, IDamage playerBDamage, int cardsLeftA, int cardsLeftB);

        /// <summary>
        /// Used to log a finished game.
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="winner"></param>
        /// <param name="looser"></param>
        void ResultLog(bool draw, string winner = "", string looser = "");

        /// <summary>
        /// Method used to acquire the log of the battle as a Dictionary, that
        /// is convenient to parse to JSON.
        /// </summary>
        /// <returns>
        /// Log of the battle as a Dictionary
        /// </returns>
        Dictionary<string, object> GetLog();
    }
}