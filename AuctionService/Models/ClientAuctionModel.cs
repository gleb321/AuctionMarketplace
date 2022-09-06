using System.ComponentModel.DataAnnotations;

namespace AuctionService.Models {
    public class ClientAuctionModel {
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public string StartTime { get; set; }
        
        [Required]
        public string FinishTime { get; set; }
        
        public string? SellerId { get; set; }
        
        [Range(1, 1000000)]
        public decimal StartBid { get; set; }
        
        public string? ImagePath { get; set; }

        public string ToUpdateString() {
            return $"'{Title}', '{Description}', {StartBid}, '{StartTime}', '{FinishTime}'";
        }

        public string ToInsertString() {
            return $"'{Title}', '{Description}', {StartBid}, '{StartTime}', '{FinishTime}', '{SellerId}'";
        }
    }
}