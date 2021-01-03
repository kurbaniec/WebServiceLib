namespace MTCG.Components.DataManagement.Schemas
{
    /// <summary>
    /// Data class that represent a battle.
    /// </summary>
    public class BattleSchema
    {
        public int Id { get; set; }
        public string PlayerA { get; set; }
        public string PlayerB { get; set; }
        public bool IsDraw { get; set; }
        public string? Winner { get; set; }
        public string? Looser { get; set; }

        public BattleSchema(int id, string playerA, string playerB)
        {
            Id = Id;
            PlayerA = playerA;
            PlayerB = playerB;
            IsDraw = true;
        }
        
        public BattleSchema(int id, string playerA, string playerB, string winner, string looser)
        {
            Id = Id;
            PlayerA = playerA;
            PlayerB = playerB;
            IsDraw = false;
            Winner = winner;
            Looser = looser;
        }
    }
}