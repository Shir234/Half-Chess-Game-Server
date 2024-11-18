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

        public IndexModel(SERVER.Data.SERVERContext context)
        {
            _context = context;
        }

        public IList<TblGames> TblGames { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.TblGames != null)
            {
                TblGames = await _context.TblGames
                    .Include(t => t.Player).ToListAsync();
            }
        }
    }
}
