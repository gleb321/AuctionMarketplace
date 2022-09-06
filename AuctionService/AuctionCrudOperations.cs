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
            using (var connection = new NpgsqlConnection(connectionString)) {
                await connection.OpenAsync();
                var command = new NpgsqlCommand(commandText, connection);
                await using (var reader = await command.ExecuteReaderAsync()) {
                    await reader.ReadAsync();
                    var response = await getResponse(reader.GetInt32(0));
                    command.CommandText = response.StatusCode == HttpStatusCode.OK ? "COMMIT;" : "ROLLBACK;";
                    await command.ExecuteNonQueryAsync();
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
        }
        
        public static async Task ChangeAuction(string connectionString, ChangeType type, string commandText, string clientId) {
            using (var connection = new NpgsqlConnection(connectionString)) {
                await connection.OpenAsync();
                var command = new NpgsqlCommand(commandText, connection);
                await using (var reader = await command.ExecuteReaderAsync()) {
                    await reader.ReadAsync();
                    (string sellerId, bool isActive) = (reader.GetString(0), reader.GetBoolean(1));

                    if (!isActive && sellerId == clientId) {
                        command.CommandText = "COMMIT;";
                    } else {
                        command.CommandText = "ROLLBACK;";
                    }

                    await command.ExecuteNonQueryAsync();
                    
                    if (isActive) {
                        throw new InvalidOperationException($"Impossible to {type.ToString().ToLower()} active auction.");
                    }

                    if (sellerId != clientId) {
                        throw new InvalidOperationException($"Only owner of auction can {type.ToString().ToLower()} it.");
                    }
                }
            }
        }
    }
}