using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Npgsql;

namespace AuctionService {
    public static class AuctionCrudOperations {
        public enum ChangeType {
            Update, 
            Delete
        }
        
        public static async Task<bool> TryCreateAuction(string connectionString, string commandText, Func<int, Task<HttpResponseMessage>> getResponse) {
            HttpStatusCode statusCode;
            using (var connection = new NpgsqlConnection(connectionString)) {
                var command = new NpgsqlCommand(commandText, connection);
                await connection.OpenAsync();
                await using (var reader = await command.ExecuteReaderAsync()) {
                    await reader.ReadAsync();
                    statusCode = (await getResponse(reader.GetInt32(0))).StatusCode;
                }
                
                command.CommandText = statusCode == HttpStatusCode.OK ? "COMMIT;" : "ROLLBACK;";
                await command.ExecuteNonQueryAsync();
            }
            
            return statusCode == HttpStatusCode.OK;
        }
        
        public static async Task ChangeAuction(string connectionString, ChangeType type, string commandText, string clientId) {
            bool isActive;
            string sellerId;
            using (var connection = new NpgsqlConnection(connectionString)) {
                await connection.OpenAsync();
                var command = new NpgsqlCommand(commandText, connection);
                await using (var reader = await command.ExecuteReaderAsync()) {
                    await reader.ReadAsync();
                    (sellerId, isActive) = (reader.GetString(0), reader.GetBoolean(1));
                }
                
                if (!isActive && sellerId == clientId) {
                        command.CommandText = "COMMIT;";
                } else {
                    command.CommandText = "ROLLBACK;";
                }

                await command.ExecuteNonQueryAsync();
            }
            
            if (isActive) {
                throw new InvalidOperationException($"Impossible to {type.ToString().ToLower()} active auction.");
            }

            if (sellerId != clientId) {
                throw new InvalidOperationException($"Only owner of auction can {type.ToString().ToLower()} it.");
            }
        }
    }
}