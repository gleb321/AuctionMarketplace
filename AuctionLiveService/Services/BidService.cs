using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using AuctionLiveService.Models;

namespace AuctionLiveService.Services {
    public class BidService {
        private ConcurrentDictionary<int, Auction> _auctions;
        private ConcurrentDictionary<int, ConcurrentQueue<Bid>> _auctionQueues;

        public BidService(ConcurrentDictionary<int, Auction> auctions,
            ConcurrentDictionary<int, ConcurrentQueue<Bid>> auctionQueues) {
            _auctions = auctions;
            _auctionQueues = auctionQueues;
        }

        public Task MakeBid(Bid bid) {
            return new Task(() => {
                var queue = _auctionQueues[bid.AuctionId];
                queue.Enqueue(bid);
                while (!queue.First().Equals(bid)) {}

                lock (_auctions[bid.AuctionId]) {
                    queue.TryDequeue(out var unused);
                    if (_auctions[bid.AuctionId].IsActive) {
                        if (bid.Value > _auctions[bid.AuctionId].CurrentBid) {
                            _auctions[bid.AuctionId].CurrentBid = bid.Value;
                        } else {
                            throw new ArgumentException("New bid value must be more than current bid value.");
                        }
                    } else {
                        throw new InvalidOperationException("This auction has not stated yet.");
                    }
                }
            });
        }
    }
}