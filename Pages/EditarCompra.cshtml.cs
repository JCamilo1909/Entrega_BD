using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaRegistros.Data;
using SistemaRegistros.Models;
using SistemaRegistros.Services;
using Microsoft.EntityFrameworkCore;

namespace SistemaRegistros.Pages
{
    public class EditarCompraModel : PageModel
    {
        private readonly BaseDatos _db;
        private readonly ProductoGestor _productoGestor;

        [BindProperty]
        public Compra Compra { get; set; } = new Compra();

        public EditarCompraModel(BaseDatos db, ProductoGestor productoGestor)
        {
            _db = db;
            _productoGestor = productoGestor;
        }

        public void OnGet(int id)
        {
            Compra = _db.Compras.FirstOrDefault(c => c.Id == id) ?? new Compra();
        }

        public IActionResult OnPost()
        {
            var compraExistente = _db.Compras.FirstOrDefault(c => c.Id == Compra.Id);
            if (compraExistente != null)
            {
                var producto = _productoGestor.ObtenerPorId(compraExistente.ProductoId ?? 0);

                compraExistente.NombreCliente = Compra.NombreCliente;
                compraExistente.ApellidoCliente = Compra.ApellidoCliente;
                compraExistente.Correo = Compra.Correo;
                compraExistente.Telefono = Compra.Telefono;
                compraExistente.Direccion = Compra.Direccion;
                compraExistente.Cantidad = Compra.Cantidad;

                if (producto != null)
                    compraExistente.Total = producto.Precio * Compra.Cantidad;

                _db.SaveChanges();
            }
            return RedirectToPage("/HistorialCompras");
        }
    }
}