using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlitterBackend.Models
{
    [Table("Users")]
    public class User
    {
        [Column("Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [Column("Username")]
        [Required]
        public string Username { get; set; }
        [Column("Email")]
        [Required]
        public string Email { get; set; }
        [Column("Password")]
        [Required]
        public string Password { get; set; }
        [Column("PhotoFileName")]
        public string PhotoFileName { get; set; }

        public virtual List<Post> Posts { get; set; }
    }
}
