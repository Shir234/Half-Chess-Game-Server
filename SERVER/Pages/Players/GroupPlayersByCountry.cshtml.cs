using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SERVER.Models;

namespace SERVER.Pages.Players
{
    public class GroupPlayersByCountryModel : PageModel
    {
        private readonly SERVER.Data.SERVERContext _context;

        public List<PlayersByCountry> PlayersByCountry { get; set; }

        public GroupPlayersByCountryModel(SERVER.Data.SERVERContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            PlayersByCountry = _context.TblPlayers
                .GroupBy(player => player.Country)                                      // Group by Country
                .Select(group => new PlayersByCountry                                   // Fill each Country group list with players
                {
                    Country = group.Key,                                                // Country, id and name    
                    PlayerDetails = group
                        .Select(player => $"Id: {player.Id}, Name: {player.Name}")
                        .ToList()
                })
                .ToList();
        }
    }
}
