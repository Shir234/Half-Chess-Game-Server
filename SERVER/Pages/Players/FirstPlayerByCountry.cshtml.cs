using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using SERVER.Models;

namespace SERVER.Pages.Players
{
    public class FirstPlayerByCountryModel : PageModel
    {
        private readonly SERVER.Data.SERVERContext _context;

        public FirstPlayerByCountryModel(SERVER.Data.SERVERContext context)
        {
            _context = context;
        }

        public List<FirstPlayerByCountry> PlayersByCountry { get; set; }

        public void OnGet()
        {
            PlayersByCountry = _context.TblPlayers
                .Where(p=> p.Games.Any())                                                           // Only countries with players who have games
                .GroupBy(p=> p.Country)                                                             // Group by Country
                .Select(group=> new FirstPlayerByCountry
                {
                    Country = group.Key,                                                            // the current country 
                    PlayerName = group
                                .SelectMany(player => player.Games                                  // select all the games - name and date
                                        .Select(game => new { player.Name, game.Date}))
                                .OrderBy(date=> date.Date)                                          // then order by date and choose the first one
                                .First().Name,
                    FirstGameDate = group
                                .SelectMany(player => player.Games
                                        .Select(game => game.Date)).Min().ToString("dd-MM-yyyy")    // Convert the earliest date to a string

                }).ToList();
        }
    }
}
