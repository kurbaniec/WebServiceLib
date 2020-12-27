namespace MTCG.DataManagement.Schemas
{
    public class StatsSchema
    {
        public string Username { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
        public long Elo { get; set; }
        public long Wins { get; set; }
        public long Looses { get; set; }
        public long GamesPlayed => Wins + Looses;
        public long Coins { get; set; }

        public StatsSchema(string username)
        {
            Username = username;
            Bio = "No Bio";
            Image = "No Image";
            Elo = 1000;
            Wins = 0;
            Looses = 0;
            Coins = 20;
        }
        
        public StatsSchema(
            string username, string bio, string image,
            uint elo, uint wins, uint looses, uint coins
        )
        {
            Username = username;
            Bio = bio;
            Image = image;
            Elo = elo;
            Wins = wins;
            Looses = looses;
            Coins = coins;
        }
    }
}