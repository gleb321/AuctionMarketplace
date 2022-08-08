using System;
using System.Linq;
using System.Data.SqlClient;
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
            
            
            return Ok("Auction was successfully added.");
        }
    }
}