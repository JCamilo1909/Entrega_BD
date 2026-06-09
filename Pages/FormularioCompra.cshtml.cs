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
        public string ErrorMessage { get; set; } = string.Empty;

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

            if (producto == null)
            {
                ErrorMessage = "El producto no existe.";
                return Page();
            }

            // Validar que la cantidad sea valida
            if (Compra.Cantidad < 1)
            {
                ProductoSeleccionado = producto;
                ErrorMessage = "La cantidad debe ser al menos 1.";
                return Page();
            }

            // Validar que no se compre mas del stock disponible
            if (Compra.Cantidad > producto.Stock)
            {
                ProductoSeleccionado = producto;
                ErrorMessage = $"Solo hay {producto.Stock} unidades disponibles. No puedes comprar {Compra.Cantidad}.";
                return Page();
            }

            // Calcular total y guardar la compra
            Compra.Total = producto.Precio * Compra.Cantidad;
            Compra.Producto = null;
            _compraGestor.Agregar(Compra);

            // Descontar el stock
            _productoGestor.DescontarStock(producto.Id, Compra.Cantidad);

            return RedirectToPage("/HistorialCompras");
        }
    }
}