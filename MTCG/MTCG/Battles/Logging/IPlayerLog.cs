using System.Collections.Generic;

namespace MTCG.Battles.Logging
{
    public interface IPlayerLog
    {
        string Username { get; }
        string CardName { get; }
        List<string> Log { get; }
        
        string? EffectLog { get; }

        void AddNewCardLog(string cardname);
        void AddBaseDamageInfo(decimal damage);

        void AddWeaponTriangleInfo(decimal damage);

        void AddSpecialityInfo(string info);

        void AddEffectInfo(string info);

        void Clear();
    }
}