using System;
using System.Collections.Concurrent;
using AuctionLiveService.Models;

namespace AuctionLiveService.Services {
    public class AuctionManagementService {
        public readonly ConcurrentDictionary<string, Action> AuctionTimeEvents;

        public AuctionManagementService() {
            AuctionTimeEvents = new ConcurrentDictionary<string, Action>();
        }

        public void Add(int id, string time, bool activityStatus, AuctionAlertService alertService) {
            Action action = () => {
                alertService.SendAuctionActivationAlert(id, activityStatus).GetAwaiter().GetResult();
            };

            if (!AuctionTimeEvents.ContainsKey(time)) {
                AuctionTimeEvents.TryAdd(time, action);
            } else {
                AuctionTimeEvents[time] += action;
            }
        }
    }
}