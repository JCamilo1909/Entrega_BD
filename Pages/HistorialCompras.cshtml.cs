using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaRegistros.Data;
using SistemaRegistros.Models;
using SistemaRegistros.Services;

namespace SistemaRegistros.Pages
{
    public class HistorialComprasModel : PageModel
    {
        private readonly CompraGestor _compraGestor;
        private readonly ProductoGestor _productoGestor;
        private readonly BaseDatos _db;

        public List<Compra> listaCompras { get; set; } = new List<Compra>();

        public HistorialComprasModel(CompraGestor compraGestor, ProductoGestor productoGestor, BaseDatos db)
        {
            _compraGestor = compraGestor;
            _productoGestor = productoGestor;
            _db = db;
        }

        public void OnGet()
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (rol == "Admin")
            {
                listaCompras = _compraGestor.ObtenerTodos();
            }
            else
            {
                var todasLasCompras = _compraGestor.ObtenerTodos();
                var usuario = _db.Usuarios.FirstOrDefault(u => u.Id == usuarioId);
                if (usuario != null)
                {
                    listaCompras = todasLasCompras.Where(c =>
                        c.Correo.ToLower() == usuario.Correo.ToLower()).ToList();
                }
            }
        }

        public IActionResult OnGetEliminar(int id)
        {
            _compraGestor.Eliminar(id);
            return RedirectToPage("/HistorialCompras");
        }

        public async Task<IActionResult> OnPostAsync(IFormFile archivoExcel)
        {
            if (archivoExcel != null && archivoExcel.Length > 0)
            {
                var comprasExistentes = _compraGestor.ObtenerTodos();
                foreach (var compra in comprasExistentes)
                {
                    _compraGestor.Eliminar(compra.Id);
                }

                using var stream = new MemoryStream();
                await archivoExcel.CopyToAsync(stream);
                stream.Position = 0;

                using var workbook = new XLWorkbook(stream);
                var hoja = workbook.Worksheet(1);
                var filas = hoja.RangeUsed().RowsUsed().Skip(2);

                foreach (var fila in filas)
                {
                    var nombreProducto = fila.Cell(7).GetString();
                    var productoEncontrado = _productoGestor.ObtenerTodos()
                        .FirstOrDefault(p => p.Nombre == nombreProducto);

                    var compra = new Compra
                    {
                        Id = int.TryParse(fila.Cell(1).GetString(), out int idOriginal) ? idOriginal : 0,
                        NombreCliente = fila.Cell(2).GetString(),
                        ApellidoCliente = fila.Cell(3).GetString(),
                        Correo = fila.Cell(4).GetString(),
                        Telefono = fila.Cell(5).GetString(),
                        Direccion = fila.Cell(6).GetString(),
                        ProductoId = productoEncontrado?.Id,
                        Producto = null,
                        Cantidad = int.TryParse(fila.Cell(8).GetString(), out int cantidad) ? cantidad : 1,
                        Total = decimal.TryParse(fila.Cell(9).GetString(), out decimal total) ? total : 0,
                        FechaCompra = DateTime.Now
                    };
                    _compraGestor.Agregar(compra, conservarId: true);
                }
            }
            return RedirectToPage("/HistorialCompras");
        }

        public IActionResult OnGetExportarExcel()
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            List<Compra> compras;

            if (rol == "Admin")
            {
                compras = _compraGestor.ObtenerTodos();
            }
            else
            {
                var todasLasCompras = _compraGestor.ObtenerTodos();
                var usuario = _db.Usuarios.FirstOrDefault(u => u.Id == usuarioId);
                compras = usuario != null
                    ? todasLasCompras.Where(c => c.Correo.ToLower() == usuario.Correo.ToLower()).ToList()
                    : new List<Compra>();
            }

            using var workbook = new XLWorkbook();
            var hoja = workbook.Worksheets.Add("Compras");

            hoja.Cell(1, 1).Value = "HISTORIAL DE COMPRAS - PRUEBA";
            hoja.Range(1, 1, 1, 10).Merge();
            hoja.Cell(1, 1).Style.Font.Bold = true;
            hoja.Cell(1, 1).Style.Font.FontSize = 16;
            hoja.Cell(1, 1).Style.Font.FontColor = XLColor.White;
            hoja.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#1a1a2e");
            hoja.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            hoja.Row(1).Height = 30;

            string[] encabezados = { "ID", "Nombre", "Apellido", "Correo", "Teléfono", "Dirección", "Producto", "Cantidad", "Total", "Fecha" };
            for (int i = 0; i < encabezados.Length; i++)
            {
                var celda = hoja.Cell(2, i + 1);
                celda.Value = encabezados[i];
                celda.Style.Font.Bold = true;
                celda.Style.Font.FontColor = XLColor.White;
                celda.Style.Fill.BackgroundColor = XLColor.FromHtml("#2e4057");
                celda.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                celda.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                celda.Style.Border.OutsideBorderColor = XLColor.White;
            }
            hoja.Row(2).Height = 20;

            for (int i = 0; i < compras.Count; i++)
            {
                var c = compras[i];
                var fila = i + 3;

                hoja.Cell(fila, 1).Value = c.Id;
                hoja.Cell(fila, 2).Value = c.NombreCliente;
                hoja.Cell(fila, 3).Value = c.ApellidoCliente;
                hoja.Cell(fila, 4).Value = c.Correo;
                hoja.Cell(fila, 5).Value = c.Telefono;
                hoja.Cell(fila, 6).Value = c.Direccion;
                hoja.Cell(fila, 7).Value = c.Producto?.Nombre ?? "N/A";
                hoja.Cell(fila, 8).Value = c.Cantidad;
                hoja.Cell(fila, 9).Value = c.Total;
                hoja.Cell(fila, 10).Value = c.FechaCompra.ToString("dd/MM/yyyy HH:mm");

                var colorFila = i % 2 == 0 ? XLColor.White : XLColor.FromHtml("#eef2f7");
                hoja.Range(fila, 1, fila, 10).Style.Fill.BackgroundColor = colorFila;
                hoja.Range(fila, 1, fila, 10).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                hoja.Range(fila, 1, fila, 10).Style.Border.OutsideBorderColor = XLColor.FromHtml("#cccccc");
                hoja.Row(fila).Height = 18;
            }

            hoja.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var contenido = stream.ToArray();

            return File(contenido, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Compras.xlsx");
        }
    }
}