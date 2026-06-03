using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaRegistros.Data;
using SistemaRegistros.Models;

namespace SistemaRegistros.Pages
{
    public class AdminPQRModel : PageModel
    {
        private readonly BaseDatos _db;

        public List<PQR> listaPQRs { get; set; } = new List<PQR>();

        public AdminPQRModel(BaseDatos db)
        {
            _db = db;
        }

        public void OnGet()
        {
            if (HttpContext.Session.GetString("UsuarioRol") != "Admin")
            {
                Response.Redirect("/Login");
                return;
            }
            listaPQRs = _db.PQRs.ToList();
        }

        public IActionResult OnPost(int id, string estado)
        {
            var pqr = _db.PQRs.FirstOrDefault(p => p.Id == id);
            if (pqr != null)
            {
                pqr.Estado = estado;
                _db.SaveChanges();
            }
            return RedirectToPage("/AdminPQR");
        }

        public IActionResult OnGetEliminar(int id)
        {
            var pqr = _db.PQRs.FirstOrDefault(p => p.Id == id);
            if (pqr != null)
            {
                _db.PQRs.Remove(pqr);
                _db.SaveChanges();
            }
            return RedirectToPage("/AdminPQR");
        }
    }
}