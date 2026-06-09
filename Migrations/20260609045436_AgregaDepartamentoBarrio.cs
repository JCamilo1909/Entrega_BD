using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaRegistros.Migrations
{
    /// <inheritdoc />
    public partial class AgregaDepartamentoBarrio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Barrio",
                table: "Compras",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Departamento",
                table: "Compras",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Barrio",
                table: "Compras");

            migrationBuilder.DropColumn(
                name: "Departamento",
                table: "Compras");
        }
    }
}
