using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using AuctionLiveService.Models;

namespace AuctionLiveService.Services {
    public class AuctionManagementService {
        public readonly ConcurrentDictionary<string, Func<Task>> AuctionTimeEvents;

        public AuctionManagementService() {
            AuctionTimeEvents = new ConcurrentDictionary<string, Func<Task>>();
        }

        public void Add(int id, string time, bool activityStatus, AuctionAlertService alertService) {
            Func<Task> sendAlert = async () => {
                await alertService.SendAuctionActivationAlert(id, activityStatus);
            };

            if (!AuctionTimeEvents.ContainsKey(time)) {
                AuctionTimeEvents.TryAdd(time, sendAlert);
            } else {
                AuctionTimeEvents[time] += sendAlert;
            }
        }
    }
}