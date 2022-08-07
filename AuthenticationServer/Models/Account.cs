using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationServer.Models {
    [Table("accounts")]
    public class Account {
        public Account() {}
        
        public Account(string login, string password, Role role) {
            (Login, Password, UserRole) = (login, password, role);
        }
        public enum Role {
            User,
            Admin
        }

        [Key] 
        [Column("login")]
        [Required]
        public string? Login { get; set; }

        [Column("password")]
        [Required]
        public string? Password { get; set; }

        [Column("role")]
        [Required]
        public Role UserRole { get; set; }
        
        public override string ToString() {
            return $"{Login} {Password} {UserRole}";
        }
    }
}