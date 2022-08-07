using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace AuthenticationServer.Models {
    public class RegistrationModel {
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        
        [JsonPropertyName("surname")]
        public string? Surname { get; set; }
        
        [JsonPropertyName("password")]
        public string? Password { get; set; }
    }
}