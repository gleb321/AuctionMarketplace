using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionService.Models {
    [Table("auctions")]
    public class Auction {
        [Key]
        [Column("id", TypeName = "serial")]
        public int Id { get; set; }
        
        [Column("title")]
        public string Title { get; set; }
        
        [Column("description")]
        public string Description { get; set; }
        
        [Column("start_bid")]
        public decimal StartBid { get; set; }
        
        [Column("last_bid")]
        public decimal? LastBid { get; set; }
        
        [Column("start_time")]
        public DateTime StartTime { get; set; }
        
        [Column("finish_time")]
        public DateTime FinishTime { get; set; }
        
        [Column("seller_id")]
        public string SellerId { get; set; }
        
        [Column("customer_id")]
        public string? CustomerId { get; set; }
        
        [Column("is_active")]
        public bool IsActive { get; set; }
        
        [Column("image_path")]
        public string? ImagePath { get; set; }
        public override string ToString() {
            return $"{Id}\n{Title}\n{Description}\n{StartTime} - {FinishTime}\n{StartBid}$ -> {LastBid}$\n" + 
                   $"SellerId: {SellerId}\nCustomerId: {CustomerId}";
        }
    }
}