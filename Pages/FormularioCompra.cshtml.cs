using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaRegistros.Models;
using SistemaRegistros.Services;

namespace SistemaRegistros.Pages
{
    public class FormularioCompraModel : PageModel
    {
        private readonly ProductoGestor _productoGestor;
        private readonly CompraGestor _compraGestor;

        public Producto? ProductoSeleccionado { get; set; }

        [BindProperty]
        public Compra Compra { get; set; } = new Compra();

        public FormularioCompraModel(ProductoGestor productoGestor, CompraGestor compraGestor)
        {
            _productoGestor = productoGestor;
            _compraGestor = compraGestor;
        }

        public void OnGet(int productoId)
        {
            ProductoSeleccionado = _productoGestor.ObtenerPorId(productoId);
        }

        public IActionResult OnPost()
        {
            var producto = _productoGestor.ObtenerPorId(Compra.ProductoId ?? 0);
            if (producto != null)
            {
                Compra.Total = producto.Precio * Compra.Cantidad;
                Compra.Producto = null;
            }
            _compraGestor.Agregar(Compra);
            return RedirectToPage("/HistorialCompras");
        }
    }
}