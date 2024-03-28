using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace User.Manager.API.Migrations
{
    /// <inheritdoc />
    public partial class Addwishlisttables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "178e36b1-5c9d-44f8-9899-fe3b41e22b9e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f2027327-7af3-4f43-bf8c-ad30ca4259ac");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "258e3043-71f2-403e-83f0-9e39afa51780", "1", "Admin", "ADMIN" },
                    { "bcdc39c9-8b2c-4405-8940-1a389f47e256", "2", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "258e3043-71f2-403e-83f0-9e39afa51780");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bcdc39c9-8b2c-4405-8940-1a389f47e256");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "178e36b1-5c9d-44f8-9899-fe3b41e22b9e", "1", "Admin", "ADMIN" },
                    { "f2027327-7af3-4f43-bf8c-ad30ca4259ac", "2", "User", "USER" }
                });
        }
    }
}
