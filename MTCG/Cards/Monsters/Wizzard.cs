﻿using System.Collections.Generic;

namespace MTCG.Cards.Monsters
{
    public class Wizzard : Monster
    {
        public Wizzard(string name, uint damage, ElementType type, uint elementDamage) : base(name, damage, type, elementDamage)
        {
        }

        public override bool IsResistant(Card enemyCard)
        {
            throw new System.NotImplementedException();
        }

        public override bool IsWeak(Card enemyCard)
        {
            throw new System.NotImplementedException();
        }

        public override bool CanEvade(Card enemyCard)
        {
            throw new System.NotImplementedException();
        }

        public override uint CalculateDamage(List<Card> enemyCards)
        {
            throw new System.NotImplementedException();
        }
    }
}