using System;
using System.ComponentModel.DataAnnotations;

namespace BidService.Models {
    public class Bid {
        [Range(1, Int32.MaxValue)]
        public int AuctionId { get; set; }
        [Range(1, 10000)]
        public decimal Value { get; set; }
    }
}