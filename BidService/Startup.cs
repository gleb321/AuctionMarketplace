using AuctionMarketplaceLibrary;
using BidService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BidService {
    public class Startup {
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();
            services.AddSingleton<BidPlacerService>(provider => new BidPlacerService(
                new HubConnectionBuilder().WithUrl(
                    $"http://{Config.AuctionLiveServiceHost}:{Config.AuctionLiveServicePort}/auction_live/connect"
                    ).WithAutomaticReconnect().Build()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();
            app.UseEndpoints(endpoints => {
                 endpoints.MapControllers();
            });
        }
    }
}