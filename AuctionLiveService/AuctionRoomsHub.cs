using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace AuctionLiveService {
    public class AuctionRoomsHub: Hub {
        public async Task SendBidValueAsync(int auctionId, decimal value) {
            await Clients.Group(auctionId.ToString()).SendAsync("BidValueChanged", value.ToString(CultureInfo.InvariantCulture));
        }
        
        public override async Task OnConnectedAsync() {
            string? auctionId = Context.GetHttpContext()?.Request.Query["auction_id"];
            Console.WriteLine($"New connection with id = {Context.ConnectionId} to auction with id = {auctionId}");
            if (auctionId != null)
                await Groups.AddToGroupAsync(Context.ConnectionId, auctionId);
            
            await base.OnConnectedAsync();
        }
    }
}