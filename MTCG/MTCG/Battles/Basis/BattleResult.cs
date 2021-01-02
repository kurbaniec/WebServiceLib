using System;
using System.Collections.Generic;

namespace MTCG.Battles.Basis
{
    public class BattleResult
    {
        public bool Draw { get; }
        public string Winner { get; }
        public string Looser { get; }
        public Dictionary<string, object> Log { get; }

        public BattleResult(string winner, string looser, Dictionary<string, object> log)
        {
            Draw = false;
            Winner = winner;
            Looser = looser;
            Log = log;
        }

        public BattleResult(Dictionary<string, object> log)
        {
            Draw = true;
            Log = log;
            Winner = string.Empty;
            Looser = string.Empty;
        }
    }
}