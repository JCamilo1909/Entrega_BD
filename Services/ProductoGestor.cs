using SistemaRegistros.Models;

namespace SistemaRegistros.Services
{
    public class ProductoGestor
    {
        private List<Producto> listaProductos = new List<Producto>()
        {
            new Producto { Id = 1001, Nombre = "Televisor Samsung", Descripcion = "Smart TV 42 pulgadas Full HD", Precio = 3000000, Stock = 10, Categoria = "Televisores" },
            new Producto { Id = 1002, Nombre = "Laptop Lenovo", Descripcion = "Intel Core i5, 8GB RAM, 256GB SSD", Precio = 2500000, Stock = 5, Categoria = "Computadores" },
            new Producto { Id = 1003, Nombre = "iPhone 14", Descripcion = "128GB, Color Negro", Precio = 4000000, Stock = 8, Categoria = "Celulares" },
            new Producto { Id = 1004, Nombre = "Audifonos Sony", Descripcion = "Inalámbricos con cancelación de ruido", Precio = 300000, Stock = 15, Categoria = "Audio" },
            new Producto { Id = 1005, Nombre = "Tablet", Descripcion = "iPad 10ma generación 64GB", Precio = 900000, Stock = 7, Categoria = "Tablets" }
        };

        public List<Producto> ObtenerTodos()
        {
            return listaProductos;
        }

        public Producto? ObtenerPorId(int id)
        {
            return listaProductos.FirstOrDefault(p => p.Id == id);
        }

        public void Agregar(Producto producto)
        {
            producto.Id = listaProductos.Count + 1;
            listaProductos.Add(producto);
        }

        public void Eliminar(int id)
        {
            var producto = ObtenerPorId(id);
            if (producto != null)
                listaProductos.Remove(producto);
        }
    }
}