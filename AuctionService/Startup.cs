using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AuctionMarketplaceLibrary;
using AuctionService.Services;

namespace AuctionService {
    public class Startup {
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();
            services.AddDbContextFactory<PgAuctionsDataBaseContext>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = Authenticator.GetTokenValidationParameters(
                    Authenticator.TokenType.Access);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints => {
                 endpoints.MapControllers();
            });
        }
    }
}