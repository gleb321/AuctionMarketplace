using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.Mvc;
using AuctionLiveService.Models;
using AuctionLiveService.Services;

namespace AuctionLiveService.Controllers {
    [ApiController]
    [Route("/auction_live/")]
    public class AuctionLiveController: Controller {
        private readonly AuctionAlertService _auctionAlertService;
        private readonly AuctionManagementService _managementService;

        public AuctionLiveController(AuctionManagementService managementService,  AuctionAlertService alertService) {
            _managementService = managementService;
            _auctionAlertService = alertService;
        }
        
        [HttpPost("add")]
        public IActionResult Add([FromBody] Auction auction) {
            _managementService.Add(auction.Id, auction.StartTime, true, _auctionAlertService);
            _managementService.Add(auction.Id, auction.FinishTime, false, _auctionAlertService);
            
            return Ok("Auction was successfully added.");
        }
    }
}