using System.ComponentModel.DataAnnotations;

namespace BidService.Models {
    public class Bid {
        [Required]
        public int AuctionId { get; set; }
        [Required]
        public decimal Value { get; set; }
    }
}