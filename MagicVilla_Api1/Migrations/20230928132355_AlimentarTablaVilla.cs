using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVillaApi1.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa", "imagenUrl" },
                values: new object[,]
                {
                    { 1, "", "Detalle de la villa...", new DateTime(2023, 9, 28, 10, 23, 54, 818, DateTimeKind.Local).AddTicks(8041), new DateTime(2023, 9, 28, 10, 23, 54, 818, DateTimeKind.Local).AddTicks(7986), 50, "Villa Real", 5, 200.0, "" },
                    { 2, "", "Detalle de la villa...", new DateTime(2023, 9, 28, 10, 23, 54, 818, DateTimeKind.Local).AddTicks(8053), new DateTime(2023, 9, 28, 10, 23, 54, 818, DateTimeKind.Local).AddTicks(8050), 40, "Premiun vista a la piscina", 4, 150.0, "" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
