using System.ComponentModel.DataAnnotations;

namespace AuctionService.Models {
    public class RegistrationModel {
        [Required]
        public string? Title { get; set; }
        
        [Required]
        public string? Description { get; set; }
        
        [Required]
        public string? StartTime { get; set; }
        
        [Required]
        public string? FinishTime { get; set; }
        
        [Required]
        public string? SellerId { get; set; }
        
        [Required]
        public decimal StartBid { get; set; }

        public override string ToString() {
            return $"'{Title}', '{Description}', {StartBid}, '{StartTime}', '{FinishTime}', '{SellerId}'";
        }
    }
}