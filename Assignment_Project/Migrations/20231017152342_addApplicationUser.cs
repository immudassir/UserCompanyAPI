using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assignment_Project.Migrations
{
    /// <inheritdoc />
    public partial class addApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2023, 10, 17, 20, 23, 42, 147, DateTimeKind.Local).AddTicks(2087), new DateTime(2023, 10, 17, 20, 23, 42, 147, DateTimeKind.Local).AddTicks(2099) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2023, 10, 17, 20, 23, 42, 147, DateTimeKind.Local).AddTicks(2102), new DateTime(2023, 10, 17, 20, 23, 42, 147, DateTimeKind.Local).AddTicks(2102) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2023, 10, 17, 19, 25, 8, 242, DateTimeKind.Local).AddTicks(7414), new DateTime(2023, 10, 17, 19, 25, 8, 242, DateTimeKind.Local).AddTicks(7435) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2023, 10, 17, 19, 25, 8, 242, DateTimeKind.Local).AddTicks(7438), new DateTime(2023, 10, 17, 19, 25, 8, 242, DateTimeKind.Local).AddTicks(7439) });
        }
    }
}
