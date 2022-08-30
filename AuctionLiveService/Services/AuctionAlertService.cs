using System;
using System.Net.Http;
using System.Threading.Tasks;
using AuctionMarketplaceLibrary;

namespace AuctionLiveService.Services {
    public class AuctionAlertService {
        private readonly HttpClient _client;
        private readonly string _bidServiceUrl;
        private readonly string _auctionServiceUrl;

        public AuctionAlertService() {
            _client = new HttpClient();
            _bidServiceUrl = $"http://{Config.BidServiceHost}:{Config.BidServicePort}/bid/set";
            _auctionServiceUrl = $"http://{Config.AuctionServiceHost}:{Config.AuctionServicePort}/auction";
        }

        public async Task SendAuctionActivationAlert(int id, bool status) {
            var content = new StringContent("");
            await Task.WhenAll(
                _client.PostAsync(new Uri($"{_bidServiceUrl}?id={id}&activityStatus={status}"), content),
                _client.PatchAsync(new Uri($"{_auctionServiceUrl}/{id}/set?activityStatus={status}"), content));
        }
    }
}