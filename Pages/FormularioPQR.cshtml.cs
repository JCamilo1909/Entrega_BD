using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaRegistros.Data;
using SistemaRegistros.Models;

namespace SistemaRegistros.Pages
{
    public class FormularioPQRModel : PageModel
    {
        private readonly BaseDatos _db;

        public bool Enviado { get; set; } = false;

        [BindProperty]
        public PQR PQR { get; set; } = new PQR();

        public FormularioPQRModel(BaseDatos db)
        {
            _db = db;
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
            Enviado = true;
            return Page();
        }
    }
}