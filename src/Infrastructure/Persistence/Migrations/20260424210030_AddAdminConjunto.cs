using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotacionesResidenciales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminConjunto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminesConjunto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConjuntoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cedula = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PinHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CreadoEn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificadoEn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminesConjunto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminesConjunto_Conjuntos_ConjuntoId",
                        column: x => x.ConjuntoId,
                        principalTable: "Conjuntos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminesConjunto_Cedula",
                table: "AdminesConjunto",
                column: "Cedula",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdminesConjunto_ConjuntoId",
                table: "AdminesConjunto",
                column: "ConjuntoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminesConjunto");
        }
    }
}
