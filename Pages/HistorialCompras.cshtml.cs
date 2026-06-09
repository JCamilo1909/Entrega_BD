using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaRegistros.Data;
using SistemaRegistros.Models;
using SistemaRegistros.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Text;
using System.Text.Json;

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

        // Metodo auxiliar: filtra las compras segun el rol del usuario
        private List<Compra> ObtenerComprasSegunRol()
        {
            var rol = HttpContext.Session.GetString("UsuarioRol");
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (rol == "Admin")
            {
                return _compraGestor.ObtenerTodos();
            }
            else
            {
                var todasLasCompras = _compraGestor.ObtenerTodos();
                var usuario = _db.Usuarios.FirstOrDefault(u => u.Id == usuarioId);
                return usuario != null
                    ? todasLasCompras.Where(c => c.Correo.ToLower() == usuario.Correo.ToLower()).ToList()
                    : new List<Compra>();
            }
        }

        public void OnGet()
        {
            listaCompras = ObtenerComprasSegunRol();
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

        // ===== EXPORTAR EXCEL =====
        public IActionResult OnGetExportarExcel()
        {
            var compras = ObtenerComprasSegunRol();

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
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Compras.xlsx");
        }

        // ===== EXPORTAR CSV =====
        public IActionResult OnGetExportarCsv()
        {
            var compras = ObtenerComprasSegunRol();
            var sb = new StringBuilder();
            sb.AppendLine("ID,Nombre,Apellido,Correo,Telefono,Direccion,Producto,Cantidad,Total,Fecha");

            foreach (var c in compras)
            {
                var producto = (c.Producto?.Nombre ?? "N/A").Replace(",", " ");
                sb.AppendLine($"{c.Id},{c.NombreCliente},{c.ApellidoCliente},{c.Correo},{c.Telefono},{c.Direccion?.Replace(",", " ")},{producto},{c.Cantidad},{c.Total},{c.FechaCompra:dd/MM/yyyy HH:mm}");
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/csv", "Compras.csv");
        }

        // ===== EXPORTAR TXT =====
        public IActionResult OnGetExportarTxt()
        {
            var compras = ObtenerComprasSegunRol();
            var sb = new StringBuilder();
            sb.AppendLine("HISTORIAL DE COMPRAS - PRUEBA");
            sb.AppendLine("=========================================");
            sb.AppendLine();

            foreach (var c in compras)
            {
                sb.AppendLine($"ID: {c.Id}");
                sb.AppendLine($"Cliente: {c.NombreCliente} {c.ApellidoCliente}");
                sb.AppendLine($"Correo: {c.Correo}");
                sb.AppendLine($"Telefono: {c.Telefono}");
                sb.AppendLine($"Direccion: {c.Direccion}");
                sb.AppendLine($"Producto: {c.Producto?.Nombre ?? "N/A"}");
                sb.AppendLine($"Cantidad: {c.Cantidad}");
                sb.AppendLine($"Total: ${c.Total:N0}");
                sb.AppendLine($"Fecha: {c.FechaCompra:dd/MM/yyyy HH:mm}");
                sb.AppendLine("-----------------------------------------");
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/plain", "Compras.txt");
        }

        // ===== EXPORTAR JSON =====
        public IActionResult OnGetExportarJson()
        {
            var compras = ObtenerComprasSegunRol();
            var datos = compras.Select(c => new
            {
                c.Id,
                c.NombreCliente,
                c.ApellidoCliente,
                c.Correo,
                c.Telefono,
                c.Direccion,
                Producto = c.Producto?.Nombre ?? "N/A",
                c.Cantidad,
                c.Total,
                Fecha = c.FechaCompra.ToString("dd/MM/yyyy HH:mm")
            });

            var opciones = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(datos, opciones);
            var bytes = Encoding.UTF8.GetBytes(json);
            return File(bytes, "application/json", "Compras.json");
        }

        // ===== EXPORTAR PDF =====
        public IActionResult OnGetExportarPdf()
        {
            var compras = ObtenerComprasSegunRol();
            QuestPDF.Settings.License = LicenseType.Community;

            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(30);
                    page.Header().Text("Historial de Compras - PRUEBA")
                        .FontSize(18).Bold().FontColor(Colors.Blue.Darken3);

                    page.Content().PaddingTop(15).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(30);
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.ConstantColumn(50);
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            void Celda(string texto) => header.Cell().Background(Colors.Blue.Darken3).Padding(5)
                                .Text(texto).FontColor(Colors.White).Bold().FontSize(9);
                            Celda("ID");
                            Celda("Cliente");
                            Celda("Correo");
                            Celda("Producto");
                            Celda("Total");
                            Celda("Cant");
                            Celda("Fecha");
                        });

                        foreach (var c in compras)
                        {
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(c.Id.ToString()).FontSize(8);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"{c.NombreCliente} {c.ApellidoCliente}").FontSize(8);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(c.Correo).FontSize(8);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(c.Producto?.Nombre ?? "N/A").FontSize(8);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"${c.Total:N0}").FontSize(8);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(c.Cantidad.ToString()).FontSize(8);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(c.FechaCompra.ToString("dd/MM/yyyy")).FontSize(8);
                        }
                    });
                });
            });

            var pdfBytes = documento.GeneratePdf();
            return File(pdfBytes, "application/pdf", "Compras.pdf");
        }
    }
}