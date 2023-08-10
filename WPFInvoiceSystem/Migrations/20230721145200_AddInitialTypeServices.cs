using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WPFInvoiceSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialTypeServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ServicesTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Type A" },
                    { 2, "Type B" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ServicesTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ServicesTypes",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
