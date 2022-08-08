using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionService.Models {
    [Table("auctions")]
    public class Auction {
        [Key]
        [Column("id", TypeName = "serial")]
        [Required]
        public ulong Id { get; set; }
        
        [Column("title")]
        [Required]
        public string Title { get; set; }
        
        [Column("description")]
        [Required]
        public string Description { get; set; }
        
        [Column("start_bid")]
        [Required]
        public decimal StartBid { get; set; }
        
        [Column("last_bid")]
        public decimal? LastBid { get; set; }
        
        [Column("start_time")]
        [Required]
        public DateTime StartTime { get; set; }
        
        [Column("finish_time")]
        [Required]
        public DateTime FinishTime { get; set; }
        
        [Column("seller_id")]
        [Required]
        public string SellerId { get; set; }
        
        [Column("customer_id")]
        public string? CustomerId { get; set; }
        public override string ToString() {
            return $"{Id}\n{Title}\n{Description}\n{StartTime} - {FinishTime}\n{StartBid}$ -> {LastBid}$\n" + 
                   $"SellerId: {SellerId}\nCustomerId: {CustomerId}";
        }
    }
}