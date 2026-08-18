using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceService.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvoiceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Invoices",
                newName: "createAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "createAt",
                table: "Invoices",
                newName: "CreateAt");
        }
    }
}
