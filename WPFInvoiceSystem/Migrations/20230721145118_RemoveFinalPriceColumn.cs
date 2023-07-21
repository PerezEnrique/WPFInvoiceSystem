using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPFInvoiceSystem.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFinalPriceColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalPrice",
                table: "InvoicesServices");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FinalPrice",
                table: "InvoicesServices",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
