using System;
using System.Threading.Tasks;
using BidService.Models;
using BidService.Services;
using Microsoft.AspNetCore.Mvc;

namespace BidService.Controllers {
    public class BidController: Controller {
        private readonly BidPlacerService _bidPlacer;
        
        public BidController(BidPlacerService bidPlacer) {
            _bidPlacer = bidPlacer;
        }
        
        [HttpPost("make_bid")]
        public async Task<IActionResult> MakeBid([FromQuery] Bid bid) {
            try {
                await _bidPlacer.PlaceBid(bid);
            } catch (InvalidOperationException invalidOperationException) {
                return BadRequest(invalidOperationException.Message);
            } catch (ArgumentException argumentException) {
                return BadRequest(argumentException.Message);
            }

            return Ok("Bid was successfully made.");
        }
    }
}