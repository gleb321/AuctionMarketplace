using System;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AuctionService.Models;
using AuctionService.Services;
using AuctionMarketplaceLibrary;
using Microsoft.AspNetCore.Mvc;
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
            try {
                using (var connection = new NpgsqlConnection(_pgDataBase.GetConnectionString())) {
                    await connection.OpenAsync();
                    string text = "BEGIN;INSERT INTO Auctions (title, description, start_bid, start_time, finish_time, seller_id) " +
                                  $"VALUES ({auction}) RETURNING id;";
                    var command = new NpgsqlCommand(text, connection);
                    int id;
                    await using (var reader = await command.ExecuteReaderAsync()) {
                        await reader.ReadAsync();
                        id = (int) reader.GetValue(0);
                    }
                    
                    var data = new {id, auction.SellerId, auction.StartTime, auction.FinishTime, auction.StartBid};
                    var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
                    string url = $"http://{Config.Host}:{Config.AuctionLiveServicePort}/auction_live/add";
                    var response = await new HttpClient().PostAsync(new Uri(url), content);
                    if (response.StatusCode == HttpStatusCode.OK) {
                        command.CommandText = "COMMIT;";
                        await command.ExecuteNonQueryAsync();
                        return Ok("Auction was successfully added.");
                    } 
                    
                    command.CommandText = "ROLLBACK;";
                    await command.ExecuteNonQueryAsync();
                    return BadRequest("Auction was not added.");
                }
            } catch (Exception exception) {
                return BadRequest("Trouble creating new auction.");
            }
        }
    }
}