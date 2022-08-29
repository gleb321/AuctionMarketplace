using System;
using System.Net.Http;
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
            services.AddSingleton<HttpClient>();
            services.AddDbContext<PgAuctionsDataBaseContext>();
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
            
            //TODO нормально настроить CORS
            app.UseCors(builder => {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
            
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints => {
                 endpoints.MapControllers();
            });
        }
    }
}