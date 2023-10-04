using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVillaApi1.Migrations
{
    /// <inheritdoc />
    public partial class AgregarNumeroVillaTabla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NumeroVilla",
                columns: table => new
                {
                    VillaNo = table.Column<int>(type: "int", nullable: false),
                    VillaId = table.Column<int>(type: "int", nullable: false),
                    DetalleEspecial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumeroVilla", x => x.VillaNo);
                    table.ForeignKey(
                        name: "FK_NumeroVilla_villas_VillaId",
                        column: x => x.VillaId,
                        principalTable: "villas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 10, 4, 10, 56, 57, 151, DateTimeKind.Local).AddTicks(4210), new DateTime(2023, 10, 4, 10, 56, 57, 151, DateTimeKind.Local).AddTicks(4158) });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 10, 4, 10, 56, 57, 151, DateTimeKind.Local).AddTicks(4222), new DateTime(2023, 10, 4, 10, 56, 57, 151, DateTimeKind.Local).AddTicks(4219) });

            migrationBuilder.CreateIndex(
                name: "IX_NumeroVilla_VillaId",
                table: "NumeroVilla",
                column: "VillaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NumeroVilla");

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 9, 28, 10, 23, 54, 818, DateTimeKind.Local).AddTicks(8041), new DateTime(2023, 9, 28, 10, 23, 54, 818, DateTimeKind.Local).AddTicks(7986) });

            migrationBuilder.UpdateData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 9, 28, 10, 23, 54, 818, DateTimeKind.Local).AddTicks(8053), new DateTime(2023, 9, 28, 10, 23, 54, 818, DateTimeKind.Local).AddTicks(8050) });
        }
    }
}
