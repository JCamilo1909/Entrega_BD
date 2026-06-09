using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemaRegistros.Migrations
{
    /// <inheritdoc />
    public partial class ProductosEnBD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "Id", "Categoria", "Descripcion", "Nombre", "Precio", "Stock" },
                values: new object[,]
                {
                    { 1001, "Televisores", "Smart TV 42 pulgadas Full HD", "Televisor Samsung", 3000000m, 10 },
                    { 1002, "Computadores", "Intel Core i5, 8GB RAM, 256GB SSD", "Laptop Lenovo", 2500000m, 5 },
                    { 1003, "Celulares", "128GB, Color Negro", "iPhone 14", 4000000m, 8 },
                    { 1004, "Audio", "Inalámbricos con cancelación de ruido", "Audifonos Sony", 300000m, 15 },
                    { 1005, "Tablets", "iPad 10ma generación 64GB", "Tablet", 900000m, 7 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Productos");
        }
    }
}
