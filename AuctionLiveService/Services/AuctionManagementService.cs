using System;
using System.Collections.Concurrent;
using AuctionLiveService.Models;

namespace AuctionLiveService.Services {
    public class AuctionManagementService {
        public readonly ConcurrentDictionary<int, Auction> Auctions;
        public readonly ConcurrentDictionary<int, ConcurrentQueue<Bid>> AuctionQueues;

        public AuctionManagementService() {
            Auctions = new ConcurrentDictionary<int, Auction>();
            AuctionQueues = new ConcurrentDictionary<int, ConcurrentQueue<Bid>>();
        }

        public void Add(Auction auction) {
            if (!Auctions.ContainsKey(auction.Id)) {
                Auctions.TryAdd(auction.Id, auction);
                AuctionQueues.TryAdd(auction.Id, new ConcurrentQueue<Bid>());
            } else {
                throw new InvalidOperationException("Auction with this id already exists.");
            }
        }
    }
}