using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationServer.Models {
    [Table("users")]
    public class User {
        public User() {}
        
        public User(string email, string name, string surname) {
            (Email, Name, Surname) = (email, name, surname);
        }
        
        [Key]
        [Column("email")]
        public string Email { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("surname")]
        public string Surname { get; set; }
        
        public override string ToString() {
            return $"{Name} {Surname} {Email}";
        }
    }
}