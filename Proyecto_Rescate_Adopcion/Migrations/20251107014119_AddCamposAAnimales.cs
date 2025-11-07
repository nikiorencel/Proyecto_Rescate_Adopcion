using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_Rescate_Adopcion.Migrations
{
    /// <inheritdoc />
    public partial class AddCamposAAnimales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Adopciones_UsuarioId",
                table: "Adopciones");

            migrationBuilder.RenameColumn(
                name: "UsuarioSolicitante",
                table: "Animales",
                newName: "FotoUrl");

            migrationBuilder.AlterColumn<string>(
                name: "NombreAnimal",
                table: "Animales",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Localidad",
                table: "Animales",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Animales",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Animales",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Edad",
                table: "Animales",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Especie",
                table: "Animales",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaPublicacion",
                table: "Animales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UsuarioCreadorId",
                table: "Animales",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioSolicitanteId",
                table: "Animales",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Animales_UsuarioCreadorId",
                table: "Animales",
                column: "UsuarioCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Animales_UsuarioSolicitanteId",
                table: "Animales",
                column: "UsuarioSolicitanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Adopciones_UsuarioId_AnimalId",
                table: "Adopciones",
                columns: new[] { "UsuarioId", "AnimalId" },
                unique: true,
                filter: "[Estado] = 'Pendiente'");

            migrationBuilder.AddForeignKey(
                name: "FK_Animales_Usuarios_UsuarioCreadorId",
                table: "Animales",
                column: "UsuarioCreadorId",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Animales_Usuarios_UsuarioSolicitanteId",
                table: "Animales",
                column: "UsuarioSolicitanteId",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animales_Usuarios_UsuarioCreadorId",
                table: "Animales");

            migrationBuilder.DropForeignKey(
                name: "FK_Animales_Usuarios_UsuarioSolicitanteId",
                table: "Animales");

            migrationBuilder.DropIndex(
                name: "IX_Animales_UsuarioCreadorId",
                table: "Animales");

            migrationBuilder.DropIndex(
                name: "IX_Animales_UsuarioSolicitanteId",
                table: "Animales");

            migrationBuilder.DropIndex(
                name: "IX_Adopciones_UsuarioId_AnimalId",
                table: "Adopciones");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Animales");

            migrationBuilder.DropColumn(
                name: "Edad",
                table: "Animales");

            migrationBuilder.DropColumn(
                name: "Especie",
                table: "Animales");

            migrationBuilder.DropColumn(
                name: "FechaPublicacion",
                table: "Animales");

            migrationBuilder.DropColumn(
                name: "UsuarioCreadorId",
                table: "Animales");

            migrationBuilder.DropColumn(
                name: "UsuarioSolicitanteId",
                table: "Animales");

            migrationBuilder.RenameColumn(
                name: "FotoUrl",
                table: "Animales",
                newName: "UsuarioSolicitante");

            migrationBuilder.AlterColumn<string>(
                name: "NombreAnimal",
                table: "Animales",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Localidad",
                table: "Animales",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Animales",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Adopciones_UsuarioId",
                table: "Adopciones",
                column: "UsuarioId");
        }
    }
}
