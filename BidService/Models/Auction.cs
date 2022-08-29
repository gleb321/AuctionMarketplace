using System.ComponentModel.DataAnnotations;

namespace BidService.Models {
    public class Auction {
        public Auction(int id, string owner, decimal startBid, bool isActive = false) {
            (Id, Owner, IsActive, CurrentBid) = (id, owner, isActive, startBid);
        }
        
        public int Id { get; init; }
        public string Owner { get; init; }
        public bool IsActive { get; set; }
        public decimal CurrentBid { get; set; }
    }
}