using System;
using System.ComponentModel.DataAnnotations;

namespace AuctionLiveService.Models {
    public class Auction {
        [Range(1, Int32.MaxValue)]
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string SellerId { get; set; }
        [Required]
        public string StartTime { get; set; }
        [Required]
        public string FinishTime { get; set; }
        [Range(1, 1000000)]
        public decimal StartPrice { get; set; }
    }
}