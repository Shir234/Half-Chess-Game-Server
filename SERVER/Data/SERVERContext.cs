using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SERVER.Models;

namespace SERVER.Data
{
    public class SERVERContext : DbContext
    {
        public SERVERContext (DbContextOptions<SERVERContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TblGames>()
                .HasOne(g => g.Player)
                .WithMany(p => p.Games).HasForeignKey(g => g.PlayerId);
        }

        public DbSet<SERVER.Models.TblPlayers> TblPlayers { get; set; } = default!;
        public DbSet<SERVER.Models.TblGames> TblGames { get; set; } = default!;

    }
}
