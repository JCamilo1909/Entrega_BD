using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaRegistros.Migrations
{
    /// <inheritdoc />
    public partial class AgregaApellidoUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Apellido",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Apellido",
                value: "");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "Apellido",
                value: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apellido",
                table: "Usuarios");
        }
    }
}
