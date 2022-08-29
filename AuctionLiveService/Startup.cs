using System;
using System.Net.Http;
using AuctionLiveService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionLiveService {
    public class Startup {
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();
            services.AddSingleton<AuctionManagementService>();
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