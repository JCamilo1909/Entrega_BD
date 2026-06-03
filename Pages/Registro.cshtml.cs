using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaRegistros.Data;
using SistemaRegistros.Models;

namespace SistemaRegistros.Pages
{
    public class RegistroModel : PageModel
    {
        private readonly BaseDatos _db;

        public string ErrorMessage { get; set; } = string.Empty;

        [BindProperty]
        public string Nombre { get; set; } = string.Empty;

        [BindProperty]
        public string Correo { get; set; } = string.Empty;

        [BindProperty]
        public string Contrasena { get; set; } = string.Empty;

        [BindProperty]
        public string ConfirmarContrasena { get; set; } = string.Empty;

        public RegistroModel(BaseDatos db)
        {
            _db = db;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (Contrasena != ConfirmarContrasena)
            {
                ErrorMessage = "Las contraseñas no coinciden.";
                return Page();
            }

            var usuarioExistente = _db.Usuarios.FirstOrDefault(u => u.Correo == Correo);
            if (usuarioExistente != null)
            {
                ErrorMessage = "Ya existe una cuenta con ese correo.";
                return Page();
            }

            var nuevoUsuario = new Usuario
            {
                Nombre = Nombre,
                Correo = Correo,
                Contrasena = Contrasena,
                Rol = "Cliente"
            };

            _db.Usuarios.Add(nuevoUsuario);
            _db.SaveChanges();

            HttpContext.Session.SetString("UsuarioNombre", nuevoUsuario.Nombre);
            HttpContext.Session.SetString("UsuarioRol", nuevoUsuario.Rol);
            HttpContext.Session.SetInt32("UsuarioId", nuevoUsuario.Id);

            return RedirectToPage("/ProductosDisponibles");
        }
    }
}