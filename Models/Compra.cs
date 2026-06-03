namespace SistemaRegistros.Models
{
    public class Compra
    {
        public int Id { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public string ApellidoCliente { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public int? ProductoId { get; set; }
        public Producto? Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaCompra { get; set; } = DateTime.Now;
    }
}