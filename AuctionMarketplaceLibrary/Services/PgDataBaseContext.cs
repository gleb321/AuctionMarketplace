using AuctionMarketplaceLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionMarketplaceLibrary.Services {
    public class PgDataBaseContext: DbContext {
        public DbSet<User>? Users { get; set; }
        public DbSet<Account>? Accounts { get; set; }

        public DbSet<Auction>? Auctions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseNpgsql($"Host={Config.DataBaseHost};" +
                                     $"Port={Config.DataBasePort};" +
                                     $"Database={Config.DataBaseName};" +
                                     $"User ID={Config.DataBaseUser};" +
                                     $"Password={Config.DataBasePassword};");
        }
    }
}