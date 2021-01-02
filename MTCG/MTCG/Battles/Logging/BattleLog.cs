using System.Collections.Generic;
using System.Linq;
using MTCG.Cards.DamageUtil;

namespace MTCG.Battles.Logging
{
    /// <summary>
    /// Concrete implementation for <c>IBattleLog</c>.
    /// Used to log battles.
    /// </summary>
    public class BattleLog : IBattleLog
    {
        private readonly Dictionary<string, object> log = new Dictionary<string, object>();
        private int counter = 1;
        private IPlayerLog playerA = null!;
        private IPlayerLog playerB = null!;

        /// <summary>
        /// Used to generate <c>IPlayerLog</c>'s that will be used by the user cards.
        /// </summary>
        /// <param name="playerAName"></param>
        /// <param name="playerBName"></param>
        /// <returns>
        /// Pair of <c>IPlayerLog</c>'s corresponding to the given usernames
        /// </returns>
        public (IPlayerLog, IPlayerLog) GetPlayerLogs(string playerAName, string playerBName)
        {
            playerA = new PlayerLog(playerAName);
            playerB = new PlayerLog(playerBName);
            return (playerA, playerB);
        }

        /// <summary>
        /// Used to log a finished round.
        /// </summary>
        /// <param name="playerADamage"></param>
        /// <param name="playerBDamage"></param>
        /// <param name="cardsLeftA"></param>
        /// <param name="cardsLeftB"></param>
        public void RoundLog(IDamage playerADamage, IDamage playerBDamage, int cardsLeftA, int cardsLeftB)
        {
            if (!(playerA is { } a) || !(playerB is { } b)) return;
            var round = new Dictionary<string, object>
            {
                [a.Username] = playerA.Log.ToList(), [b.Username] = playerB.Log.ToList()
            };
            var result = new List<string>
            {
                $"{a.CardName} VS {b.CardName}", 
                $"{playerADamage.ToString()} VS {playerBDamage.ToString()}"
            };
            if (playerADamage.CompareTo(playerBDamage) == 0)
            {
                result.Add("Result: Draw");
                result.Add($"Remaining Cards: {cardsLeftA} VS {cardsLeftB}");
            } else if (playerADamage.CompareTo(playerBDamage) > 0)
            {
                result.Add($"Result: {a.Username} Win");
                result.Add($"Remaining Cards: {cardsLeftA} VS {cardsLeftB}");
                if (a.EffectLog is {} effect) result.Add(effect);
            }
            else
            {
                result.Add($"Result: {b.Username} Win");
                result.Add($"Remaining Cards: {cardsLeftA} VS {cardsLeftB}");
                if (b.EffectLog is {} effect) result.Add(effect);
            }
            round["result"] = result;
            log[$"round {counter}"] = round;
            counter++;
            
            playerA.Clear();
            playerB.Clear();
        }

        /// <summary>
        /// Used to log a finished game.
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="winner"></param>
        /// <param name="looser"></param>
        public void ResultLog(bool draw, string winner = "", string looser = "")
        {
            if (draw)
            {
                log["result"] = new Dictionary<string, object>() { {"draw", draw} };
            }
            else
            {
                log["result"] = new Dictionary<string, object>()
                {
                    {"draw", draw}, {"winner", winner}, {"looser", looser}
                };
            }
        }

        /// <summary>
        /// Method used to acquire the log of the battle as a Dictionary, that
        /// is convenient to parse to JSON.
        /// </summary>
        /// <returns>
        /// Log of the battle as a Dictionary
        /// </returns>
        public Dictionary<string, object> GetLog()
        {
            return log;
        }
    }
}