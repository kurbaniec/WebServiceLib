using System;
using System.Collections.Generic;

namespace MTCG.Battles.Logging
{
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
        
        public void AddNewCardLog(string cardname)
        {
            CardName = cardname;
        }

        public void AddBaseDamageInfo(decimal damage)
        {
            Log.Add($"{Username} summons a {CardName} with base damage {damage}");
        }

        public void AddWeaponTriangleInfo(decimal damage)
        {
            Log.Add($"{CardName} has an effective damage of {damage} in this battle");
        }

        public void AddSpecialityInfo(string info)
        {
            Log.Add(info);
        }

        public void AddEffectInfo(string info)
        {
            EffectLog = info;
        }

        public void Clear()
        {
            Log.Clear();
        }
    }
}