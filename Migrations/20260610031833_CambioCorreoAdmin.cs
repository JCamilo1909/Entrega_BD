using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaRegistros.Migrations
{
    /// <inheritdoc />
    public partial class CambioCorreoAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Correo",
                value: "pruebascorreo1909@gmail.com");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Correo",
                value: "admin@prueba.com");
        }
    }
}
