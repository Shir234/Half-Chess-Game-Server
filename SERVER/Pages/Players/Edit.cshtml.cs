using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SERVER.Data;
using SERVER.Models;

namespace SERVER.Pages.Players
{
    public class EditModel : PageModel
    {
        private readonly SERVER.Data.SERVERContext _context;

        public EditModel(SERVER.Data.SERVERContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TblPlayers TblPlayers { get; set; } = default!;
        public List<SelectListItem> CountryList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var tblplayers =  await _context.TblPlayers.FirstOrDefaultAsync(m => m.Id == id);
            //if (tblplayers == null)
            //{
            //    return NotFound();
            //}
            //TblPlayers = tblplayers;
            TblPlayers = await _context.TblPlayers.FirstOrDefaultAsync(m => m.Id == id);
            if (TblPlayers == null)
            {
                return NotFound();
            }
            
            PopulateCountryList();
            return Page();
        }

        private void PopulateCountryList()
        {
            //CountryList = new List<SelectListItem>
            //{
            //    new SelectListItem { Value = "United States", Text = "United States" },
            //    new SelectListItem { Value = "United Kingdom", Text = "United Kingdom" },
            //    new SelectListItem { Value = "Canada", Text = "Canada" },
            //    new SelectListItem { Value = "Australia", Text = "Australia" },
            //    new SelectListItem { Value = "France", Text = "France" },
            //    new SelectListItem { Value = "Germany", Text = "Germany" },
            //    new SelectListItem { Value = "Italy", Text = "Italy" },
            //    new SelectListItem { Value = "Spain", Text = "Spain" },
            //    new SelectListItem { Value = "Portugal", Text = "Portugal" },
            //    new SelectListItem { Value = "Israel", Text = "Israel" },
            //    // Add more countries as needed
            //};
            //CountryList = await _context.TblPlayers
            //    .Select(player => player.Country)
            //    .Distinct()
            //    .OrderBy(c => c)
            //    .Select(c => new SelectListItem
            //    {
            //        Text = c,
            //        Value = c
            //    })
            //    .ToListAsync();

            string selectedCountry = TblPlayers.Country; // The current country of the player

            // Each SelectListItem checks whether its the selected item
            CountryList = new List<SelectListItem>
            {
                new SelectListItem { Value = "United States", Text = "United States", Selected = (selectedCountry == "United States") },
                new SelectListItem { Value = "United Kingdom", Text = "United Kingdom", Selected = (selectedCountry == "United Kingdom") },
                new SelectListItem { Value = "Canada", Text = "Canada", Selected = (selectedCountry == "Canada") },
                new SelectListItem { Value = "Australia", Text = "Australia", Selected = (selectedCountry == "Australia") },
                new SelectListItem { Value = "France", Text = "France", Selected = (selectedCountry == "France") },
                new SelectListItem { Value = "Germany", Text = "Germany", Selected = (selectedCountry == "Germany") },
                new SelectListItem { Value = "Italy", Text = "Italy", Selected = (selectedCountry == "Italy") },
                new SelectListItem { Value = "Spain", Text = "Spain", Selected = (selectedCountry == "Spain") },
                new SelectListItem { Value = "Portugal", Text = "Portugal", Selected = (selectedCountry == "Portugal") },
                new SelectListItem { Value = "Israel", Text = "Israel", Selected = (selectedCountry == "Israel") },
                // Add more countries as needed
            };
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            // Remove ModelState error for the Id field (ALWAYS SAYS THAT THE ID EXIST)
            ModelState.Remove("TblPlayers.Id");
            if (!ModelState.IsValid)
            {
                PopulateCountryList();
                return Page();
            }

            _context.Attach(TblPlayers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblPlayersExists(TblPlayers.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TblPlayersExists(int id)
        {
            return _context.TblPlayers.Any(e => e.Id == id);
        }
    }
}
