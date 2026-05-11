using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPSAPI.Migrations
{
    /// <inheritdoc />
    public partial class KeyCloak : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KeyCloakId",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KeyCloakId",
                table: "User");
        }
    }
}
