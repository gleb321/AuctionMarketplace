using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR.Client;
using BidService.Models;

namespace BidService.Services {
    public class BidPlacerService {
        private HubConnection _connection;
        private ConcurrentDictionary<int, Auction> _auctions;
        private ConcurrentDictionary<int, ConcurrentQueue<Bid>> _auctionQueues;

        public BidPlacerService(HubConnection connection) {
            _connection = connection;
            _auctions = new ConcurrentDictionary<int, Auction>();
            _auctionQueues = new ConcurrentDictionary<int, ConcurrentQueue<Bid>>();
        }

        public void AddAuctionForBids(Auction auction) {
            if (!_auctions.ContainsKey(auction.Id)) {
                _auctions.TryAdd(auction.Id, auction);
            } else {
                throw new InvalidOperationException("Auction with this id already exists.");
            }
        }
        
        public void SetAuctionAvailableForBids(int id) {
            if (_auctions.ContainsKey(id)) {
                _auctionQueues.TryAdd(id, new ConcurrentQueue<Bid>());
                _auctions[id].IsActive = true;
            } else {
                throw new InvalidOperationException("Auction with this id does not exist.");
            }
        }

        public async Task PlaceBid(Bid bid) {
            if (!_auctions.ContainsKey(bid.AuctionId)) {
                throw new ArgumentException("Auction with this id does not exist.");
            }
            
            if (!_auctions[bid.AuctionId].IsActive) {
                throw new InvalidOperationException("Auction with this id has not started yet.");
            }
            
            var queue = _auctionQueues[bid.AuctionId];
            queue.Enqueue(bid);
            while (!queue.First().Equals(bid)) {}

            lock (_auctions[bid.AuctionId]) {
                queue.TryDequeue(out var unused);
                if (bid.Value > _auctions[bid.AuctionId].CurrentBid) {
                    _auctions[bid.AuctionId].CurrentBid = bid.Value;
                } else {
                    throw new InvalidOperationException("New bid value must be more than current bid value.");
                }
                    
            }

            if (_connection.State == HubConnectionState.Disconnected) {
                await _connection.StartAsync();
            }
            
            await _connection.SendAsync("SendBidValueAsync", bid.AuctionId, bid.Value);
        }
    }
}