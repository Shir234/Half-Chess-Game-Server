namespace SERVER.Models
{
    /// Object to use in the page that display players grouped by the game count!! (query 28)
    public class GroupPlayersByGamesCount
    {
        // GroupPlayersByGamesCount
        public int GamesCount { get; set; }
        public List<string> PlayerDetails { get; set; }

    }
}
