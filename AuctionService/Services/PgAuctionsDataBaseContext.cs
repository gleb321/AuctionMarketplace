using Microsoft.EntityFrameworkCore;
using AuctionMarketplaceLibrary;
using AuctionService.Models;

namespace AuctionService.Services {
    public class PgAuctionsDataBaseContext: DbContext {
        public DbSet<Auction>? Auctions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseNpgsql($"Host={Config.DataBaseHost};" +
                                     $"Port={Config.DataBasePort};" +
                                     $"Database={Config.AuctionServiceDataBaseName};" +
                                     $"User ID={Config.DataBaseUser};" +
                                     $"Password={Config.DataBasePassword};");
        }
    }
}