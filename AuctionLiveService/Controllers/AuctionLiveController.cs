using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuctionLiveService.Models;
using AuctionLiveService.Services;
using AuctionMarketplaceLibrary;

namespace AuctionLiveService.Controllers {
    [ApiController]
    [Route("/auction_live/")]
    public class AuctionLiveController: Controller {
        private readonly HttpClient _client;
        private readonly AuctionAlertService _auctionAlertService;
        private readonly AuctionManagementService _managementService;
        private readonly TimerService _timerService;

        public AuctionLiveController(AuctionManagementService managementService,  AuctionAlertService alertService, HttpClient client, TimerService timerService) {
            _managementService = managementService;
            _auctionAlertService = alertService;
            _client = client;
            _timerService = timerService;
        }
        
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] Auction auction) {
            _managementService.Add(auction.Id, auction.StartTime, true, _auctionAlertService);
            _managementService.Add(auction.Id, auction.FinishTime, false, _auctionAlertService);
            
            //TODO Подумать об использовании разных http клиентов
            var data = new {auction.Id, auction.SellerId, CurrentBid = auction.StartPrice};
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var url = $"http://{Config.BidServiceHost}:{Config.BidServicePort}/bid/add";
            var response = await _client.PostAsync(new Uri(url), content);
            if (response.StatusCode != HttpStatusCode.OK) {
                return BadRequest("Trouble adding auction to bid service.");
            }
            
            return Ok("Auction was successfully added.");
        }
    }
}