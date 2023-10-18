using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Assignment_Project.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedDate", "Email", "FirstName", "LastName", "ModifiedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 10, 17, 19, 25, 8, 242, DateTimeKind.Local).AddTicks(7414), "john.doe@example.com", "John", "Doe", new DateTime(2023, 10, 17, 19, 25, 8, 242, DateTimeKind.Local).AddTicks(7435) },
                    { 2, new DateTime(2023, 10, 17, 19, 25, 8, 242, DateTimeKind.Local).AddTicks(7438), "jane.smith@example.com", "Jane", "Smith", new DateTime(2023, 10, 17, 19, 25, 8, 242, DateTimeKind.Local).AddTicks(7439) }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "Address", "Email", "Name", "UserId" },
                values: new object[,]
                {
                    { 1, "123 Main St", "companyA@example.com", "Company A", 1 },
                    { 2, "456 Elm St", "companyB@example.com", "Company B", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);
        }
    }
}
