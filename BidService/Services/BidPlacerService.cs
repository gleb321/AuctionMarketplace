using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using BidService.Models;

namespace BidService.Services {
    public class BidPlacerService {
        private ConcurrentDictionary<int, Auction> _auctions;
        private ConcurrentDictionary<int, ConcurrentQueue<Bid>> _auctionQueues;

        public BidPlacerService() {
            _auctions = new ConcurrentDictionary<int, Auction>();
            _auctionQueues = new ConcurrentDictionary<int, ConcurrentQueue<Bid>>();
        }

        public Task PlaceBid(Bid bid) {
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