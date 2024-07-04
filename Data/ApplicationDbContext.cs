using Microsoft.EntityFrameworkCore;
using puntodeventa.Models;

namespace puntodeventa.Data
{
    public class ApplicationDbContext : DbContext
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) { }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
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
