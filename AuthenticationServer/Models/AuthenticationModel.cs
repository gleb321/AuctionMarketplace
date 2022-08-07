using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace AuthenticationServer.Models {
    public class AuthenticationModel {
        [JsonPropertyName("login")]
        public string? Login { get; set; }
        
        [JsonPropertyName("password")]
        public string? Password { get; set; }
    }
}