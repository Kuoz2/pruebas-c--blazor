using Microsoft.EntityFrameworkCore;
using puntodeventa.Models;

namespace puntodeventa.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) { }
        public DbSet<Productos> polls_productos { get; set; }
        public DbSet<Ventas> polls_ventas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Ventas>()
                .HasOne(v=> v.Productos)
                .WithMany(p=> p.Ventas)
                .HasForeignKey(v=> v.Productoid);
        }
    }
}
