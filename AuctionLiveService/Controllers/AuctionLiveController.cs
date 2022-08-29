using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuctionLiveService.Models;
using AuctionLiveService.Services;

namespace AuctionLiveService.Controllers {
    [Route("/auction_live/")]
    public class AuctionLiveController: Controller {
        private AuctionManagementService _auctionManagementService;

        public AuctionLiveController(AuctionManagementService auctionManagementService) {
            _auctionManagementService = auctionManagementService;
        }
        
        [HttpPost("add")]
        public IActionResult Add([FromBody] InitialModel auction) {
            try {
                _auctionManagementService.Add(new Auction(auction.Id, auction.SellerId, auction.StartBid,
                    DateTime.ParseExact(auction.StartTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                    DateTime.ParseExact(auction.FinishTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            } catch (InvalidOperationException invalidOperationException) {
                return BadRequest(invalidOperationException.Message);
            }
            
            
            return Ok("Auction was successfully added.");
        }
    }
}