using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SERVER.Data;
using SERVER.Models;

namespace SERVER.Pages.Games
{
    public class CreateModel : PageModel
    {
        private readonly SERVER.Data.SERVERContext _context;

        public CreateModel(SERVER.Data.SERVERContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["PlayerId"] = new SelectList(_context.TblPlayers, "Id", "Country");
            return Page();
        }

        [BindProperty]
        public TblGames TblGames { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.TblGames.Add(TblGames);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
