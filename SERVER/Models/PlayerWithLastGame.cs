namespace SERVER.Models
{
    /// Object to use in the page that display each player last game date!! (query 23)
    public class PlayerWithLastGame
    {
        public string PlayerName { get; set; }
        public string LastGameDate { get; set; } // Store the formatted date

        public PlayerWithLastGame(string playerName, DateOnly? lastGameDate)
        {
            PlayerName = playerName;
            LastGameDate = lastGameDate?.ToString("dd-MM-yyyy") ?? "No games played";
        }
    }
}
