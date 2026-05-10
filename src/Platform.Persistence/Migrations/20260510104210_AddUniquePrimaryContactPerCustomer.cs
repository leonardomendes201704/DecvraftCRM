using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Platform.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUniquePrimaryContactPerCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Contacts_TenantId_CustomerId_IsPrimary",
                table: "Contacts",
                columns: new[] { "TenantId", "CustomerId", "IsPrimary" },
                unique: true,
                filter: "[IsPrimary] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contacts_TenantId_CustomerId_IsPrimary",
                table: "Contacts");
        }
    }
}
