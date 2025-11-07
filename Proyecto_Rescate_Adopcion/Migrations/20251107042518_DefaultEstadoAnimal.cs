using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_Rescate_Adopcion.Migrations
{
    /// <inheritdoc />
    public partial class DefaultEstadoAnimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Adopciones_UsuarioId_AnimalId",
                table: "Adopciones");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Animales",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                defaultValue: "Disponible",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Adopciones_UsuarioId",
                table: "Adopciones",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Adopciones_UsuarioId",
                table: "Adopciones");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Animales",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true,
                oldDefaultValue: "Disponible");

            migrationBuilder.CreateIndex(
                name: "IX_Adopciones_UsuarioId_AnimalId",
                table: "Adopciones",
                columns: new[] { "UsuarioId", "AnimalId" },
                unique: true,
                filter: "[Estado] = 'Pendiente'");
        }
    }
}
