namespace SistemaRegistros.Models
{
    public class PQR
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty; // Peticion, Queja, Reclamo
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public string Estado { get; set; } = "Pendiente"; // Pendiente, En proceso, Resuelto
        public DateTime FechaEnvio { get; set; } = DateTime.Now;
    }
}