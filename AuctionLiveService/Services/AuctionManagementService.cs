using System;
using System.Collections.Concurrent;
using AuctionLiveService.Models;

namespace AuctionLiveService.Services {
    public class AuctionManagementService {
        private readonly ConcurrentDictionary<int, Auction> _auctions;

        public AuctionManagementService() {
            _auctions = new ConcurrentDictionary<int, Auction>();
        }

        public void Add(Auction auction) {
            if (!_auctions.ContainsKey(auction.Id)) {
                _auctions.TryAdd(auction.Id, auction);
            } else {
                throw new InvalidOperationException("Auction with this id already exists.");
            }
        }
    }
}