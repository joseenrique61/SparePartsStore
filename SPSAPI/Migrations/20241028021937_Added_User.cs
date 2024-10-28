using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPSAPI.Migrations
{
    /// <inheritdoc />
    public partial class Added_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Client_Email",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Administrator_Email",
                table: "Administrator");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Administrator");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Administrator");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Administrator");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Client",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Administrator",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Client_UserId",
                table: "Client",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Administrator_UserId",
                table: "Administrator",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Administrator_User_UserId",
                table: "Administrator",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Client_User_UserId",
                table: "Client",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Administrator_User_UserId",
                table: "Administrator");

            migrationBuilder.DropForeignKey(
                name: "FK_Client_User_UserId",
                table: "Client");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_Client_UserId",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Administrator_UserId",
                table: "Administrator");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Administrator");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Client",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Client",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Client",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Administrator",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Administrator",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Administrator",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Client_Email",
                table: "Client",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Administrator_Email",
                table: "Administrator",
                column: "Email",
                unique: true);
        }
    }
}
