using Microsoft.AspNetCore.Mvc;

namespace AuctionService {
    [Route("/auction/")]
    public class AuctionController : Controller {
        [HttpPost("add")]
        public IActionResult Add() {
            return Ok();
        }
    }
}