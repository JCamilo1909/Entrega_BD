using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemaRegistros.Migrations
{
    /// <inheritdoc />
    public partial class NuevosModelos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PQRs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tipo = table.Column<string>(type: "TEXT", nullable: false),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Correo = table.Column<string>(type: "TEXT", nullable: false),
                    Mensaje = table.Column<string>(type: "TEXT", nullable: false),
                    Estado = table.Column<string>(type: "TEXT", nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PQRs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Correo = table.Column<string>(type: "TEXT", nullable: false),
                    Contrasena = table.Column<string>(type: "TEXT", nullable: false),
                    Rol = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Contrasena", "Correo", "Nombre", "Rol" },
                values: new object[,]
                {
                    { 1, "admin123", "admin@techstore.com", "Administrador", "Admin" },
                    { 2, "super123", "supervisor@techstore.com", "Supervisor", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PQRs");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
