using System.Net.Http;
using AuctionLiveService.Hubs;
using AuctionLiveService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionLiveService {
    public class Startup {
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();
            services.AddSignalR();
            services.AddSingleton<HttpClient>();
            services.AddSingleton<AuctionManagementService>(new AuctionManagementService(new AuctionAlertService(), 1));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();
            app.UseEndpoints(endpoints => {
                 endpoints.MapControllers();
                 endpoints.MapHub<AuctionRoomsHub>("/auction_live/connect");
            });
        }
    }
}