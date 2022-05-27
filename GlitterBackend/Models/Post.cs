using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GlitterBackend.Models;

namespace GlitterBackend
{
    [Table("Posts")]
    public class Post
    {
        [Column("Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [Column("Content")]
        [Required]
        public string Content { get; set; }
        [Column("Published")]
        [Required]
        public DateTime Published { get; set; }

        [Column("UserId")]
        public int UserId { get; set; }
        public virtual User? User { get; set; }
    }
}