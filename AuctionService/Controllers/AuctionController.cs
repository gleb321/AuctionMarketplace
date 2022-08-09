using System;
using System.Linq;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AuctionMarketplaceLibrary;
using Microsoft.AspNetCore.Mvc;
using AuctionService.Models;
using AuctionService.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace AuctionService.Controllers {
    [Route("/auction/")]
    public class AuctionController : Controller {
        private PgAuctionsDataBaseContext _pgDataBase;
        public AuctionController(PgAuctionsDataBaseContext pgDataBase) {
            _pgDataBase = pgDataBase;
        }
        
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] RegistrationModel auction) {
            int id;
            try {
                using (var connection = new NpgsqlConnection(_pgDataBase.GetConnectionString())) {
                    await connection.OpenAsync();
                    string text = "INSERT INTO Auctions (title, description, start_bid, start_time, finish_time, seller_id) " +
                                  $"VALUES ({auction}) RETURNING id;";
                    var command = new NpgsqlCommand(text, connection);
                    using (var reader = await command.ExecuteReaderAsync()) {
                        await reader.ReadAsync();
                        id = (int) reader.GetValue(0);
                    }
                }
            } catch (Exception exception) {
                return BadRequest("Trouble creating new auction.");
            }
            
            var data = new {id, auction.SellerId, auction.StartTime, auction.FinishTime, auction.StartBid};
            string json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            string url = $"http://{Config.Host}:{Config.AuctionLiveServicePort}/auction_live/add";
            var response = await new HttpClient().PostAsync(new Uri(url), content);
            if (response.StatusCode == HttpStatusCode.OK) {
                return Ok("Auction was successfully added.");
            }
            
            
            return BadRequest("Auction was not added.");
        }
    }
}