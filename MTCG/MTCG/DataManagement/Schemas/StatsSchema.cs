namespace MTCG.DataManagement.Schemas
{
    public class StatsSchema
    {
        public string Username { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
        public string Elo { get; set; }
        public uint Wins { get; set; }
        public uint Looses { get; set; }
        public uint GamesPlayed => Wins + Looses;
        public uint Coins { get; set; }

        public StatsSchema(
            string username, string bio, string image,
            string elo, uint wins, uint looses, uint coins
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