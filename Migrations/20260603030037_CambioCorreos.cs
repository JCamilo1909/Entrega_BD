using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaRegistros.Migrations
{
    /// <inheritdoc />
    public partial class CambioCorreos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Correo",
                value: "admin@prueba.com");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "Correo",
                value: "supervisor@prueba.com");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Correo",
                value: "admin@techstore.com");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "Correo",
                value: "supervisor@techstore.com");
        }
    }
}
