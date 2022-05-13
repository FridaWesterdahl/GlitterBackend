using Microsoft.EntityFrameworkCore;

namespace GlitterBackend.Models
{
    public class EFContext : DbContext
    {
        public const string connectionString = "Data Source=LAPTOP-MOP66LEC\\SQLEXPRESS;Initial Catalog=GlitterDatabase;Integrated Security=True";

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //        optionsBuilder.UseSqlServer(connectionString);
        //}
        public EFContext(DbContextOptions<EFContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }

    }
}

