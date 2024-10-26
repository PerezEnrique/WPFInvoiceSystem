using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPFInvoiceSystem.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class SeedServicesAndTypesData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO ServicesTypes (Id, Name) VALUES (1, 'Type A'); INSERT INTO ServicesTypes (Id, Name) VALUES (2, 'Type B');");

            migrationBuilder.Sql("INSERT INTO Services (Id, Name, Price, TypeId, IsExempt) VALUES (1, 'Service 1', 10, 1, 0); INSERT INTO Services (Id, Name, Price, TypeId, IsExempt) VALUES (2, 'Service 2', 50, 2, 1);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Services WHERE Id IN (1, 2);");

            migrationBuilder.Sql("DELETE FROM ServicesTypes WHERE Id IN (1, 2);");
        }
    }
}
