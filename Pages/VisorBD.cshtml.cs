using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaRegistros.Data;
using SistemaRegistros.Models;
using SistemaRegistros.Services;

namespace SistemaRegistros.Pages
{
    public class VisorBDModel : PageModel
    {
        private readonly BaseDatos _db;
        private readonly CompraGestor _compraGestor;

        public List<Usuario> listaUsuarios { get; set; } = new List<Usuario>();
        public List<Compra> listaCompras { get; set; } = new List<Compra>();
        public List<PQR> listaPQRs { get; set; } = new List<PQR>();

        public VisorBDModel(BaseDatos db, CompraGestor compraGestor)
        {
            _db = db;
            _compraGestor = compraGestor;
        }

        public void OnGet()
        {
            if (HttpContext.Session.GetString("UsuarioRol") != "Admin")
            {
                Response.Redirect("/Login");
                return;
            }

            listaUsuarios = _db.Usuarios.ToList();
            listaCompras = _compraGestor.ObtenerTodos();
            listaPQRs = _db.PQRs.ToList();
        }
    }
}