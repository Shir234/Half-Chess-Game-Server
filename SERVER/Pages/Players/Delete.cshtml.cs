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
    public class DeleteModel : PageModel
    {
        private readonly SERVER.Data.SERVERContext _context;

        public DeleteModel(SERVER.Data.SERVERContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TblPlayers TblPlayers { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblplayers = await _context.TblPlayers.FirstOrDefaultAsync(m => m.Id == id);

            if (tblplayers == null)
            {
                return NotFound();
            }
            else
            {
                TblPlayers = tblplayers;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblplayers = await _context.TblPlayers.FindAsync(id);
            if (tblplayers != null)
            {
                TblPlayers = tblplayers;
                _context.TblPlayers.Remove(TblPlayers);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
