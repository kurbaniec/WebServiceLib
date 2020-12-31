namespace MTCG.Components.DataManagement.Schemas
{
    public class StatsSchema
    {
        public string Username { get; set; }
        public long Elo { get; set; }
        public long Wins { get; set; }
        public long Looses { get; set; }
        public long Draws { get; set; }
        public long GamesPlayed => Wins + Looses + Draws;
        public long Coins { get; set; }
        public string Realname { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }

        public StatsSchema(string username)
        {
            Username = username;
            Elo = 1000;
            Wins = 0;
            Looses = 0;
            Draws = 0;
            Coins = 20;
            Realname = "No name given";
            Bio = "No Bio";
            Image = "No Image";
        }

        // Used for scoreboard
        public StatsSchema(string username, long elo, long wins, long looses, long draws)
        {
            Username = username;
            Elo = elo;
            Wins = wins;
            Looses = looses;
            Draws = draws;
        }
        
        public StatsSchema(
            string username, long elo, long wins, long looses, long draws, long coins,
            string realname, string bio, string image
        )
        {
            Username = username;
            Realname = realname;
            Bio = bio;
            Image = image;
            Elo = elo;
            Wins = wins;
            Looses = looses;
            Draws = draws;
            Coins = coins;
        }
    }
}