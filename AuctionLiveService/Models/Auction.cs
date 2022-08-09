using System;

namespace AuctionLiveService.Models {
    public class Auction {
        public Auction(int id, string sellerId, decimal bid, DateTime startTime, DateTime finishTime) {
            (Id, SellerId, StartTime, FinishTime, CurrentBid) = (id, sellerId, startTime, finishTime, bid);
            IsActive = startTime <= DateTime.Now && finishTime > DateTime.Now;
        }
        
        public int Id { get; set; }
        public string SellerId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public decimal CurrentBid { get; set; }
        public bool IsActive { get; set; }

        public override string ToString() {
            return $"{Id} {SellerId} {StartTime} - {FinishTime} {IsActive}";
        }
    }
}