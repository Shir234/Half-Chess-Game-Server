using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SERVER.Models;

namespace SERVER.Pages.Players
{
    public class PlayerGameCountModel : PageModel
    {
        private readonly SERVER.Data.SERVERContext _context;

        public List<PlayerGameCount> PlayersGameCount { get; set; }

        public PlayerGameCountModel(SERVER.Data.SERVERContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            PlayersGameCount = _context.TblPlayers
                .Select(player => new PlayerGameCount
                {
                    PlayerId = player.Id,
                    PlayerName = player.Name,
                    GameCount = player.Games.Count()
                })
                .ToList();
        }
    }
}




