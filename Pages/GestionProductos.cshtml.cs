using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaRegistros.Models;
using SistemaRegistros.Services;

namespace SistemaRegistros.Pages
{
    public class GestionProductosModel : PageModel
    {
        private readonly ProductoGestor _productoGestor;

        public List<Producto> listaProductos { get; set; } = new List<Producto>();

        public GestionProductosModel(ProductoGestor productoGestor)
        {
            _productoGestor = productoGestor;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("UsuarioRol") != "Admin")
            {
                return RedirectToPage("/Login");
            }
            listaProductos = _productoGestor.ObtenerTodos();
            return Page();
        }

        public IActionResult OnPostActualizar(int id, int stock, decimal precio)
        {
            if (HttpContext.Session.GetString("UsuarioRol") != "Admin")
            {
                return RedirectToPage("/Login");
            }

            var producto = _productoGestor.ObtenerPorId(id);
            if (producto != null)
            {
                producto.Stock = stock < 0 ? 0 : stock;
                producto.Precio = precio < 0 ? 0 : precio;
                _productoGestor.Actualizar(producto);
            }
            return RedirectToPage("/GestionProductos");
        }

        public IActionResult OnPostEliminar(int id)
        {
            if (HttpContext.Session.GetString("UsuarioRol") != "Admin")
            {
                return RedirectToPage("/Login");
            }
            _productoGestor.Eliminar(id);
            return RedirectToPage("/GestionProductos");
        }
    }
}