using MTCG.Users;

namespace MTCG.Battles
{
    public class Battle
    {

        /// <summary>
        /// Simulates the MTCG battle.
        /// </summary>
        /// <param name="playerA"></param>
        /// <param name="playerB"></param>
        public Battle(User playerA, User playerB)
        {

        }

        /// <summary>
        /// Starts and runs the battle simulator.
        /// </summary>
        /// <returns>
        /// The winning player or null if it is a draw.
        /// </returns>
        public User? RunGame()
        {
            return new User();
        }

        /// <summary>
        /// Starts internally a new game round.
        /// </summary>
        private void StartRound()
        {

        }

        /// <summary>
        /// Checks internally the game status at the end of a round.
        /// </summary>
        private void RoundCheck()
        {

        }

        /// <summary>
        /// Reassigns the player cards at the end of the round.
        /// </summary>
        private void ReassignCards()
        {

        }

        /// <summary>
        /// Check if a player has won the game.
        /// </summary>
        /// <returns>
        /// Tuple consisting of a boolean value indicating a player win and the
        /// player if it is a win or null if no one has won yet
        /// </returns>
        private (bool, User?) CheckWin()
        {
            return (true, new User());
        }

    }
}