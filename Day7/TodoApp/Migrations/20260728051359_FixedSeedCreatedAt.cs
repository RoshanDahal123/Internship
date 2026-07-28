using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApp.Migrations
{
    /// <inheritdoc />
    public partial class FixedSeedCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CompletedAt", "CreatedAt", "Description", "IsCompleted", "Title" },
                values: new object[] { null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Learn the basic foundations", false, "Learn C# advanced function" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 28, 10, 37, 41, 170, DateTimeKind.Local).AddTicks(840));

            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 28, 10, 37, 41, 171, DateTimeKind.Local).AddTicks(1400));

            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CompletedAt", "CreatedAt", "Description", "IsCompleted", "Title" },
                values: new object[] { new DateTime(2026, 7, 27, 10, 37, 41, 171, DateTimeKind.Local).AddTicks(1731), new DateTime(2026, 7, 28, 10, 37, 41, 171, DateTimeKind.Local).AddTicks(1414), "Document the code", true, "Write Documentation" });
        }
    }
}
