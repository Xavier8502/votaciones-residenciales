using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotacionesResidenciales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Inmuebles_ConjuntoId_Numero",
                table: "Inmuebles");

            migrationBuilder.CreateIndex(
                name: "IX_Inmuebles_ConjuntoId_Torre_Numero",
                table: "Inmuebles",
                columns: new[] { "ConjuntoId", "Torre", "Numero" },
                unique: true,
                filter: "[Torre] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Inmuebles_ConjuntoId_Torre_Numero",
                table: "Inmuebles");

            migrationBuilder.CreateIndex(
                name: "IX_Inmuebles_ConjuntoId_Numero",
                table: "Inmuebles",
                columns: new[] { "ConjuntoId", "Numero" },
                unique: true);
        }
    }
}
