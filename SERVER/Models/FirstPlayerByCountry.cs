namespace SERVER.Models
{
    /// Object to use in the page that display the first player that ever played from each country!! (query 25)
    public class FirstPlayerByCountry
    {
        public string Country { get; set; }
        public string PlayerName { get; set; }
        public string FirstGameDate { get; set; } 

    }
}
