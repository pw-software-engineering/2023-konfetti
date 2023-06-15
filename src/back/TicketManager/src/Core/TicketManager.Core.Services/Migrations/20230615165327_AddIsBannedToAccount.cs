using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketManager.Core.Services.Migrations
{
    /// <inheritdoc />
    public partial class AddIsBannedToAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBanned",
                table: "Accounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBanned",
                table: "Accounts");
        }
    }
}
