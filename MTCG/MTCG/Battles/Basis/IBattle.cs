namespace MTCG.Battles.Basis
{
    /// <summary>
    /// Interface that describes battle logic.
    /// </summary>
    public interface IBattle
    {
        /// <summary>
        /// Perform a Monster Trading Cards Game battle.
        /// </summary>
        /// <returns>
        /// <c>BattleResult</c> of the battle
        /// </returns>
        BattleResult ProcessBattle();
    }
}