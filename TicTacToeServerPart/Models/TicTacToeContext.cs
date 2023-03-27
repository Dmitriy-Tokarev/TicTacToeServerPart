using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TicTacToeServerPart.Models
{
    public class TicTacToeContext : DbContext
    {
        public TicTacToeContext(DbContextOptions<TicTacToeContext> options)
            : base(options)
        {
        }

        public DbSet<PlayerModel> Players { get; set; } = null!;
        public DbSet<LoginModel> Login { get; set; } = null!;
        public DbSet<InGameLogicModel> InGameLogic { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerModel>()
                .HasIndex(player => player.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<LoginModel>()
                .HasIndex(loginInfo => loginInfo.EmailAddress)
                .IsUnique();
        }
    }
}
