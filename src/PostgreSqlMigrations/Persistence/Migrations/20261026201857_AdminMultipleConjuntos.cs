using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotacionesResidenciales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdminMultipleConjuntos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "AdminConjuntoAsignaciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AdminConjuntoId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConjuntoId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreadoEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModificadoEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminConjuntoAsignaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminConjuntoAsignaciones_AdminesConjunto_AdminConjuntoId",
                        column: x => x.AdminConjuntoId,
                        principalTable: "AdminesConjunto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdminConjuntoAsignaciones_Conjuntos_ConjuntoId",
                        column: x => x.ConjuntoId,
                        principalTable: "Conjuntos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.Sql("""
                INSERT INTO "AdminConjuntoAsignaciones"
                    ("Id", "AdminConjuntoId", "ConjuntoId", "CreadoEn", "ModificadoEn", "Activo")
                SELECT
                    gen_random_uuid(),
                    a."Id",
                    a."ConjuntoId",
                    CURRENT_TIMESTAMP,
                    NULL,
                    TRUE
                FROM "AdminesConjunto" a
                WHERE a."ConjuntoId" IS NOT NULL;
                """);

            migrationBuilder.DropForeignKey(
                name: "FK_AdminesConjunto_Conjuntos_ConjuntoId",
                table: "AdminesConjunto");

            migrationBuilder.DropIndex(
                name: "IX_AdminesConjunto_ConjuntoId",
                table: "AdminesConjunto");

            migrationBuilder.DropColumn(
                name: "ConjuntoId",
                table: "AdminesConjunto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ConjuntoId",
                table: "AdminesConjunto",
                type: "uuid",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE "AdminesConjunto" a
                SET "ConjuntoId" = asignacion."ConjuntoId"
                FROM "AdminConjuntoAsignaciones" asignacion
                WHERE asignacion."AdminConjuntoId" = a."Id";
                """);

            migrationBuilder.DropTable(
                name: "AdminConjuntoAsignaciones");

            migrationBuilder.AlterColumn<Guid>(
                name: "ConjuntoId",
                table: "AdminesConjunto",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdminesConjunto_ConjuntoId",
                table: "AdminesConjunto",
                column: "ConjuntoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminesConjunto_Conjuntos_ConjuntoId",
                table: "AdminesConjunto",
                column: "ConjuntoId",
                principalTable: "Conjuntos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
