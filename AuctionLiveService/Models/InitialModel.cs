namespace AuctionLiveService.Models {
    public class InitialModel {
        public int Id { get; set; }
        public string SellerId { get; set; }
        public string StartTime { get; set; }
        public string FinishTime { get; set; }
        public decimal StartBid { get; set; }
    }
}