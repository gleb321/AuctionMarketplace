using System;
using System.ComponentModel.DataAnnotations;

namespace BidService.Models {
    public class Auction {
        [Range(1, Int32.MaxValue)]
        public int Id { get; init; }
        [Required]
        [EmailAddress]
        public string? Owner { get; init; }
        public bool IsActive { get; set; }
        [Range(1, 1000000)]
        public decimal CurrentBid { get; set; }
    }
}