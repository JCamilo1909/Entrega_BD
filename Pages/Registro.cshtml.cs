using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaRegistros.Data;
using SistemaRegistros.Models;
using System.Text.RegularExpressions;
using DnsClient;

namespace SistemaRegistros.Pages
{
    public class RegistroModel : PageModel
    {
        private readonly BaseDatos _db;

        public string ErrorMessage { get; set; } = string.Empty;

        [BindProperty]
        public string Nombre { get; set; } = string.Empty;

        [BindProperty]
        public string Apellido { get; set; } = string.Empty;

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
            // Validar que las contraseñas coincidan
            if (Contrasena != ConfirmarContrasena)
            {
                ErrorMessage = "Las contraseñas no coinciden.";
                return Page();
            }
            // Validar el formato del correo
            var errorCorreo = ValidarCorreo(Correo);
            if (!string.IsNullOrEmpty(errorCorreo))
            {
                ErrorMessage = errorCorreo;
                return Page();
            }

            // Validar la fortaleza de la contraseña
            var errorContrasena = ValidarContrasena(Contrasena);
            if (!string.IsNullOrEmpty(errorContrasena))
            {
                ErrorMessage = errorContrasena;
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
                Apellido = Apellido,
                Correo = Correo,
                Contrasena = Contrasena,
                Rol = "Cliente"
            };

            _db.Usuarios.Add(nuevoUsuario);
            _db.SaveChanges();

            HttpContext.Session.SetString("UsuarioNombre", nuevoUsuario.Nombre);
            HttpContext.Session.SetString("UsuarioApellido", nuevoUsuario.Apellido);
            HttpContext.Session.SetString("UsuarioCorreo", nuevoUsuario.Correo);
            HttpContext.Session.SetString("UsuarioRol", nuevoUsuario.Rol);
            HttpContext.Session.SetInt32("UsuarioId", nuevoUsuario.Id);

            return RedirectToPage("/ProductosDisponibles");
        }

        private string ValidarContrasena(string contrasena)
        {
            if (contrasena.Length < 8)
                return "La contraseña debe tener al menos 8 caracteres.";

            if (!Regex.IsMatch(contrasena, "[A-Z]"))
                return "La contraseña debe tener al menos una letra mayúscula.";

            if (!Regex.IsMatch(contrasena, "[a-z]"))
                return "La contraseña debe tener al menos una letra minúscula.";

            if (!Regex.IsMatch(contrasena, "[0-9]"))
                return "La contraseña debe tener al menos un número.";

            if (!Regex.IsMatch(contrasena, "[^a-zA-Z0-9]"))
                return "La contraseña debe tener al menos un símbolo (ej: !@#$%).";

            return string.Empty;
        }
        private string ValidarCorreo(string correo)
        {
            // Verifica que tenga formato nombre@dominio.extension
            var patron = @"^[^@\s]+@[^@\s]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(correo, patron))
                return "El correo no es válido. Debe tener un formato como nombre@dominio.com";

                 // 2. Verifica que el dominio exista y pueda recibir correos
            try
            {
                var dominio = correo.Split('@')[1];
                var lookup = new LookupClient();
                var resultado = lookup.Query(dominio, QueryType.MX);

                if (!resultado.Answers.MxRecords().Any())
                {
                    return "El dominio del correo no existe o no puede recibir correos.";
                }
            }
            catch
            {
                return "No se pudo verificar el dominio del correo. Intenta de nuevo.";
            }

            return string.Empty;
        }
    }
}