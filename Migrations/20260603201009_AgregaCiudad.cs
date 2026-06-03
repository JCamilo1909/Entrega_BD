using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaRegistros.Migrations
{
    /// <inheritdoc />
    public partial class AgregaCiudad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ciudad",
                table: "Compras",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ciudad",
                table: "Compras");
        }
    }
}
