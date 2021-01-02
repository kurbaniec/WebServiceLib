using MTCG.Battles.Logging;
using MTCG.Battles.Player;

namespace MTCG.Battles.Basis
{
    /// <summary>
    /// Concrete implementation of the battle logic for the
    /// Monster Trading Cards Game.
    /// </summary>
    public class Battle : IBattle
    {
        private readonly IPlayer playerA;
        private readonly IPlayer playerB;
        private readonly IBattleLog log;

        public Battle(IPlayer playerA, IPlayer playerB, IBattleLog log)
        {
            this.playerA = playerA;
            this.playerB = playerB;
            this.log = log;
        }
        
        /// <summary>
        /// Perform a Monster Trading Cards Game battle.
        /// </summary>
        /// <returns>
        /// <c>BattleResult</c> of the battle
        /// </returns>
        public BattleResult ProcessBattle()
        {
            for (var i = 0; i < 100; i++)
            {
                var cardA = playerA.GetRandomCard();
                var cardB = playerB.GetRandomCard();
                var damageA = cardA.CalculateDamage(cardB);
                var damageB = cardB.CalculateDamage(cardA);

                if (damageA.CompareTo(damageB) > 0)
                {
                    playerA.AddToDeck(cardB);
                    playerB.RemoveFromDeck(cardB);
                }
                else if (damageA.CompareTo(damageB) < 0)
                {
                    playerB.AddToDeck(cardA);
                    playerA.RemoveFromDeck(cardA);
                }
                
                log.RoundLog(damageA, damageB, playerA.CardCount, playerB.CardCount);

                if (playerA.CardCount == 0)
                {
                    log.ResultLog(false, winner: playerB.Username, looser: playerA.Username);
                    return new BattleResult(playerB.Username, playerA.Username, log.GetLog());
                }
                if (playerB.CardCount == 0)
                {
                    log.ResultLog(false, winner: playerA.Username, looser: playerB.Username);
                    return new BattleResult(playerA.Username, playerB.Username, log.GetLog());
                }
            }
            return new BattleResult(log.GetLog());
        }
    }
}