namespace SERVER.Models
{
    public class TblGames
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }


        /// NAVIGATION PROPERTY ? 
        public TblPlayers Player { get; set; }

    }
}
