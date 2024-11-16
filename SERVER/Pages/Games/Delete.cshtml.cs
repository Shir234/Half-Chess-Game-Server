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
    public class DeleteModel : PageModel
    {
        private readonly SERVER.Data.SERVERContext _context;

        public DeleteModel(SERVER.Data.SERVERContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblgames = await _context.TblGames.FindAsync(id);
            if (tblgames != null)
            {
                TblGames = tblgames;
                _context.TblGames.Remove(TblGames);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
