using System;
using System.Threading.Tasks;
using BidService.Models;
using BidService.Services;
using Microsoft.AspNetCore.Mvc;

namespace BidService.Controllers {
    [ApiController]
    [Route("/bid/")]
    public class BidController: Controller {
        private readonly BidPlacerService _bidPlacer;
        
        public BidController(BidPlacerService bidPlacer) {
            _bidPlacer = bidPlacer;
        }
        
        [HttpPost("place")]
        public async Task<IActionResult> PlaceBid([FromBody] Bid bid) {
            try {
                await _bidPlacer.PlaceBid(bid);
            } catch (InvalidOperationException invalidOperationException) {
                return BadRequest(invalidOperationException.Message);
            } catch (ArgumentException argumentException) {
                return BadRequest(argumentException.Message);
            }

            return Ok("Bid was successfully made.");
        }

        [HttpPost("add")]
        public IActionResult AddAuction([FromBody] Auction auction) {
            try {
                _bidPlacer.AddAuctionForBids(auction);
            } catch (InvalidOperationException exception) {
                return BadRequest(exception.Message);
            }

            return Ok("Auction was successfully added.");
        }

        [HttpPost("set/active")]
        public IActionResult ActivateAuction(int id) {
            try {
                _bidPlacer.SetAuctionAvailableForBids(id);
            } catch (InvalidOperationException exception) {
                return NotFound(exception.Message);
            }
            
            return Ok("Auction was successfully activated");
        }
    }
}