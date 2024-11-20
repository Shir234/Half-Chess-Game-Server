using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SERVER.Models;

namespace SERVER.Pages.Players
{
    public class PlayerLastGameModel : PageModel
    {

        private readonly SERVER.Data.SERVERContext _context;

        public PlayerLastGameModel(SERVER.Data.SERVERContext context)
        {
            _context = context;
        }

        public List<PlayerWithLastGame> Players { get; set; }

        public void OnGet()
        {
            if (_context.TblPlayers != null)
            {
                Players = _context.TblPlayers
                    .OrderByDescending(p=> p.Name)
                    .Select(p=> new PlayerWithLastGame(
                        p.Name, 
                        p.Games
                            .OrderByDescending(g=> g.Date)
                            .Select(g=> (DateOnly?)g.Date)
                            .FirstOrDefault()))
                    .ToList();
            }
        }
    }
}
