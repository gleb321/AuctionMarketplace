using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace AuthenticationServer.Models {
    public class AuthenticationModel {
        [Required]
        [EmailAddress]
        public string Login { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}