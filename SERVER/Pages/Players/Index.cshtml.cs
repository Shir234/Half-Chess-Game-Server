using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SERVER.Data;
using SERVER.Models;

namespace SERVER.Pages.Players
{
    public class IndexModel : PageModel
    {
        private readonly SERVER.Data.SERVERContext _context;

        public IndexModel(SERVER.Data.SERVERContext context)
        {
            _context = context;
        }

        public IList<TblPlayers> TblPlayers { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if(_context.TblPlayers != null)
            {
                TblPlayers = await _context.TblPlayers.ToListAsync();
            }
            
        }

        public async Task OnPostSortDefaultAsync()
        {
            if(_context.TblPlayers != null)
            {
                TblPlayers = await _context.TblPlayers.ToListAsync();
            }
        }

        public async Task OnPostSortAscendingAsync()
        {
            if (_context.TblPlayers != null)
            {
                TblPlayers = await _context.TblPlayers.OrderBy(p=> p.Name.ToLower()).ToListAsync();
            }
        }

        public async Task OnPostSortDescendingAsync()
        {
            if (_context.TblPlayers != null)
            {
                TblPlayers = await _context.TblPlayers.OrderByDescending(p => p.Name.ToLower()).ToListAsync();
            }
        }
    }
}
