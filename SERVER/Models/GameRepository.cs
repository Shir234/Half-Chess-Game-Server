using Microsoft.AspNetCore.Mvc;

namespace SERVER.Models
{
    public class GameRepository
    {
        private readonly SERVER.Data.SERVERContext _context;

        public GameRepository(SERVER.Data.SERVERContext context)
        {
            _context = context;
        }

    //    [BindProperty]
   //     public TblGames TblGames { get; set; } = default!;

        public async Task SaveGameAsync(TblGames game)
        {

            _context.TblGames.Add(game);
            await _context.SaveChangesAsync();
        }
    }
}
