using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaRegistros.Data;
using SistemaRegistros.Models;
using SistemaRegistros.Services;

namespace SistemaRegistros.Pages
{
    public class AdminPQRModel : PageModel
    {
        private readonly BaseDatos _db;
        private readonly CorreoServicio _correoServicio;

        public List<PQR> listaPQRs { get; set; } = new List<PQR>();

        public AdminPQRModel(BaseDatos db, CorreoServicio correoServicio)
        {
            _db = db;
            _correoServicio = correoServicio;
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

                // Enviar correo al cliente avisando el cambio de estado
                var cuerpo = $@"
                    <div style='font-family:Arial, sans-serif; max-width:600px; margin:0 auto;'>
                        <div style='background:#1a1a2e; padding:25px; text-align:center;'>
                            <h1 style='color:#e94560; margin:0;'>Actualización de tu PQR</h1>
                        </div>
                        <div style='padding:30px; background:#f4f6f9;'>
                            <p style='font-size:16px; color:#333;'>Hola <strong>{pqr.Nombre}</strong>,</p>
                            <p style='font-size:15px; color:#555;'>El estado de tu {pqr.Tipo} ha sido actualizado a:</p>
                            <p style='font-size:20px; font-weight:bold; color:#e94560; text-align:center; padding:15px; background:white; border-radius:8px;'>{estado}</p>
                            <p style='font-size:14px; color:#777; margin-top:20px;'><strong>Tu mensaje original:</strong><br>{pqr.Mensaje}</p>
                        </div>
                        <div style='background:#1a1a2e; padding:15px; text-align:center;'>
                            <p style='color:#999; font-size:12px; margin:0;'>Sistema PRUEBA - Notificación automática</p>
                        </div>
                    </div>";

                _correoServicio.Enviar(pqr.Correo, $"Tu {pqr.Tipo} fue actualizada - PRUEBA", cuerpo);
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