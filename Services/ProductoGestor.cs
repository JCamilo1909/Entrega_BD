using SistemaRegistros.Data;
using SistemaRegistros.Models;

namespace SistemaRegistros.Services
{
    public class ProductoGestor
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ProductoGestor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public List<Producto> ObtenerTodos()
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BaseDatos>();
            return db.Productos.ToList();
        }

        public Producto? ObtenerPorId(int id)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BaseDatos>();
            return db.Productos.FirstOrDefault(p => p.Id == id);
        }

        public void Agregar(Producto producto)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BaseDatos>();
            producto.Id = 0;
            db.Productos.Add(producto);
            db.SaveChanges();
        }

        public void Eliminar(int id)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BaseDatos>();
            var producto = db.Productos.FirstOrDefault(p => p.Id == id);
            if (producto != null)
            {
                db.Productos.Remove(producto);
                db.SaveChanges();
            }
        }

        public void ActualizarStock(int id, int nuevoStock)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BaseDatos>();
            var producto = db.Productos.FirstOrDefault(p => p.Id == id);
            if (producto != null)
            {
                producto.Stock = nuevoStock;
                db.SaveChanges();
            }
        }

        public void Actualizar(Producto producto)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BaseDatos>();
            var existente = db.Productos.FirstOrDefault(p => p.Id == producto.Id);
            if (existente != null)
            {
                existente.Nombre = producto.Nombre;
                existente.Descripcion = producto.Descripcion;
                existente.Precio = producto.Precio;
                existente.Stock = producto.Stock;
                existente.Categoria = producto.Categoria;
                db.SaveChanges();
            }
        }

        public void DescontarStock(int id, int cantidad)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BaseDatos>();
            var producto = db.Productos.FirstOrDefault(p => p.Id == id);
            if (producto != null)
            {
                producto.Stock -= cantidad;
                if (producto.Stock < 0) producto.Stock = 0;
                db.SaveChanges();
            }
        }
    }
}