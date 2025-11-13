using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_Rescate_Adopcion.Migrations
{
    /// <inheritdoc />
    public partial class AgregarHistorial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sexo",
                table: "Animales",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Historiales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioSolicitanteId = table.Column<int>(type: "int", nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EstadoSolicitud = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TipoMascota = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NombreMascota = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaResolucion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdopcionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historiales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Historiales_Adopciones_AdopcionId",
                        column: x => x.AdopcionId,
                        principalTable: "Adopciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Historiales_Usuarios_UsuarioSolicitanteId",
                        column: x => x.UsuarioSolicitanteId,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Historiales_AdopcionId",
                table: "Historiales",
                column: "AdopcionId");

            migrationBuilder.CreateIndex(
                name: "IX_Historiales_UsuarioSolicitanteId",
                table: "Historiales",
                column: "UsuarioSolicitanteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Historiales");

            migrationBuilder.DropColumn(
                name: "Sexo",
                table: "Animales");
        }
    }
}
