using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TodoApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Todos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "CompletedAt", "CreatedAt", "Description", "IsCompleted", "Priority", "Title" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 7, 28, 10, 37, 41, 170, DateTimeKind.Local).AddTicks(840), "Master the framework", false, 3, "Learn ASP.NET Core" },
                    { 2, null, new DateTime(2026, 7, 28, 10, 37, 41, 171, DateTimeKind.Local).AddTicks(1400), "Create a complete project", false, 2, "Build a Todo App" },
                    { 3, new DateTime(2026, 7, 27, 10, 37, 41, 171, DateTimeKind.Local).AddTicks(1731), new DateTime(2026, 7, 28, 10, 37, 41, 171, DateTimeKind.Local).AddTicks(1414), "Document the code", true, 1, "Write Documentation" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Todos");
        }
    }
}
