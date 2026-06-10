using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaRegistros.Data;
using SistemaRegistros.Models;
using SistemaRegistros.Services;

namespace SistemaRegistros.Pages
{
    public class FormularioPQRModel : PageModel
    {
        private readonly BaseDatos _db;
        private readonly CorreoServicio _correoServicio;

        public bool Enviado { get; set; } = false;

        [BindProperty]
        public PQR PQR { get; set; } = new PQR();

        public FormularioPQRModel(BaseDatos db, CorreoServicio correoServicio)
        {
            _db = db;
            _correoServicio = correoServicio;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            PQR.FechaEnvio = DateTime.Now;
            PQR.Estado = "Pendiente";
            _db.PQRs.Add(PQR);
            _db.SaveChanges();

            // Avisar a los administradores que llego una PQR nueva
            var admins = _db.Usuarios.Where(u => u.Rol == "Admin").ToList();

            var cuerpo = $@"
                <div style='font-family:Arial, sans-serif; max-width:600px; margin:0 auto;'>
                    <div style='background:#1a1a2e; padding:25px; text-align:center;'>
                        <h1 style='color:#e94560; margin:0;'>Nueva PQR recibida</h1>
                    </div>
                    <div style='padding:30px; background:#f4f6f9;'>
                        <p style='font-size:16px; color:#333;'>Se ha registrado una nueva <strong>{PQR.Tipo}</strong>.</p>
                        <p style='font-size:14px; color:#555;'><strong>De:</strong> {PQR.Nombre} ({PQR.Correo})</p>
                        <p style='font-size:14px; color:#555;'><strong>Mensaje:</strong></p>
                        <p style='font-size:14px; color:#333; background:white; padding:15px; border-radius:8px;'>{PQR.Mensaje}</p>
                        <p style='font-size:13px; color:#777;'>Ingresa al panel de Admin PQRS para gestionarla.</p>
                    </div>
                    <div style='background:#1a1a2e; padding:15px; text-align:center;'>
                        <p style='color:#999; font-size:12px; margin:0;'>Sistema PRUEBA - Notificación automática</p>
                    </div>
                </div>";

            foreach (var admin in admins)
            {
                _correoServicio.Enviar(admin.Correo, $"Nueva {PQR.Tipo} recibida - PRUEBA", cuerpo);
            }

            Enviado = true;
            return Page();
        }
    }
}