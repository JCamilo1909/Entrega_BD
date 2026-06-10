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
        public DbSet<Producto> Productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Compra>()
                .Ignore(c => c.Producto);

            // Admin por defecto
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { Id = 1, Nombre = "Administrador", Correo = "pruebascorreo1909@gmail.com", Contrasena = "admin123", Rol = "Admin" },
                new Usuario { Id = 2, Nombre = "Supervisor", Correo = "supervisor@prueba.com", Contrasena = "super123", Rol = "Admin" }
            );

            // Productos iniciales
            modelBuilder.Entity<Producto>().HasData(
                new Producto { Id = 1001, Nombre = "Televisor Samsung", Descripcion = "Smart TV 42 pulgadas Full HD", Precio = 3000000, Stock = 10, Categoria = "Televisores" },
                new Producto { Id = 1002, Nombre = "Laptop Lenovo", Descripcion = "Intel Core i5, 8GB RAM, 256GB SSD", Precio = 2500000, Stock = 5, Categoria = "Computadores" },
                new Producto { Id = 1003, Nombre = "iPhone 14", Descripcion = "128GB, Color Negro", Precio = 4000000, Stock = 8, Categoria = "Celulares" },
                new Producto { Id = 1004, Nombre = "Audifonos Sony", Descripcion = "Inalámbricos con cancelación de ruido", Precio = 300000, Stock = 15, Categoria = "Audio" },
                new Producto { Id = 1005, Nombre = "Tablet", Descripcion = "iPad 10ma generación 64GB", Precio = 900000, Stock = 7, Categoria = "Tablets" }
            );
        }
    }
}