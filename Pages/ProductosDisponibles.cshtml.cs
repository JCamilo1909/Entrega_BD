using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaRegistros.Models;
using SistemaRegistros.Services;

namespace SistemaRegistros.Pages
{
    public class ProductosDisponiblesModel : PageModel
    {
        private readonly ProductoGestor _productoGestor;

        public List<Producto> listaProductos { get; set; } = new List<Producto>();

        public ProductosDisponiblesModel(ProductoGestor productoGestor)
        {
            _productoGestor = productoGestor;
        }
        public void OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioRol")))
            {
                Response.Redirect("/Login");
                return;
            }
            listaProductos = _productoGestor.ObtenerTodos();
        }
    }
}