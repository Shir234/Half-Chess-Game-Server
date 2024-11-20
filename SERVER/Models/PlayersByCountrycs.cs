namespace SERVER.Models
{
    /// Object to use in the page that display players grouped by country!! (query 29)
    public class PlayersByCountry
    {
        //PlayersByCountry
        public string Country { get; set; }
        public List<string> PlayerDetails { get; set; }
    }
}
