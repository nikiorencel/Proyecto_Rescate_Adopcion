using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_Rescate_Adopcion.Migrations
{
    /// <inheritdoc />
    public partial class AddRelacionesAdopcion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Animal",
                table: "Animal");

            migrationBuilder.RenameTable(
                name: "Animal",
                newName: "Animales");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Adopciones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Pendiente");

            migrationBuilder.AddColumn<string>(
                name: "Notas",
                table: "Adopciones",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioSolicitante",
                table: "Animales",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "NombreAnimal",
                table: "Animales",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Animales",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AddColumn<string>(
                name: "Localidad",
                table: "Animales",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Animales",
                table: "Animales",
                column: "IdSolicitud");

            migrationBuilder.CreateIndex(
                name: "IX_Adopciones_AnimalId",
                table: "Adopciones",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Adopciones_UsuarioId",
                table: "Adopciones",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adopciones_Animales_AnimalId",
                table: "Adopciones",
                column: "AnimalId",
                principalTable: "Animales",
                principalColumn: "IdSolicitud",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Adopciones_Usuarios_UsuarioId",
                table: "Adopciones",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adopciones_Animales_AnimalId",
                table: "Adopciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Adopciones_Usuarios_UsuarioId",
                table: "Adopciones");

            migrationBuilder.DropIndex(
                name: "IX_Adopciones_AnimalId",
                table: "Adopciones");

            migrationBuilder.DropIndex(
                name: "IX_Adopciones_UsuarioId",
                table: "Adopciones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Animales",
                table: "Animales");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Adopciones");

            migrationBuilder.DropColumn(
                name: "Notas",
                table: "Adopciones");

            migrationBuilder.DropColumn(
                name: "Localidad",
                table: "Animales");

            migrationBuilder.RenameTable(
                name: "Animales",
                newName: "Animal");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioSolicitante",
                table: "Animal",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NombreAnimal",
                table: "Animal",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Animal",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Animal",
                table: "Animal",
                column: "IdSolicitud");
        }
    }
}
