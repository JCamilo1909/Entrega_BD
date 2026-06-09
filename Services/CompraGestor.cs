using SistemaRegistros.Data;
using SistemaRegistros.Models;

namespace SistemaRegistros.Services
{
    public class CompraGestor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ProductoGestor _productoGestor;

        public CompraGestor(IServiceScopeFactory scopeFactory, ProductoGestor productoGestor)
        {
            _scopeFactory = scopeFactory;
            _productoGestor = productoGestor;
        }

        public List<Compra> ObtenerTodos()
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BaseDatos>();
            var compras = db.Compras.ToList();

            foreach (var compra in compras)
            {
                if (compra.ProductoId.HasValue)
                    compra.Producto = _productoGestor.ObtenerPorId(compra.ProductoId.Value);
            }

            return compras;
        }

        public void Agregar(Compra compra, bool conservarId = false)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BaseDatos>();
            compra.FechaCompra = compra.FechaCompra == default ? DateTime.Now : compra.FechaCompra;
            compra.Producto = null;
            compra.Id = 0;
            db.Compras.Add(compra);
            db.SaveChanges();
        }

        public void Eliminar(int id)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BaseDatos>();
            var compra = db.Compras.FirstOrDefault(c => c.Id == id);
            if (compra != null)
            {
                db.Compras.Remove(compra);
                db.SaveChanges();
            }
        }
    }
}