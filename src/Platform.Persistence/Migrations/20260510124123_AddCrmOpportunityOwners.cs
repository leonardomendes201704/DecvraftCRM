using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Platform.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCrmOpportunityOwners : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OwnerEmployeeId",
                table: "OpportunityActivities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerEmployeeId",
                table: "Opportunities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OpportunityActivities_TenantId_OwnerEmployeeId",
                table: "OpportunityActivities",
                columns: new[] { "TenantId", "OwnerEmployeeId" });

            migrationBuilder.CreateIndex(
                name: "IX_Opportunities_TenantId_OwnerEmployeeId",
                table: "Opportunities",
                columns: new[] { "TenantId", "OwnerEmployeeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Opportunities_Employees_TenantId_OwnerEmployeeId",
                table: "Opportunities",
                columns: new[] { "TenantId", "OwnerEmployeeId" },
                principalTable: "Employees",
                principalColumns: new[] { "TenantId", "Id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OpportunityActivities_Employees_TenantId_OwnerEmployeeId",
                table: "OpportunityActivities",
                columns: new[] { "TenantId", "OwnerEmployeeId" },
                principalTable: "Employees",
                principalColumns: new[] { "TenantId", "Id" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Opportunities_Employees_TenantId_OwnerEmployeeId",
                table: "Opportunities");

            migrationBuilder.DropForeignKey(
                name: "FK_OpportunityActivities_Employees_TenantId_OwnerEmployeeId",
                table: "OpportunityActivities");

            migrationBuilder.DropIndex(
                name: "IX_OpportunityActivities_TenantId_OwnerEmployeeId",
                table: "OpportunityActivities");

            migrationBuilder.DropIndex(
                name: "IX_Opportunities_TenantId_OwnerEmployeeId",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "OwnerEmployeeId",
                table: "OpportunityActivities");

            migrationBuilder.DropColumn(
                name: "OwnerEmployeeId",
                table: "Opportunities");
        }
    }
}
