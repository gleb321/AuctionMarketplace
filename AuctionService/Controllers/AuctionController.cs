using System;
using System.Net;
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

        private enum OperationType {
            Update, 
            Delete
        }
        public AuctionController(PgAuctionsDataBaseContext pgDataBase, HttpClient client) {
            _pgDataBase = pgDataBase;
            _client = client;
        }
        
        /// <summary>
        /// Метод для запроса текущего состояния аукциона
        /// </summary>
        /// <param name="id">Id аукциона</param>
        /// <returns>Task, возвращающий сообщение о текщуем состоянии аукциона</returns>
        private Task<HttpResponseMessage> GetAuctionActivityInformation(int id) {
            string url = $"http://{Config.Host}:{Config.AuctionLiveServicePort}/auction_live/is_active?id={id}";
            return _client.GetAsync(url);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAuction([FromBody] ClientAuctionModel auction) {
            try {
                using (var connection = new NpgsqlConnection(_pgDataBase.GetConnectionString())) {
                    await connection.OpenAsync();
                    //TODO использовать библиотеку для построения запросов
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
                    var response = await _client.PostAsync(new Uri(url), content);
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
            return from auction in _pgDataBase.Auctions
                where auction.IsActive
                select auction;
        }

        [HttpPost("set/active")]
        public async Task<IActionResult> UpdateAuctionActivityInformation([FromQuery] int id) {
            using (var connection = new NpgsqlConnection(_pgDataBase.GetConnectionString())) {
                await connection.OpenAsync();
                string text = $"BEGIN;UPDATE Auctions SET is_active = true WHERE id = {id}; COMMIT;";
                var command = new NpgsqlCommand(text, connection);
                try {
                    command.ExecuteNonQuery();
                } catch (Exception exception) {
                    Console.WriteLine(exception.Message);
                }
            }
            
            return Ok("Auction was set active.");
        }
        
        [HttpPost("update")]
        public async Task<IActionResult> UpdateAuction([FromQuery] int id, [FromBody] ClientAuctionModel auction) {
            try {
                using (var connection = new NpgsqlConnection(_pgDataBase.GetConnectionString())) {
                    var responseSending = GetAuctionActivityInformation(id);
                    string text = "BEGIN;UPDATE Auctions SET (title, description, start_bid, start_time, finish_time, seller_id) = " +
                                  $"({auction}) WHERE id = {id} RETURNING seller_id;";
                    var command = new NpgsqlCommand(text, connection);
                    if (await TryExecuteTransaction(command, responseSending, "gleb@evlakhov.com")) {
                        return Ok("Auction was successfully updated.");
                    }
                    
                    return await RollbackTransaction(command, responseSending.Result, OperationType.Update);
                }
            } catch (Exception exception) {
                Console.WriteLine(exception.Message);
                return BadRequest("Trouble updating auction.");
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAuction([FromQuery] int id) {
            try { 
                using (var connection = new NpgsqlConnection(_pgDataBase.GetConnectionString())) {
                    var responseSending = GetAuctionActivityInformation(id);
                    string text = $"BEGIN;DELETE FROM Auctions WHERE id = {id} RETURNING seller_id;";
                    var command = new NpgsqlCommand(text, connection);
                    if (await TryExecuteTransaction(command, responseSending, "gleb@evlakhov.com")) {
                        return Ok("Auction was successfully deleted.");
                    }
                    
                    return await RollbackTransaction(command, responseSending.Result, OperationType.Delete);
                }
            } catch (Exception exception) {
                return BadRequest("Trouble deleting auction.");
            }
        }

        /// <summary>
        /// Метод, пробующий выполнить транзакцию
        /// </summary>
        /// <param name="command">SQL команда</param>
        /// <param name="responseSending">Task отправленный на AuctionLiveService, возвращающий информацию об активности аукциона</param>
        /// <param name="senderId">Id клиента, запросившего транзакцию</param>
        /// <returns>Получилось ли исполнить транзакцию</returns>
        private async Task<bool> TryExecuteTransaction(NpgsqlCommand command, Task<HttpResponseMessage> responseSending, string senderId) {
            string sellerId;
            await command.Connection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync()) {
                await reader.ReadAsync();
                sellerId = (string) reader.GetValue(0);
            }

            var response = await responseSending;
            if (response.StatusCode == HttpStatusCode.OK && !Boolean.Parse(await response.Content.ReadAsStringAsync()) &&
                sellerId == senderId) {
                command.CommandText = "COMMIT;";
                await command.ExecuteNonQueryAsync();
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Откатывает изменения в бд
        /// </summary>
        /// <param name="command">SQL команда</param>
        /// <param name="response">Ответ AuctionLiveService об активности аукциона</param>
        /// <param name="type">Тип транзакции</param>
        /// <returns>Сообщение с причиной отмены транзакции</returns>
        private async Task<IActionResult> RollbackTransaction(NpgsqlCommand command, HttpResponseMessage response, OperationType type) {
            command.CommandText = "ROLLBACK;";
            await command.ExecuteNonQueryAsync();
            if (response.StatusCode == HttpStatusCode.NotFound) {
                return NotFound("Auction with this id already finished or does not exist.");
            }
            
            if (Boolean.Parse(await response.Content.ReadAsStringAsync())) {
                return BadRequest($"Impossible to {type.ToString().ToLower()} running up auction.");
            }

            return BadRequest($"Only owner of the auction can {type.ToString().ToLower()} it.");
        }
    }
}