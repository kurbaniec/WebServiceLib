﻿using System.Collections.Generic;

namespace MTCG.Cards.Monsters
{
    public class Kraken : Monster
    {
        public Kraken(string name, uint damage, ElementType type, uint elementDamage) : base(name, damage, type, elementDamage)
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

        public override bool UseElementDamage(Card enemyCard)
        {
            throw new System.NotImplementedException();
        }

        public override uint CalculateDamage(List<Card> enemyCard)
        {
            throw new System.NotImplementedException();
        }

        
    }
}