using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SERVER.Models;

namespace SERVER.Pages.Players
{
    public class LoginModel : PageModel
    {
        private readonly SERVER.Data.SERVERContext _context;

        public LoginModel(SERVER.Data.SERVERContext context)
        {
            _context = context;
        }


        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public string Name { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Check if player exists with the entered ID and Name
            var player = _context.TblPlayers.FirstOrDefault(p => p.Id == Id && p.Name == Name);
            Console.WriteLine("Player: " + player);
            if (player == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid ID or Name. Please try again.");
                return Page();
            }

            // Login successful - redirect to another page or set up session
            return RedirectToPage("/Index"); // Redirect to the homepage or another page
        }
    }
}
