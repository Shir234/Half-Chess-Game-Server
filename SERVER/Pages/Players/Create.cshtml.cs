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
                new SelectListItem { Value = "United States", Text = "United States" },
                new SelectListItem { Value = "United Kingdom", Text = "United Kingdom" },
                new SelectListItem { Value = "Canada", Text = "Canada" },
                new SelectListItem { Value = "Australia", Text = "Australia" },
                new SelectListItem { Value = "France", Text = "France" },
                new SelectListItem { Value = "Germany", Text = "Germany" },
                new SelectListItem { Value = "Italy", Text = "Italy" },
                new SelectListItem { Value = "Spain", Text = "Spain" },
                new SelectListItem { Value = "Portugal", Text = "Portugal" },
                new SelectListItem { Value = "Israel", Text = "Israel" },
                // Add more countries as needed
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage); // Log or view the errors
                }

                PopulateCountryList();
                Console.WriteLine("we get here?");
                return Page();

            }

            _context.TblPlayers.Add(TblPlayers);
            await _context.SaveChangesAsync();
            Console.WriteLine("After add and sync");

            return RedirectToPage("./Index");
          // return new EmptyResult();
        }
    }
}
