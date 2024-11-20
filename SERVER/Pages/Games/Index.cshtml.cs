using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SERVER.Data;
using SERVER.Models;

namespace SERVER.Pages.Games
{
    public class IndexModel : PageModel
    {
        private readonly SERVER.Data.SERVERContext _context;

        public IList<TblGames> TblGames { get; set; } = default!;
        public IList<TblPlayers> TblPlayers { get; set; } = default!;
        public List<string> PlayersNames { get; set; }

        public IndexModel(SERVER.Data.SERVERContext context)
        {
            _context = context;
            PlayersNames = _context.TblPlayers.Select(p => p.Name.ToLower()).Distinct().OrderBy(name => name).ToList();
        }



        public async Task OnGetAsync()
        {
            if (_context.TblGames != null)
            {
                TblGames = await _context.TblGames
                    .Include(t => t.Player).ToListAsync();
            }
            if (_context.TblPlayers != null)
            {
                TblPlayers = await _context.TblPlayers.ToListAsync();
            }
        }


        public async Task OnPostSortDefaultAsync()
        {
            if (_context.TblGames != null)
            {
                TblGames = await _context.TblGames
                    .Include(t => t.Player).ToListAsync();
            }
        }

        [BindProperty]
        public string Names {  get; set; }
        public async Task OnPostSortByNameAsync()
        {
            if (_context.TblGames != null)
            {
                TblGames = await _context.TblGames
                    .Include(t => t.Player).Where(g=> g.Player.Name.ToLower() == Names.ToLower()).ToListAsync();
            }
        }


    }
}
