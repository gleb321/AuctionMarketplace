using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionService.Models {
    public class Auction {
        [Key]
        [Column("id")]
        [Required]
        public uint Id { get; set; }
        
        [Column("title")]
        [Required]
        public string? Title { get; set; }
        
        [Column("description")]
        public string? Description { get; set; }
        
        [Column("start_bid")]
        [Required]
        public decimal StartBid { get; set; }
        
        [Column("last_bid")]
        public decimal LastBid { get; set; }
        
        [Column("start_time")]
        [Required]
        public DateTime StartTime { get; set; }
        
        [Column("end_time")]
        [Required]
        public DateTime FinishTime { get; set; }

        public override string ToString() {
            return $"{Id}\n{Title}\n{Description}\n{StartTime} - {FinishTime}\n{StartBid}$ -> {LastBid}$";
        }
    }
}