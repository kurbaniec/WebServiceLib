using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MTCG.Cards.DamageUtil;

namespace MTCG.Battles.Logging
{
    public class BattleLog : IBattleLog
    {
        private readonly Dictionary<string, object> log = new Dictionary<string, object>();
        private int counter = 1;
        private IPlayerLog playerA = null!;
        private IPlayerLog playerB = null!;

        public (IPlayerLog, IPlayerLog) GetPlayerLogs(string playerAName, string playerBName)
        {
            playerA = new PlayerLog(playerAName);
            playerB = new PlayerLog(playerBName);
            return (playerA, playerB);
        }

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

        public Dictionary<string, object> GetLog()
        {
            return log;
        }
    }
}