using System;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;
using AuctionService.Models;
using AuctionService.Services;
using AuctionMarketplaceLibrary;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace AuctionService.Controllers {
    [Route("/auction/")]
    public class AuctionController: Controller {
        private PgAuctionsDataBaseContext _pgDataBase;
        private HttpClient _client;
        
        public AuctionController(PgAuctionsDataBaseContext pgDataBase, HttpClient client) {
            _pgDataBase = pgDataBase;
            _client = client;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAuction([FromBody] ClientAuctionModel auction) {
            try {
                //TODO использовать библиотеку для построения запросов
                string text = "BEGIN;INSERT INTO Auctions (title, description, start_bid, start_time, finish_time, seller_id) " +
                              $"VALUES ({auction}) RETURNING id;";
                Func<int, Task<HttpResponseMessage>> getResponse = id => {
                    var data = new {id, auction.SellerId, auction.StartTime, auction.FinishTime, auction.StartBid};
                    var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
                    string url = $"http://{Config.Host}:{Config.AuctionLiveServicePort}/auction_live/add";
                    return _client.PostAsync(new Uri(url), content);
                };
                
                if (await AuctionCrudOperations.TryCreateAuction(_pgDataBase.GetConnectionString(), text, getResponse)) {
                    return Ok("Auction was successfully added");
                } 
                
                return BadRequest("Auction was not added.");
            } catch {
                return BadRequest("Trouble creating new auction.");
            }
        }
        
        [HttpPost("update")]
        public async Task<IActionResult> UpdateAuction([FromQuery] int id, [FromBody] ClientAuctionModel auction) {
            try {
                string text = "BEGIN;UPDATE Auctions SET (title, description, start_bid, start_time, finish_time, seller_id) = " +
                              $"({auction}) WHERE id = {id} RETURNING seller_id, is_active;";
                await AuctionCrudOperations.ChangeAuction(_pgDataBase.GetConnectionString(), text, AuctionCrudOperations.ChangeType.Update);
                return Ok("Auction was successfully updated.");
            } catch (InvalidOperationException exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAuction([FromQuery] int id) {
            try {
                string text = $"BEGIN;DELETE FROM Auctions WHERE id = {id} RETURNING seller_id, is_active;";
                await AuctionCrudOperations.ChangeAuction(_pgDataBase.GetConnectionString(), text, AuctionCrudOperations.ChangeType.Delete);
                return Ok("Auction was successfully deleted.");
            } catch (InvalidOperationException exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("get/by_id")]
        public async Task<IActionResult> GetAuction([FromQuery] int id) {
            var auction = await (from dbAuction in _pgDataBase.Auctions
                where dbAuction.Id == id
                select dbAuction).SingleOrDefaultAsync();

            if (auction is null) {
                return NotFound("Auction with this id does not exist.");
            }
            
            return Ok(auction);
        }
        
        [HttpGet("get/all")]
        public IEnumerable<Auction> GetAllAuctions() {
            return from auction in _pgDataBase.Auctions select auction;
        }

        [HttpGet("get/all/active")]
        public IEnumerable<Auction> GetAllActiveAuctions() {
            return from auction in _pgDataBase.Auctions where auction.IsActive select auction;
        }

        [HttpPost("set/active")]
        public async Task<IActionResult> UpdateAuctionActivityInformation([FromQuery] int id) {
            using (var connection = new NpgsqlConnection(_pgDataBase.GetConnectionString())) {
                await connection.OpenAsync();
                string text = $"BEGIN;UPDATE Auctions SET is_active = true WHERE id = {id};COMMIT;";
                var command = new NpgsqlCommand(text, connection);
                try {
                    command.ExecuteNonQuery();
                } catch (Exception exception) {
                    Console.WriteLine(exception.Message);
                }
            }
            
            return Ok("Auction was set active.");
        }
    }
}