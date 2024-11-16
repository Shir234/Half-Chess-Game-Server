using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SERVER.Data;
using SERVER.Models;

namespace SERVER.Pages.Players
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
            PopulateCountryList();
            return Page();
        }

        [BindProperty]
        public TblPlayers TblPlayers { get; set; }

        public List<SelectListItem> CountryList { get; set; }

        private void PopulateCountryList()
        {
            CountryList = new List<SelectListItem>
            {
                new SelectListItem { Value = "USA", Text = "United States" },
                new SelectListItem { Value = "UK", Text = "United Kingdom" },
                new SelectListItem { Value = "CA", Text = "Canada" },
                new SelectListItem { Value = "AU", Text = "Australia" },
                new SelectListItem { Value = "FR", Text = "France" },
                new SelectListItem { Value = "DE", Text = "Germany" },
                new SelectListItem { Value = "IT", Text = "Italy" },
                new SelectListItem { Value = "ES", Text = "Spain" },
                new SelectListItem { Value = "PT", Text = "Portugal" },
                new SelectListItem { Value = "IL", Text = "Israel" },
                // Add more countries as needed
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PopulateCountryList();
                return Page();
            }

            _context.TblPlayers.Add(TblPlayers);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
          // return new EmptyResult();
        }
    }
}
