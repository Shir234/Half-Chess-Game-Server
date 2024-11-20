using System.Diagnostics.Contracts;

namespace SERVER.Models
{
    /// Object to use in the page that display each player games count!! (query 27)
    public class PlayerGameCount
    {
        //PlayerGameCount
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int GameCount { get; set; }
    }
}
