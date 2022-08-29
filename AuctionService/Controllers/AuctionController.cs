using System;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using AuctionService.Models;
using AuctionService.Services;
using AuctionMarketplaceLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
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

        // [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddAuction([FromBody] ClientAuctionModel auction) {
            try {
                //TODO использовать библиотеку для построения запросов
                string text = "BEGIN;INSERT INTO Auctions (title, description, start_bid, start_time, finish_time, seller_id) " +
                              $"VALUES ({auction.ToInsertString()}) RETURNING id;";
                Func<int, Task<HttpResponseMessage>> getResponse = id => {
                    var data = new {id, auction.SellerId, auction.StartTime, auction.FinishTime, auction.StartBid};
                    var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
                    string url = $"http://{Config.AuctionLiveServiceHost}:{Config.AuctionLiveServicePort}/auction_live/add";
                    return _client.PostAsync(new Uri(url), content);
                };
                
                if (await AuctionCrudOperations.TryCreateAuction(_pgDataBase.GetConnectionString(), text, getResponse)) {
                    return Ok("Auction was successfully added.");
                } 
                
                return BadRequest("Auction was not added.");
            } catch {
                return BadRequest("Trouble creating new auction.");
            }
        }
        
        // [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuction(int id, [FromBody] ClientAuctionModel auction) {
            try {
                string text = "BEGIN;UPDATE Auctions SET (title, description, start_bid, start_time, finish_time) = " +
                              $"({auction.ToUpdateString()}) WHERE id = {id} RETURNING seller_id, is_active;";
                string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                (string clientId, string role) = JwtParser.GetClaims(token, Authenticator.TokenType.Access);
                await AuctionCrudOperations.ChangeAuction(_pgDataBase.GetConnectionString(), AuctionCrudOperations.ChangeType.Update,
                    text, clientId);
                
                return Ok("Auction was successfully updated.");
            } catch (InvalidOperationException exception) {
                if (exception.Message == "No row is available") {
                    return NotFound("Auction with this id does not exist.");
                }
                
                return BadRequest(exception.Message);
            }
        }

        // [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuction(int id) {
            try {
                string text = $"BEGIN;DELETE FROM Auctions WHERE id = {id} RETURNING seller_id, is_active;";
                string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                (string clientId, string role) = JwtParser.GetClaims(token, Authenticator.TokenType.Access);
                await AuctionCrudOperations.ChangeAuction(_pgDataBase.GetConnectionString(), AuctionCrudOperations.ChangeType.Delete,
                    text, clientId);
                
                return Ok("Auction was successfully deleted.");
            } catch (InvalidOperationException exception) {
                if (exception.Message == "No row is available") {
                    return NotFound("Auction with this id does not exist.");
                }
                
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuction(int id) {
            var auction = await (from dbAuction in _pgDataBase.Auctions
                where dbAuction.Id == id
                select dbAuction).SingleOrDefaultAsync();

            if (auction is null) {
                return NotFound("Auction with this id does not exist.");
            }
            
            return Ok(auction);
        }

        [HttpGet("all")]
        public IEnumerable<Auction> GetAllAuctions([FromQuery] bool onlyActive) {
            //TODO использовать butch запросы
            return from auction in _pgDataBase.Auctions where !onlyActive || auction.IsActive select auction;
        }

        [HttpPatch("{id}/set")]
        public async Task<IActionResult> UpdateAuctionActivityInformation(int id, [FromQuery] bool active) {
            using (var connection = new NpgsqlConnection(_pgDataBase.GetConnectionString())) {
                await connection.OpenAsync();
                string text = $"BEGIN;UPDATE Auctions SET is_active = {active} WHERE id = {id};COMMIT;";
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