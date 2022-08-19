using Microsoft.EntityFrameworkCore;
using AuctionMarketplaceLibrary;
using AuctionService.Models;

namespace AuctionService.Services {
    public class PgAuctionsDataBaseContext: DbContext {
        public DbSet<Auction>? Auctions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseNpgsql(GetConnectionString());
        }

        public string GetConnectionString() {
            return 
                $"Host={Config.AuctionServiceDataBaseHost};" +
                $"Port={Config.AuctionServiceDataBasePort};" +
                $"Database={Config.AuctionServiceDataBaseName};" +
                $"User ID={Config.AuctionServiceDataBaseUser};" +
                $"Password={Config.AuctionServiceDataBasePassword};";
        }
    }
}