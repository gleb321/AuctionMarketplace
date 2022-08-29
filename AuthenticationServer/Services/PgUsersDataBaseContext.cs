using AuctionMarketplaceLibrary;
using Microsoft.EntityFrameworkCore;
using AuthenticationServer.Models;

namespace AuthenticationServer.Services {
    public class PgUsersDataBaseContext: DbContext {
        public DbSet<User>? Users { get; set; }
        public DbSet<Account>? Accounts { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseNpgsql($"Host={Config.AuthenticationServerDataBaseHost};" +
                                     $"Port={Config.AuthenticationServerDataBasePort};" +
                                     $"Database={Config.AuthenticationServerDataBaseName};" +
                                     $"User ID={Config.AuthenticationServerDataBaseUser};" +
                                     $"Password={Config.AuthenticationServerDataBasePassword};");
        }
    }
}