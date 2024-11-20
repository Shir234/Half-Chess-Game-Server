using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SERVER.Models;

namespace SERVER.Pages.Players
{
    public class GroupPlayersByGamesCountModel : PageModel
    {
        private readonly SERVER.Data.SERVERContext _context;

        public List<GroupPlayersByGamesCount> PlayersGameCount { get; set; }

        public GroupPlayersByGamesCountModel(SERVER.Data.SERVERContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            PlayersGameCount = _context.TblPlayers
                .GroupBy(player => player.Games.Count())                // Group by games count
                .OrderByDescending(group => group.Key)                  // Descending order by the games count
                .Select(group => new GroupPlayersByGamesCount           // Fill each games count group list with players
                {
                    GamesCount = group.Key,                             // count, id and name    
                    PlayerDetails = group
                        .Select(player=> $"Id: {player.Id}, Name: {player.Name}, Country: {player.Country}")
                        .ToList()
                })
                .ToList();
        }
    }
}

