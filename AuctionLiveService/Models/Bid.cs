using System.ComponentModel.DataAnnotations;

namespace AuctionLiveService.Models {
    public class Bid {
        [Required]
        public int AuctionId { get; set; }
        [Required]
        public decimal Value { get; set; }
    }
}