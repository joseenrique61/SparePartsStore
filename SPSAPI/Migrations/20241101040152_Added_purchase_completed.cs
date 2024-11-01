using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPSAPI.Migrations
{
    /// <inheritdoc />
    public partial class Added_purchase_completed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PurchaseCompleted",
                table: "PurchaseOrders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchaseCompleted",
                table: "PurchaseOrders");
        }
    }
}
