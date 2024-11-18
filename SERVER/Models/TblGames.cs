namespace SERVER.Models
{
    public class TblGames
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public TimeOnly StartTime { get; set; }             // hh:mm:ss
        public DateOnly Date { get; set; }       //yyyy-mm-dd
        public int Duration { get; set; }


        /// NAVIGATION PROPERTY ? 
        public TblPlayers Player { get; set; }

    }
}
