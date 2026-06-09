using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaRegistros.Data;

namespace SistemaRegistros.Pages
{
    public class LoginModel : PageModel
    {
        private readonly BaseDatos _db;

        public string ErrorMessage { get; set; } = string.Empty;

        [BindProperty]
        public string Correo { get; set; } = string.Empty;

        [BindProperty]
        public string Contrasena { get; set; } = string.Empty;

        public LoginModel(BaseDatos db)
        {
            _db = db;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var usuario = _db.Usuarios.FirstOrDefault(u => u.Correo == Correo && u.Contrasena == Contrasena);

            if (usuario == null)
            {
                ErrorMessage = "Correo o contraseña incorrectos.";
                return Page();
            }

            HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
            HttpContext.Session.SetString("UsuarioApellido", usuario.Apellido);
            HttpContext.Session.SetString("UsuarioCorreo", usuario.Correo);
            HttpContext.Session.SetString("UsuarioRol", usuario.Rol);
            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);

            return RedirectToPage("/ProductosDisponibles");
        }
    }
}