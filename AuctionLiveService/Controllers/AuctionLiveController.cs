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
        private BidService _bidService;
        
        public AuctionLiveController(AuctionManagementService auctionManagementService, BidService bidService) {
            _auctionManagementService = auctionManagementService;
            _bidService = bidService;
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

        [HttpPost("make_bid")]
        public async Task<IActionResult> MakeBid([FromQuery] Bid bid) {
            try {
                await _bidService.MakeBid(bid);
                
            } catch (InvalidOperationException invalidOperationException) {
                return BadRequest(invalidOperationException.Message);
            } catch (ArgumentException argumentException) {
                return BadRequest(argumentException.Message);
            }

            return Ok("Bid was successfully made.");
        }

        [HttpGet("is_active")]
        public IActionResult IsActive([FromQuery] int id) {
            if (!_auctionManagementService.Auctions.ContainsKey(id)) {
                return NotFound("Auction with this id does not exist.");
            }
            
            return Ok(_auctionManagementService.Auctions[id].IsActive);
        }

        [HttpGet("get_bid")]
        public IActionResult GetCurrentBid([FromQuery] int id) {
            return Ok();
        }
    }
}