using Microsoft.EntityFrameworkCore;
using AuctionMarketplaceLibrary;
using AuthenticationServer.Models;

namespace AuthenticationServer.Services {
    public class PgUsersDataBaseContext: DbContext {
        public DbSet<User>? Users { get; set; }
        public DbSet<Account>? Accounts { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseNpgsql($"Host={Config.UsersDataBaseHost};" +
                                     $"Port={Config.UsersDataBasePort};" +
                                     $"Database={Config.AuthenticationServerDataBaseName};" +
                                     $"User ID={Config.UsersDataBaseUser};" +
                                     $"Password={Config.UsersDataBasePassword};");
        }
    }
}