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
    public class DetailsModel : PageModel
    {
        private readonly SERVER.Data.SERVERContext _context;

        public DetailsModel(SERVER.Data.SERVERContext context)
        {
            _context = context;
        }

        public TblGames TblGames { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblgames = await _context.TblGames.FirstOrDefaultAsync(m => m.Id == id);
            if (tblgames == null)
            {
                return NotFound();
            }
            else
            {
                TblGames = tblgames;
            }
            return Page();
        }
    }
}
