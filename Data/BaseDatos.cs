using Microsoft.EntityFrameworkCore;
using SistemaRegistros.Models;

namespace SistemaRegistros.Data
{
    public class BaseDatos : DbContext
    {
        public BaseDatos(DbContextOptions<BaseDatos> options) : base(options)
        {
        }

        public DbSet<Compra> Compras { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<PQR> PQRs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Compra>()
                .Ignore(c => c.Producto);

            // Admin por defecto
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { Id = 1, Nombre = "Administrador", Correo = "admin@prueba.com", Contrasena = "admin123", Rol = "Admin" },
                new Usuario { Id = 2, Nombre = "Supervisor", Correo = "supervisor@prueba.com", Contrasena = "super123", Rol = "Admin" }
            );
        }
    }
}