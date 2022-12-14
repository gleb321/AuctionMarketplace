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
        public string Login { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("role")]
        public Role UserRole { get; set; }
        
        public override string ToString() {
            return $"{Login} {Password} {UserRole}";
        }
    }
}