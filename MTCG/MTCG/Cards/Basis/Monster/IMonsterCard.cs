﻿using System.Collections.Generic;
using MTCG.Cards.Effects;

namespace MTCG.Cards.Basis.Monster
{
    public interface IMonsterCard
    {
        MonsterType MonsterType { get; }
        IEnumerable<IEffect> Effects { get; }
        void ApplyEffect(ICard self);
        void DropEffect(ICard self);
    }
}